using System;
using System.IO;
using System.Text;
using Magic.Core;
using Magic.Core.Network;
using Magic.Helpers;
using Magic.Logic;
using Magic.PacketProcessing.Commands.Client;
using Magic.PacketProcessing.Messages.Server;
using static Magic.Logic.ClientAvatar;

namespace Magic.PacketProcessing.Messages.Client
{
    internal class GoHomeMessage : Message
    {
        public int State;

        public GoHomeMessage(PacketProcessing.Client client, PacketReader reader) : base(client, reader)
        {
            // Space
        }

        public override void Decode()
        {
            State = Reader.ReadInt32();
        }

        public override void Process(Level level)
        {
            if (level.GetPlayerAvatar().State == UserState.PVP)
            {
                var info = default(AttackInfo);
                if (!level.GetPlayerAvatar().AttackingInfo.TryGetValue(level.GetPlayerAvatar().GetId(), out info))
                {
                    Logger.Write("Unable to obtain attack info.");
                }
                else
                {
                    var defender = info.Defender;
                    var attacker = info.Attacker;

                    var lost = info.Lost;
                    var reward = info.Reward;

                    var usedtroop = info.UsedTroop;

                    int attackerscore = attacker.GetPlayerAvatar().GetScore();
                    int defenderscore = defender.GetPlayerAvatar().GetScore();

                    if (defender.GetPlayerAvatar().GetScore() > 0)
                        defender.GetPlayerAvatar().SetScore(defenderscore -= lost);

                    //Logger.Write("Used troop type: " + usedtroop.Count);
                    //foreach(var a in usedtroop)
                    //{
                    //    Logger.Write("Troop Name: " + a.Data.GetName());
                    //    Logger.Write("Troop Used Value: " + a.Value);
                    //}
                    attacker.GetPlayerAvatar().SetScore(attackerscore += reward);
                    attacker.GetPlayerAvatar().AttackingInfo.Clear(); //Since we use userid for now,We need to clear to prevent overlapping
                    Resources(attacker);

                    DatabaseManager.Instance.Save(attacker);
                    DatabaseManager.Instance.Save(defender);
                }
            }

            if (level.GetPlayerAvatar().State == UserState.CHA)
            {
                //Attack 
            }

            if (State == 1)
            {
                var player = level.GetPlayerAvatar();
                player.State = UserState.Editmode;
            }
            else
            {
                var player = level.GetPlayerAvatar();
                player.State = UserState.Home;
            }

            level.Tick();

            var alliance = ObjectManager.GetAlliance(level.GetPlayerAvatar().GetAllianceId());
            new OwnHomeDataMessage(Client, level).Send();
            if (alliance != null)
            {
                new AllianceStreamMessage(Client, alliance).Send();
            }
        }

        public void Resources(Level level)
        {
            var avatar = level.GetPlayerAvatar();
            var currentGold = avatar.GetResourceCount(CSVManager.DataTables.GetResourceByName("Gold"));
            var currentElixir = avatar.GetResourceCount(CSVManager.DataTables.GetResourceByName("Elixir"));
            var goldLocation = CSVManager.DataTables.GetResourceByName("Gold");
            var elixirLocation = CSVManager.DataTables.GetResourceByName("Elixir");

            if (currentGold >= 1000000000 | currentElixir >= 1000000000)
            {
                avatar.SetResourceCount(goldLocation, currentGold + 10);
                avatar.SetResourceCount(elixirLocation, currentElixir + 10);
            }
            else if (currentGold <= 999999999 || currentElixir <= 999999999)
            {
                avatar.SetResourceCount(goldLocation, currentGold + 1000);
                avatar.SetResourceCount(elixirLocation, currentElixir + 1000);
            }
        }
    }
}
