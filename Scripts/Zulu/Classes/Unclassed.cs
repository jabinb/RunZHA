using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Spells;
using RunZH.Zulu.Magic;

namespace RunZH.Zulu.Classes
{
    public sealed class Unclassed : ZuluClass
    {
        private Unclassed()
        {
            _ClassName = "Unclassed";
            _RestrictedItems = new List<Type>();
            _ClassSkills = new SkillName[] { };
            _MaxSpellCircle = SpellCircle.Eighth;
            _RestrictedEnchantments = new IMagicalProperty[] { };
            _StatAffinities = new Stat[] { };
            _StatDifficulties = new Stat[] { };
        }

        private static readonly Unclassed _Instance = new Unclassed();


        public static Unclassed Instance
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
