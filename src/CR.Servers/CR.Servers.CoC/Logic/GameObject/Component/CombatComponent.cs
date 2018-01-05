namespace CR.Servers.CoC.Logic
{
    using CR.Servers.CoC.Extensions.Helper;
    using CR.Servers.CoC.Files;
    using CR.Servers.CoC.Files.CSV_Helpers;
    using CR.Servers.CoC.Files.CSV_Logic.Logic;
    using CR.Servers.CoC.Logic.Enums;
    using Newtonsoft.Json.Linq;

    internal class CombatComponent : Component
    {
        internal int AimAngle = 360;
        internal int AimAngleDraft = 360;
        internal bool AimRotateStep;
        internal bool AirMode;
        internal bool AirModeDraft;
        internal bool AltAttackMode;
        internal bool AltDirectionMode;
        internal bool AltTrapAttackMode;

        internal int Ammo = -1;
        internal bool AttackMode;
        internal bool AttackModeDraft;
        internal int GearUp = -1;
        internal bool NeedsRepair = true;
        internal int TrapDirection;
        internal int TrapDirectionDraft;
        internal int WallI = -1;
        internal int WallP = -1;
        internal int WallX = -1;

        public CombatComponent(GameObject GameObject) : base(GameObject)
        {
            if (GameObject is Building Building)
            {
                BuildingData bd = Building.BuildingData;

                if (bd.AmmoCount > 0)
                {
                    this.Ammo = bd.AmmoCount;
                }

                if (bd.AltAttackMode)
                {
                    this.AltAttackMode = true;
                }

                if (bd.AimRotateStep > 0)
                {
                    this.AimRotateStep = true;
                }
            }
            else if (GameObject is Trap Trap)
            {
                TrapData td = Trap.TrapData;
                if (td.HasAltMode)
                {
                    this.AltTrapAttackMode = true;
                }

                if (td.DirectionCount > 0)
                {
                    this.AltDirectionMode = true;
                }
            }
        }

        internal override int Type => 1;

        internal void FillAmmo()
        {
            Player ca = this.Parent.Level.Player;
            BuildingData bd = (BuildingData) this.Parent.Data;
            Data rd = CSV.Tables.Get(Gamefile.Resources).GetData(bd.AmmoResource);
            if (ca.Resources.GetCountByData(rd) >= bd.AmmoCost[0])
            {
                ca.Resources.Minus(rd.GlobalId, bd.AmmoCost[0]);
                this.Ammo = bd.AmmoCount;
            }
        }

        internal override void Load(JToken Json)
        {
            if (JsonHelper.GetJsonNumber(Json, "gear", out int GearUp))
            {
                this.GearUp = GearUp;
            }

            if (JsonHelper.GetJsonNumber(Json, "ammo", out int Ammo))
            {
                this.Ammo = Ammo;
            }

            if (JsonHelper.GetJsonNumber(Json, "wX", out int WallX))
            {
                this.WallX = WallX;
            }

            if (JsonHelper.GetJsonNumber(Json, "wP", out int WallP))
            {
                this.WallP = WallP;
            }

            if (JsonHelper.GetJsonNumber(Json, "wI", out int WallI))
            {
                this.WallI = WallI;

                if (WallI > this.Parent.Level.Player.WallGroupId)
                {
                    this.Parent.Level.Player.WallGroupId = WallI;
                }
            }


            if (JsonHelper.GetJsonBoolean(Json, "attack_mode", out bool AttackMode) && JsonHelper.GetJsonBoolean(Json, "attack_mode_draft", out bool AttackModeDraft))
            {
                this.AltAttackMode = true;
                this.AttackMode = AttackMode;
                this.AttackModeDraft = AttackModeDraft;
            }

            if (JsonHelper.GetJsonBoolean(Json, "air_mode", out bool AirMode) && JsonHelper.GetJsonBoolean(Json, "air_mode", out bool AirModeDraft))
            {
                this.AltTrapAttackMode = true;
                this.AirMode = AirMode;
                this.AirModeDraft = AirModeDraft;
            }


            if (JsonHelper.GetJsonNumber(Json, "aim_angle", out int AimAngle) && JsonHelper.GetJsonNumber(Json, "aim_angle_draft", out int AimAngleDraft))
            {
                this.AimRotateStep = true;
                this.AimAngle = AimAngle;
                this.AimAngleDraft = AimAngleDraft;
            }


            if (JsonHelper.GetJsonNumber(Json, "trapd", out int TrapDirection) && JsonHelper.GetJsonNumber(Json, "trapd_draft", out int TrapDirectionDraft))
            {
                this.AltDirectionMode = true;
                this.TrapDirection = TrapDirection;
                this.TrapDirectionDraft = TrapDirectionDraft;
            }

            base.Load(Json);
        }

        internal override void Save(JObject jsonObject)
        {
            if (this.GearUp >= 0)
            {
                jsonObject.Add("gear", this.GearUp);
            }

            if (this.Ammo >= 0)
            {
                jsonObject.Add("ammo", this.Ammo);
            }

            if (this.WallX >= 0)
            {
                jsonObject.Add("wX", this.WallX);
            }

            if (this.WallP >= 0)
            {
                jsonObject.Add("wP", this.WallP);
            }

            if (this.WallI >= 0)
            {
                jsonObject.Add("wI", this.WallI);
            }

            if (this.AltAttackMode)
            {
                jsonObject.Add("attack_mode", this.AttackMode);
                jsonObject.Add("attack_mode_draft", this.AttackModeDraft);
            }

            if (this.AltTrapAttackMode)
            {
                jsonObject.Add("air_mode", this.AirMode);
                jsonObject.Add("air_mode_draft", this.AirModeDraft);
            }

            if (this.AimRotateStep)
            {
                jsonObject.Add("aim_angle", this.AimAngle);
                jsonObject.Add("aim_angle_draft", this.AimAngle);
            }

            if (this.AltDirectionMode)
            {
                jsonObject.Add("trapd", this.TrapDirection);
                jsonObject.Add("trapd_draft", this.TrapDirectionDraft);
            }

            base.Save(jsonObject);
        }
    }
}