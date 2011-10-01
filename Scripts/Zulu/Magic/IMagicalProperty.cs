using System;
using System.Collections.Generic;
using System.Text;

namespace RunZH.Zulu.Magic
{
    public enum MagicalAffix
    {
        Prefix,
        Suffix
    }

    public interface IMagicalProperty
    {
        int NamingOrder { get; }

        MagicalAffix Affix { get; }

        string Name { get; }
    }
}
