using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.Servers.CoC.Core;
using CR.Servers.CoC.Extensions.Game;
using CR.Servers.CoC.Extensions.Helper;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic;
using CR.Servers.CoC.Logic.Manager;
using CR.Servers.Extensions.Binary;

namespace CR.Servers.CoC.Packets.Commands.Client
{
    internal class Buy_Resources : Command
    {
        internal override int Type => 518;

        public Buy_Resources(Device device, Reader reader) : base(device, reader)
        {
        }


        internal int Count;

        internal Command Command;
        internal ResourceData Data;
        internal bool EmbedCommand;

        internal override void Decode()
        {
            this.Count = this.Reader.ReadInt32();
            this.Data = this.Reader.ReadData<ResourceData>();

            this.Reader.ReadInt32();

            this.EmbedCommand = this.Reader.ReadBoolean();

            if (this.EmbedCommand)
            {
                var CommandID = this.Reader.ReadInt32();

                if (Factory.Commands.ContainsKey(CommandID))
                {
                    Command = Factory.CreateCommand(CommandID, this.Device, this.Reader);

                    if (Command != null)
                    {
                        Command.Decode();
                    }
                }
                else
                {
                    Logging.Error(this.GetType(), "Embded Command is not handled! (" + CommandID + ")");
                    this.Reader.BaseStream.Position = this.Reader.BaseStream.Position - (4 + 1 + 4 + 4 + 4);
                    this.Log();
                }
            }

            base.Decode();
        }

        internal override void Execute()
        {
            ShowValues();

            if (this.Data != null)
            {
                if (!this.Data.PremiumCurrency)
                {
                    // if (string.IsNullOrEmpty(this.Data.WarRefResource))
                    {
                        var Level = this.Device.GameMode.Level;
                        int Cost = GamePlayUtil.GetResourceCost(this.Data, this.Count, this.Data.VillageType);

                        if (Level.Player.HasEnoughDiamonds(Cost))
                        {
                            if (Level.Player.GetAvailableResourceStorage(this.Data) >= this.Count)
                            {
                                Level.Player.UseDiamonds(Cost);
                                Level.Player.Resources.Add(this.Data, this.Count);

                                if (this.Command != null)
                                {
                                    try
                                    {
                                        if (!this.Command.IsServerCommand)
                                        {
                                            this.Command.Execute();
                                            Logging.Info(this.GetType(), "Embedded Command is handled! (" + Command.Type + ")");
                                        }
                                        else
                                        {
                                            Logging.Error(this.GetType(), "Unable to execute server command as embedded command! (" +  Command.Type + ")");
                                        }
                                    }
                                    catch (Exception Exception)
                                    {
                                        Logging.Error(this.GetType(), "An error has been throwed when the process of command type " + this.Command.Type + ". " + Exception);
                                    }
                                }
                            }
                        }
                        else
                            Logging.Error(this.GetType(), "Unable to buy resources. The player don't have enough diamond.");
                    }
                }
                else
                    Logging.Error(this.GetType(), "Unable to buy resources. Premium resources is not buyable.");
            }
            else
                Logging.Error(this.GetType(), "Unable to buy resources. Data is null or invalid.");
        }
    }
}
