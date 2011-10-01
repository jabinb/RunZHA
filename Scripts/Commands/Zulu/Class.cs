using System;
using System.Collections.Generic;
using System.Text;
using Server.Commands;
using Server;
using RunZH.Zulu.Classes;
using Server.Mobiles;

namespace RunZH.Commands.Zulu
{
    class Class
    {

        public static void Initialize()
        {
            CommandSystem.Register("Class", AccessLevel.Player, new CommandEventHandler(Class_OnCommand));
        }

        [Usage("Class")]
        [Description("Sets your skills to a chosen class.")]
        private static void Class_OnCommand(CommandEventArgs e)
        {
            PlayerMobile Player = e.Mobile as PlayerMobile;

            if (Player == null)
                return;

            if (e.Length < 2)
            {
                Player.SendMessage("Class <classname> <skillpoints>");
                return;
            }

            int Points = 0;
            string TargetClass = e.Arguments[0];

            if (!int.TryParse(e.Arguments[1], out Points))
                return;

            

            foreach (ZuluClass Classe in ZuluClass.Classes)
            {
                if (TargetClass == Classe.ClassName)
                {
                    foreach (SkillName Skill in Classe.ClassSkills)
                        Player.Skills[Skill].Base = Points;
                    break;
                }
            }


            Player.SendMessage("You are now a level {0} {1}", Player.ZuluClassLevel, Player.ZuluClass);
        }
    }
}

