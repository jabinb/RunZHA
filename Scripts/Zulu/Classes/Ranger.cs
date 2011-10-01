using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Spells;
using RunZH.Zulu.Magic;

namespace RunZH.Zulu.Classes
{
    public class Ranger : ZuluClass
    {
        private Ranger()
        {
            _ClassName = "Ranger";
            _RestrictedItems = new List<Type>();
            _ClassSkills = new SkillName[] 
            {
                SkillName.Archery,
                SkillName.Tracking,
                SkillName.AnimalLore,
                SkillName.Fishing,
                SkillName.Cooking,
                SkillName.AnimalTaming,
                SkillName.Veterinary,
                SkillName.Camping
            };
            _MaxSpellCircle = SpellCircle.Eighth;
            _RestrictedEnchantments = new IMagicalProperty[] { };
            _StatAffinities = new Stat[] { Stat.Intelligence };
            _StatDifficulties = new Stat[] { Stat.Strength, Stat.Dexterity };
        }

        private static readonly Ranger _Instance = new Ranger();


        public static Ranger Instance
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
