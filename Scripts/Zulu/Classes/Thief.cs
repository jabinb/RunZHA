using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Spells;
using RunZH.Zulu.Magic;

namespace RunZH.Zulu.Classes
{
    public class Thief : ZuluClass
    {
        private Thief()
        {
            _ClassName = "Thief";
            _RestrictedItems = new List<Type>();
            _ClassSkills = new SkillName[] 
            {
                SkillName.Stealing,
                SkillName.Hiding,
                SkillName.Lockpicking,
                SkillName.Stealth,
                SkillName.RemoveTrap,
                SkillName.DetectHidden,
                SkillName.Poisoning,
                SkillName.Snooping
            };
            _MaxSpellCircle = SpellCircle.Eighth;
            _RestrictedEnchantments = new IMagicalProperty[] { };
            _StatAffinities = new Stat[] { Stat.Dexterity };
            _StatDifficulties = new Stat[] { Stat.Strength };
        }

        private static readonly Thief _Instance = new Thief();


        public static Thief Instance
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
