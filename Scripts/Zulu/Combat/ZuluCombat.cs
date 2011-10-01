using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Items;
using RunZH.Zulu.Hitscripts;
using System.Text.RegularExpressions;
using System.Collections;

namespace RunZH.Zulu
{
    public static class ZuluCombat
    {


        public static int CombatHit(Mobile Attacker, Mobile Defender, BaseWeapon Weapon, bool UseHitscript)
        {
            bool ApplyDamage = true;

            int Damage = 0;

            

            if (Weapon is BaseRanged)
                Damage = RangedHit(Attacker, Defender, Weapon, UseHitscript);
            else if (Weapon is BaseMeleeWeapon)
                Damage = MeleeHit(Attacker, Defender, Weapon);


            //(100-weaponType.Speed)/10

            //long Ticks = ((100 - Weapon.Speed) / 10) * TimeSpan.TicksPerMillisecond;

            

            if (UseHitscript)
            {
                ApplyDamage = HitscriptManager.Run(Weapon.Hitscript, Attacker, Defender, Weapon, ref Damage);
            }

            if (!ApplyDamage)
                Damage = 0;
            //Timer.DelayCall(new TimeSpan(Ticks), new TimerStateCallback(ApplyCombatDamage), Damage);

            if (Damage < 0)
                Damage = 0;

            Attacker.SendMessage("Returning Damage is {0}", Damage);

            return Damage;
        }

        private static void ApplyCombatDamage(object Damage)
        {

        }

        private static int MeleeHit(Mobile Attacker, Mobile Defender, BaseWeapon Weapon)
        {

            bool Creature = (Defender is Server.Mobiles.BaseCreature);

            if (Weapon.Skill != SkillName.Wrestling)
            {
                if (Utility.Random(100) == 1)
                    Weapon.HitPoints--;
            }
            
            double ParrySkill = Defender.Skills[SkillName.Parry].Value;
            double ParryChance = ParrySkill + Defender.Dex * 0.2;
            ParryChance *= 0.2;
            
            
            double PierceChance = Attacker.Skills[SkillName.Anatomy].Value;
            PierceChance += Attacker.Skills[Weapon.Skill].Value;

            if(!Creature)
                PierceChance += Attacker.Str;

            PierceChance *= 0.030;



            //double BaseDamage = Utility.Dice(1, Weapon.MaxDamage, GetDamageLevel(Weapon.DamageLevel));
            double BaseDamage = Weapon.DiceRoll.Evaluate();
            

            Attacker.SendMessage("BaseDamage rolled {0}", BaseDamage);

            double Multi;
            Multi = Attacker.Skills[SkillName.Tactics].Value + 50;
            Multi += (Attacker.Skills[SkillName.Anatomy].Value + 25) / 5;
            Multi += Attacker.Str * 0.2;
            Multi *= 0.01;

            if (Creature)
                Multi += 2.0;

            BaseDamage *= Multi;

            Attacker.SendMessage("BaseDamage is {0}", BaseDamage);
            
            BaseShield Shield = Defender.FindItemOnLayer(Layer.TwoHanded) as BaseShield;
            if (Shield != null)
            {
                //double ParryChance = Defender.Skills[SkillName.Parry].Value / 200;
                //if (Utility.RandomDouble() < ParryChance)
                Attacker.SendMessage("Defender parry chance is {0}-{1}={2}", ParryChance, PierceChance, ParryChance - PierceChance );
                ParryChance = (ParryChance - PierceChance) / 100;

                if (ParryChance < 0.0)
                    ParryChance = 0.1;

                if (Defender.CheckSkill(SkillName.Parry, ParryChance))
                {
                    if (Utility.Random(100) == 1)
                        Shield.HitPoints--;

                    Defender.SendMessage("You successfully parry an attack!");
                    BaseDamage -= Shield.ArmorRating;

                    Attacker.SendMessage("Defender Parried. BaseDamage is {0}", BaseDamage);
                }
            }

            BaseArmor ArmorPiece = GetRandomArmor(Defender);
            if (ArmorPiece != null)
            {
                double Blocked = ArmorPiece.ArmorRating + Defender.VirtualArmorMod;
                double Absorbed = Blocked / 2;
                Blocked -= Absorbed;
                Absorbed += Utility.Random((int)Math.Floor(Blocked + 1));
                BaseDamage -= Absorbed;

                Attacker.SendMessage("{0} Armor absorbed {1}. BaseDamage is {2}", ArmorPiece, Absorbed, BaseDamage);

                if (Utility.Random(100) == 1)
                    ArmorPiece.HitPoints--;
            }

            if (BaseDamage >= 2.0 && !(Defender is Server.Mobiles.BaseCreature))
                BaseDamage *= 0.5;

            

            Attacker.SendMessage("Final BaseDamage is {0}", BaseDamage);


            return (int)Math.Floor(BaseDamage);
        }

