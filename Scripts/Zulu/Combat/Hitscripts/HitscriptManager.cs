using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Items;

namespace RunZH.Zulu.Hitscripts
{
    public enum ZuluHitscript
    {
        None = 0,
        Blackrock = 1
    }

    public static class HitscriptManager
    {
        private delegate bool Call(Mobile Attacker, Mobile Defender, BaseWeapon Weapon, ref int BaseDamage);

        private static Call[] HitscriptCalls = new Call[]
        {
            EmptyCall,
            BlackrockHitscript.Run
        };

        public static bool Run(ZuluHitscript Hitscript, Mobile Attacker, Mobile Defender, BaseWeapon Weapon, ref int BaseDamage)
        {

            bool RunDamage = true;

            try
            {
                Call HitscriptCall = HitscriptCalls[(int)Hitscript];

                HitscriptCall(Attacker, Defender, Weapon, ref BaseDamage);
            }
            catch { Console.WriteLine("Couldnt run hitscript {0}", Hitscript); }

            return RunDamage;
        }

        public static bool EmptyCall(Mobile Attacker, Mobile Defender, BaseWeapon Weapon, ref int BaseDamage)
        {
            return true;
        }

    }
}
