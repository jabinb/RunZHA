using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Spells;
using RunZH.Zulu.Magic;

namespace RunZH.Zulu.Classes
{
    public class Mage : ZuluClass
    {
        private Mage()
        {
            _ClassName = "Mage";
            _RestrictedItems = new List<Type>();
            _ClassSkills = new SkillName[] 
            {
                SkillName.Magery,
                SkillName.Meditation,
                SkillName.MagicResist,
                SkillName.Alchemy,
                SkillName.EvalInt,
                SkillName.SpiritSpeak,
                SkillName.Inscribe,
                SkillName.ItemID
            };
            _MaxSpellCircle = SpellCircle.Eighth;
            _RestrictedEnchantments = new IMagicalProperty[] { };
            _StatAffinities = new Stat[] { Stat.Intelligence };
            _StatDifficulties = new Stat[] { Stat.Strength, Stat.Dexterity };
        }

        private static readonly Mage _Instance = new Mage();


        public static Mage Instance
        {
            get
            {
                return _Instance;
            }
        }

        #region ZuluClass Members

        public override bool IsItemProhibited(Server.Item Item)
        {
            return false;
        }

        #endregion

        public override string ToString()
        {
            return _ClassName;
        }


    }
}
