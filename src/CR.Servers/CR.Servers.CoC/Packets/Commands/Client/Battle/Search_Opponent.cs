﻿namespace CR.Servers.CoC.Packets.Commands.Client.Battle
{
    using System.Collections.Generic;
    using CR.Servers.CoC.Core;
    using CR.Servers.CoC.Core.Network;
    using CR.Servers.CoC.Logic;
    using CR.Servers.CoC.Logic.Battles;
    using CR.Servers.CoC.Packets.Messages.Server.Battle;
    using CR.Servers.Extensions.Binary;
    using CR.Servers.Extensions.List;
    using CR.Servers.Logic.Enums;

    internal class Search_Opponent : Command
    {
        public Search_Opponent(Device Device, Reader Reader) : base(Device, Reader)
        {
        }

        internal override int Type
        {
            get
            {
                return 800;
            }
        }

        internal override void Decode()
        {
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
            base.Decode();
        }

        internal override void Encode(List<byte> Data)
        {
            Data.AddInt(0);
            Data.AddInt(0);
            base.Encode(Data);
        }

        internal override void Execute()
        {
            if (this.Device.GameMode.Level.Player.ModSlot.AIAttack)
            {
                new EnemyHomeDataMessage(this.Device)
                {
                    Enemy = this.Device.GameMode.Level.Player.ModSlot.AILevel
                }.Send();
            }
            else if (this.Device.GameMode.Level.Player.ModSlot.SelfAttack)
            {
                new EnemyHomeDataMessage(this.Device)
                {
                    Enemy = this.Device.GameMode.Level
                }.Send();

                this.Device.GameMode.Level.Player.ModSlot.SelfAttack = false;
            }
            else
            {
                if (this.Device.State == State.IN_PC_BATTLE)
                {
                    if (this.Device.Account.Battle != null)
                    {
                        this.Device.Account.Battle.EndBattle();
                        this.Device.Account.Battle = null;
                    }
                    else
                    {
                        Logging.Error(this.GetType(), "Yo boss, Check this out...Battle is null but player state is IN_PC_BATTLE");
                    }

                    this.Device.State = State.LOGGED;
                }

                Account rndAccount = Resources.Accounts.LoadRandomOfflineAccount();

                if (rndAccount != null)
                {
                    Battle battle = new Battle(this.Device, this.Device.GameMode.Level, rndAccount.Home.Level, false);

                    this.Device.Account.Battle = battle;
                    rndAccount.Battle = battle;

                    new EnemyHomeDataMessage(this.Device, battle.Defender)
                    {
                        NextButton = true
                    }.Send();
                }
                else
                {
                    new AttackHomeFailedMessage(this.Device).Send();
                }
            }
        }
    }
}