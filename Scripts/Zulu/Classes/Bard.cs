using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Spells;
using RunZH.Zulu.Magic;

namespace RunZH.Zulu.Classes
{
    public class Bard : ZuluClass
    {
        private Bard()
        {
            _ClassName = "Bard";
            _RestrictedItems = new List<Type>();
            _ClassSkills = new SkillName[] 
            {
                SkillName.Provocation,
                SkillName.Musicianship,
                SkillName.Peacemaking,
                SkillName.Begging,
                SkillName.Cartography,
                SkillName.Discordance,
                SkillName.TasteID,
                SkillName.Herding
            };
            _MaxSpellCircle = SpellCircle.Eighth;
            _RestrictedEnchantments = new IMagicalProperty[] { };
            _StatAffinities = new Stat[] { Stat.Intelligence };
            _StatDifficulties = new Stat[] { Stat.Strength, Stat.Dexterity };
        }

        private static readonly Bard _Instance = new Bard();


        public static Bard Instance
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