        public static int GetDamageLevel(WeaponDamageLevel DamageLevel)
        {
            int Damage = 0;
            switch (DamageLevel)
            {
                case WeaponDamageLevel.Ruin:
                    Damage = 5;
                    break;
                case WeaponDamageLevel.Might:
                    Damage = 10;
                    break;
                case WeaponDamageLevel.Force:
                    Damage = 15;
                    break;
                case WeaponDamageLevel.Power:
                    Damage = 20;
                    break;
                case WeaponDamageLevel.Vanq:
                    Damage = 25;
                    break;
                case WeaponDamageLevel.Devastation:
                    Damage = 30;
                    break;
            }
            return Damage;
        }

        private static BaseArmor GetRandomArmor(Mobile m)
        {
            List<Item> EquippedItems = m.Items.FindAll(p => p is BaseArmor);

            
            /*
            foreach (Item i in m.Items)
            {
                switch (i.Layer)
                {
                    case Layer.OneHanded: break;
                    case Layer.TwoHanded: break;
                    case Layer.Helm: break;
                    case Layer.Gloves: break;
                    case Layer.Neck: break;
                    case Layer.Waist: break;
                    case Layer.InnerTorso: break;
                    case Layer.MiddleTorso: break;
                    case Layer.Arms: break;
                    case Layer.OuterTorso: break;
                    case Layer.OuterLegs: break;
                    case Layer.InnerLegs: break;
                    default:
                        continue;
                        break;
                }

                EquippedItems.Add(i);
            }*/

            int Random = Utility.Random(EquippedItems.Count);

            if (EquippedItems.Count > 0)
                return (BaseArmor)EquippedItems[Random];
            else
                return null;
        }

        private static int RangedHit(Mobile Attacker, Mobile Defender, BaseWeapon Weapon, bool UseHitscript)
        {
            return 0;
        }


        public class DiceRoll
        {
            public enum DiceOperator
            {
                None,
                Minus,
                Add
            }

            public int Dice = 1;
            public int Sides;
            public int Modifier;


            private static readonly string unicodeminus = " − ";
            private static readonly Regex DiceRegex = 
                new Regex(@"^(?<dice>[0-9]*d{1})?(?<sides>[0-9]*)?(?<operator>\+|\-)?(?<modifier>[0-9]*)(?<nextoperator>\+|\-?)");

            private Dictionary<DiceRoll, DiceOperator> m_ChildDice;

            public Dictionary<DiceRoll, DiceOperator> ChildDice
            {
                get 
                { 
                    if(m_ChildDice == null)
                        m_ChildDice = new Dictionary<DiceRoll, DiceOperator>();

                    return m_ChildDice; 
                }
            }

            public DiceRoll(int Dice, int Sides, int Modifier)
            {
                this.Dice = Dice;
                this.Sides = Sides;
                this.Modifier = Modifier;
            }

            public DiceRoll(string expression)
            {
                expression = expression.ToLowerInvariant().Replace(" ", String.Empty);

                Match Match = ParseExpression(expression, ref Dice, ref Sides, ref Modifier);

                if (Match.Success)
                {
                    string NextOperator = Match.Groups["nextoperator"].Value;
                    while (!String.IsNullOrEmpty(NextOperator))
                    {
                        int nextDice = 0;
                        int nextSides = 0;
                        int nextModifier = 0;


                        string NextDiceExpression = expression.Remove(0, Match.Length);

                        Match = ParseExpression(NextDiceExpression, ref nextDice, ref nextSides, ref nextModifier);

                        if (Match.Success)
                        {
                            ChildDice.Add(new DiceRoll(nextDice, nextSides, nextModifier), (NextOperator == "-") ? DiceOperator.Minus : DiceOperator.Add);
                        }

                        NextOperator = Match.Groups["nextoperator"].Value;
                        expression = NextDiceExpression;
                    }
                }
                else
                    throw new ArgumentException("Failed to parse dice expression. Provided expression did not match established pattern");
            }

