using System;
using System.Collections.Generic;
using System.Text;
using Server.Commands;
using Server;
using RunZH.Zulu.Classes;
using Server.Mobiles;

namespace RunZH.Commands
{
    class ShowClasse
    {

        public static void Initialize()
        {
            CommandSystem.Register("Showclasse", AccessLevel.Player, new CommandEventHandler(Showclasse_OnCommand));
        }

        [Usage("Showclasse")]
        [Description("Shows what classe level you are.")]
        private static void Showclasse_OnCommand(CommandEventArgs e)
        {
            PlayerMobile Player = e.Mobile as PlayerMobile;

            if (Player == null)
                return;

            Player.SendMessage(String.Format("You are currently a level {0} {1}", Player.ZuluClassLevel, Player.ZuluClass));
        }
    }
}
