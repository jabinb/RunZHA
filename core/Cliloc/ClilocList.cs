using System;
using System.IO;
using System.Text;
using System.Collections;

namespace Ultima
{
    public class ClilocList
    {
        public static readonly string ClilocPath = Path.Combine("Data/", "Cliloc.");

        private Hashtable m_Table;
        private ClilocEntry[] m_Entries;
        private string m_Language;

        public ClilocEntry[] Entries { get { return m_Entries; } }
        public Hashtable Table { get { return m_Table; } }
        public string Language { get { return m_Language; } }

        private static byte[] m_Buffer = new byte[1024];

        public ClilocList(string language)
        {
            m_Language = language;
            m_Table = new Hashtable();

            string path = ClilocPath + language;

            if (path == null)
            {
                m_Entries = new ClilocEntry[0];
                return;
            }

            ArrayList list = new ArrayList();

            using (BinaryReader bin = new BinaryReader(new PeekableStream(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))))
            {
                PeekableStream PeekableStream = bin.BaseStream as PeekableStream;
                bin.ReadInt32();
                bin.ReadInt16();

                while (PeekableStream.PeekByte() != -1)
                {
                    int number = bin.ReadInt32();
                    bin.ReadByte();
                    int length = bin.ReadInt16();

                    if (length > m_Buffer.Length)
                        m_Buffer = new byte[(length + 1023) & ~1023];

                    bin.Read(m_Buffer, 0, length);
                    string text = Encoding.UTF8.GetString(m_Buffer, 0, length);

                    list.Add(new ClilocEntry(number, text));
                    m_Table[number] = text;
                }
            }

            m_Entries = (ClilocEntry[])list.ToArray(typeof(ClilocEntry));
        }

        private class PeekableStream : Stream
        {
            bool hasPeek;
            Stream input;
            byte[] peeked;

            public PeekableStream(Stream input)
            {
                this.input = input;
                this.peeked = new byte[1];
            }

            public override bool CanRead
            {
                get { return input.CanRead; }
            }

            public override bool CanSeek
            {
                get { return input.CanSeek; }
            }

            public override bool CanWrite
            {
                get { return false; }
            }

            public override void Flush()
            {
                throw new NotSupportedException();
            }

            public override long Length
            {
                get { return input.Length; }
            }

            public int PeekByte()
            {
                if (!hasPeek)
                    hasPeek = Read(peeked, 0, 1) == 1;
                return hasPeek ? peeked[0] : -1;
            }

            public override int ReadByte()
            {
                if (hasPeek)
                {
                    hasPeek = false;
                    return peeked[0];
                }
                return base.ReadByte();
            }

            public override long Position
            {
                get
                {
                    if (hasPeek)
                        return input.Position - 1;
                    return input.Position;
                }
                set
                {
                    if (value != Position)
                    {
                        hasPeek = false;
                        input.Position = value;
                    }
                }
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                int read = 0;
                if (hasPeek && count > 0)
                {
                    hasPeek = false;
                    buffer[offset] = peeked[0];
                    offset++;
                    count--;
                    read++;
                }
                read += input.Read(buffer, offset, count);
                return read;
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                long val;
                if (hasPeek && origin == SeekOrigin.Current)
                    val = input.Seek(offset - 1, origin);
                else
                    val = input.Seek(offset, origin);
                hasPeek = false;
                return val;
            }

            public override void SetLength(long value)
            {
                throw new NotSupportedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
            }
        }
    }

    public class ClilocEntry
    {
        private int m_Number;
        private string m_Text;

        public int Number { get { return m_Number; } }
        public string Text { get { return m_Text; } }

        public ClilocEntry(int number, string text)
        {
            m_Number = number;
            m_Text = text;
        }
    }


}