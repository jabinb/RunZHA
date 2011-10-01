using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Fourth;
using Server.Spells.First;

namespace RunZH.Zulu.Hitscripts
{
    public static class BlackrockHitscript
    {

        
        

        public static bool Run(Mobile Attacker, Mobile Defender, BaseWeapon Weapon, ref int BaseDamage)
        {
            Effects.SendLocationParticles(EffectItem.Create(Defender.Location, Defender.Map, EffectItem.DefaultDuration), 0x3728, 8, 20, 5042);
            Effects.PlaySound(Defender, Defender.Map, 0x201);

            bool Summoned = false;

            if (Defender is BaseCreature)
            {
                BaseCreature Creature = Defender as BaseCreature;
                Summoned = Creature.Summoned;
                if (Summoned)
                {
                    if(Attacker.NetState != null)
                        Attacker.PrivateOverheadMessage(Server.Network.MessageType.Regular, 0x03B2, true,
                            "Your weapon causes the creature to dissipate on impact!", Attacker.NetState);

                    Creature.Delete();
                    return true;
                }
            }

            ZuluUtil.WipeMods(Defender);

            Defender.Mana = 0;

            if (Weapon.DamageLevel == WeaponDamageLevel.Regular)
                BaseDamage += ZuluCombat.GetDamageLevel(WeaponDamageLevel.Devastation);
            

            

            return true;
        }

    }
}
