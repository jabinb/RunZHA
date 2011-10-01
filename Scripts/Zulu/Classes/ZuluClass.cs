using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Spells;
using RunZH.Zulu.Magic;
using Server.Mobiles;

namespace RunZH.Zulu.Classes
{
    public enum Stat
    {
        Strength,
        Dexterity,
        Intelligence
    }

    public abstract class ZuluClass
    {
        /*
         * const CLASSE_BONUS        := 1.5;
         * const EACH_MUST_REPRESENT := 7.5;
         * const REPRESENT_LEVEL_MOD := 1.0;
         * const AVERAGE_SKILL       := 75;
         * const AVERAGE_LEVEL_MOD   := 15;
         * const BONUS_PER_LEVEL     := 0.25;
        */

        private static readonly double Class_Bonus = 1.5;
        private static readonly double EACH_MUST_REPRESENT = 7.5;
        private static readonly double REPRESENT_LEVEL_MOD = 1.0;
        private static readonly double AVERAGE_SKILL = 75;
        private static readonly double AVERAGE_LEVEL_MOD = 15;
        private static readonly double BONUS_PER_LEVEL = 0.25;

        public static readonly ZuluClass[] Classes = new ZuluClass[]
        {
            Bard.Instance,
            Crafter.Instance,
            Mage.Instance,
            Ranger.Instance,
            Thief.Instance,
            Warrior.Instance,
            PowerPlayer.Instance
        };
        
        #region ZuluClass Members


        protected string _ClassName = null;
        protected List<Type> _RestrictedItems = null;
        protected SkillName[] _ClassSkills = null;
        protected SpellCircle _MaxSpellCircle = SpellCircle.First;
        protected IMagicalProperty[] _RestrictedEnchantments = null;
        protected Stat[] _StatAffinities = null;
        protected Stat[] _StatDifficulties = null;



        public virtual string ClassName
        {
            get { return _ClassName; }
        }

        public virtual List<Type> RestrictedItems
        {
            get { return _RestrictedItems; }
        }

        public virtual SkillName[] ClassSkills
        {
            get { return _ClassSkills; }
        }

        public virtual SpellCircle MaxSpellCircle
        {
            get { return _MaxSpellCircle; }
        }

        public virtual IMagicalProperty[] RestrictedEnchantments
        {
            get { return _RestrictedEnchantments; }
        }

        public virtual Stat[] StatAffinities
        {
            get { return _StatAffinities; }
        }

        public virtual Stat[] StatDifficulties
        {
            get { return _StatDifficulties; }
        }

        public abstract bool IsItemProhibited(Item Item);

        public virtual ushort IsClassed(PlayerMobile Player)
        {
            ushort level = 0;
            bool PowerPlayer = (this.GetType() == typeof(PowerPlayer));

            if (_ClassSkills == null)
                return level;

            if (_ClassSkills.Length == 0)
                return level;

            int SkillsTotal = 0;
            int ClassSkillsTotal = 0;

            foreach (SkillName Skill in _ClassSkills)
            {
                ClassSkillsTotal += (int)Math.Floor(Player.Skills[Skill].NonRacialValue);
            }
            /*
            for (int i = 0; i < Player.Skills.Length; i++)
            {
                SkillsTotal += (int)Math.Floor(Player.Skills[i].Value);
            }*/
            SkillsTotal = (int)Math.Floor(Player.Skills.Total / 10.0);

            if (ClassSkillsTotal < (AVERAGE_SKILL * _ClassSkills.Length))
                return level;
            else if (!PowerPlayer && ClassSkillsTotal < ((SkillsTotal * _ClassSkills.Length * EACH_MUST_REPRESENT) * 0.01))
                return level;
            else
            {
                level = 1;
                double represent = EACH_MUST_REPRESENT + REPRESENT_LEVEL_MOD;
                int percent = (int)Math.Floor( SkillsTotal * _ClassSkills.Length * represent * 0.01);
                int average_t = (int)Math.Floor((AVERAGE_SKILL + AVERAGE_LEVEL_MOD) * _ClassSkills.Length);
                while ((ClassSkillsTotal >= average_t) && ((ClassSkillsTotal >= percent) || PowerPlayer) )
                {
                    level += 1;
                    represent += REPRESENT_LEVEL_MOD;
                    percent = (int)Math.Floor(SkillsTotal * _ClassSkills.Length * represent * 0.01);
                    average_t = (int)Math.Floor(average_t + AVERAGE_LEVEL_MOD * _ClassSkills.Length);
                }
            }

            return level;
        }

        /*
         * function IsFromThatClasse( who , classe_skills )

	        var classe	:= 0;
	        var total	:= 0;
	        var number := Len( classe_skills );

	        for i := 0 to SKILLID__HIGHEST
		        var amount := GetSkill( who , i );
		        total := total + amount;
		        if( i in classe_skills )
			        classe := classe + amount;
		        endif
	        endfor

	        if( classe < AVERAGE_SKILL * number )
		        return 0;
	        elseif( classe < CInt(total * number * EACH_MUST_REPRESENT * 0.01) )
		        return 0;
	        else
		        var level     := 1;
		        var represent := EACH_MUST_REPRESENT + REPRESENT_LEVEL_MOD;
		        var percent   := CInt( total * number * represent * 0.01 );
		        var average_t := CInt( (AVERAGE_SKILL + AVERAGE_LEVEL_MOD) * number );
		        while( (classe >= average_t) and (classe >= percent) )
			        level     := level + 1;
			        represent := CDbl( represent + REPRESENT_LEVEL_MOD );
			        percent   := CInt( total * number * represent * 0.01 );
			        average_t := CInt( average_t + AVERAGE_LEVEL_MOD * number );
		        endwhile

		        return level;
	        endif

            endfunction
         * 
        */

        #endregion
    }
}
