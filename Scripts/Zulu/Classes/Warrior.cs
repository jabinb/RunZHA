using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Spells;
using RunZH.Zulu.Magic;

namespace RunZH.Zulu.Classes
{
    public class Warrior : ZuluClass
    {
        private Warrior()
        {
            _ClassName = "Warrior";
            _RestrictedItems = new List<Type>();
            _ClassSkills = new SkillName[] 
            {
                SkillName.Healing,
                SkillName.Fencing,
                SkillName.Swords,
                SkillName.Macing,
                SkillName.Parry,
                SkillName.Tactics,
                SkillName.Anatomy,
                SkillName.Wrestling
            };
            _MaxSpellCircle = SpellCircle.Eighth;
            _RestrictedEnchantments = new IMagicalProperty[] { };
            _StatAffinities = new Stat[] { Stat.Strength };
            _StatDifficulties = new Stat[] { Stat.Intelligence };
        }

        private static readonly Warrior _Instance = new Warrior();


        public static Warrior Instance
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
