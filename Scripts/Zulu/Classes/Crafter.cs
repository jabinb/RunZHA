using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Spells;
using RunZH.Zulu.Magic;

namespace RunZH.Zulu.Classes
{
    public class Crafter : ZuluClass
    {
        private Crafter()
        {
            _ClassName = "Crafter";
            _RestrictedItems = new List<Type>();
            _ClassSkills = new SkillName[] 
            {
                SkillName.Blacksmith,
                SkillName.ArmsLore,
                SkillName.Mining,
                SkillName.Lumberjacking,
                SkillName.Tailoring,
                SkillName.Tinkering,
                SkillName.Fletching,
                SkillName.Carpentry
            };
            _MaxSpellCircle = SpellCircle.Eighth;
            _RestrictedEnchantments = new IMagicalProperty[] { };
            _StatAffinities = new Stat[] { Stat.Strength };
            _StatDifficulties = new Stat[] { };
        }

        private static readonly Crafter _Instance = new Crafter();


        public static Crafter Instance
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

