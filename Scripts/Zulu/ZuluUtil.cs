using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Mobiles;
using Server.Spells;
using Server.Items;
using Server.Spells.Fifth;
using Server.Spells.Seventh;
using Server.Spells.Fourth;

namespace RunZH.Zulu
{
    public static class ZuluUtil
    {

        private static Type[] DispelActions = new Type[]
        {
            typeof(Server.Spells.Fourth.ArchProtectionSpell),
            typeof(Server.Spells.Seventh.PolymorphSpell),
            typeof(Server.Spells.Fifth.IncognitoSpell)
        };

        public static void WipeMods(Mobile m)
        {
            PlayerMobile pm = m as PlayerMobile;

            m.DisruptiveAction();

            foreach (StatType Stat in Enum.GetValues(typeof(StatType)))
            {
                string name = String.Format("[Magic] {0} Offset", Stat);
                if (m.GetStatMod(name) != null)
                    m.RemoveStatMod(name);
            }

            if (TransformationSpellHelper.UnderTransformation(m))
                TransformationSpellHelper.RemoveContext(m, true);

            m.HueMod = -1;
            m.NameMod = null;
            

            PolymorphSpell.StopTimer(m);
            IncognitoSpell.StopTimer(m);
            DisguiseGump.StopTimer(m);


            foreach (Type T in DispelActions)
                m.EndAction(T);

            m.BodyMod = 0;
            m.HueMod = -1;


            

            if (pm != null)
            {
                if (pm.BuffTable != null)
                {
                    List<BuffInfo> list = new List<BuffInfo>();

                    foreach (BuffInfo buff in pm.BuffTable.Values)
                    {
                        if (!buff.RetainThroughDeath)
                        {
                            list.Add(buff);
                        }
                    }

                    for (int i = 0; i < list.Count; i++)
                    {
                        pm.RemoveBuff(list[i]);
                    }
                }

                pm.SavagePaintExpiration = TimeSpan.Zero;
                pm.SetHairMods(-1, -1);

                pm.ResendBuffs();
                
            }

            

            m.VirtualArmorMod = 0;


            BaseArmor.ValidateMobile(m);
            BaseClothing.ValidateMobile(m);
            
        }
    }
}