            private Match ParseExpression(string expression, ref int Dice, ref int Sides, ref int Modifier)
            {
                Match Match = DiceRegex.Match(expression);

                if (Match.Success)
                {

                    string strDice = Match.Groups["dice"].Value.Replace("d", "");
                    int.TryParse(strDice, out Dice);

                    string strSides = Match.Groups["sides"].Value;
                    Sides = int.Parse(strSides);


                    string strOperator = Match.Groups["operator"].Value;
                    string strModifier = Match.Groups["modifier"].Value;
                    if (strOperator != String.Empty && strModifier != string.Empty)
                        Modifier = int.Parse(strOperator + strModifier);


                }

                return Match;
            }

            public DiceRoll AddChildDice(int Dice, int Sides, int Modifier, DiceOperator Operator)
            {

                DiceRoll Child = new DiceRoll(Dice, Sides, Modifier);

                ChildDice.Add(Child, Operator);

                return Child;
            }

            public int Evaluate()
            {
                int Result = Utility.Dice(Dice, Sides, Modifier);

                if(m_ChildDice != null)
                {
                    foreach(KeyValuePair<DiceRoll, DiceOperator> pair in ChildDice)
                    {
                        if(pair.Value == DiceOperator.Add)
                            Result += pair.Key.Evaluate();
                        else if (pair.Value == DiceOperator.Minus)
                            Result -= pair.Key.Evaluate();
                    }
                }

                if (Result < 0)
                    Result = 0;

                return Result;
            }

            public decimal GetCalculatedAverage()
            {
                decimal Result = this.Dice * ((this.Sides + 1.0m) / 2.0m);

                if(m_ChildDice != null)
                {
                    foreach(KeyValuePair<DiceRoll, DiceOperator> pair in ChildDice)
                    {
                        if(pair.Value == DiceOperator.Add)
                            Result += pair.Key.GetCalculatedAverage();
                        else if (pair.Value == DiceOperator.Minus)
                            Result -= pair.Key.GetCalculatedAverage();
                    }
                }

                return Result;
            }

            public int GetMaximumValue()
            {
                int Result = this.Dice * (this.Sides + this.Modifier);

                if (m_ChildDice != null)
                {
                    foreach (KeyValuePair<DiceRoll, DiceOperator> pair in ChildDice)
                    {
                        if (pair.Value == DiceOperator.Add)
                            Result += pair.Key.GetMaximumValue();
                        else if (pair.Value == DiceOperator.Minus)
                            Result -= pair.Key.GetMaximumValue();
                    }
                }
                return Result;
            }

            public int GetMinimumValue()
            {
                int Result = this.Dice + this.Modifier;

                if (m_ChildDice != null)
                {
                    foreach (KeyValuePair<DiceRoll, DiceOperator> pair in ChildDice)
                    {
                        if (pair.Value == DiceOperator.Add)
                            Result += pair.Key.GetMinimumValue();
                        else if (pair.Value == DiceOperator.Minus)
                            Result -= pair.Key.GetMinimumValue();
                    }
                }

                return Result;
            }

            public override string ToString()
            {
                String Result = String.Format("{0}d{1}{2:+#;-#;#}", Dice, Sides, Modifier);

                if (m_ChildDice != null)
                {
                    foreach (KeyValuePair<DiceRoll, DiceOperator> pair in ChildDice)
                    {
                        if (pair.Value == DiceOperator.Add)
                            Result += "+";
                        else if (pair.Value == DiceOperator.Minus)
                            Result += "-"; //Unicode minus, not a dash ( - vs − )

                        Result += pair.Key.ToString();
                    }
                }

                return Result;
            }
        }
    }
}
