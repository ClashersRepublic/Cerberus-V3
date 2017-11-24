using CR.Servers.CoC.Extensions.Helper;
using CR.Servers.CoC.Files;
using CR.Servers.CoC.Files.CSV_Logic.Logic;
using CR.Servers.CoC.Logic.Enums;
using Newtonsoft.Json.Linq;

namespace CR.Servers.CoC.Logic
{
    internal class CombatComponent : Component
    {
        internal override int Type => 1;

        public CombatComponent(GameObject GameObject) : base(GameObject)
        {
            if (GameObject is Building Building)
            {
                var bd = Building.BuildingData;

                if (bd.AmmoCount > 0)
                    Ammo = bd.AmmoCount;

                if (bd.AltAttackMode)
                    AltAttackMode = true;

                if (bd.AimRotateStep > 0)
                    AimRotateStep = true;
            }
            else if (GameObject is Trap Trap)
            {
                var td = Trap.TrapData;
                if (td.HasAltMode)
                    AltTrapAttackMode = true;

                if (td.DirectionCount > 0)
                    AltDirectionMode = true;
            }

        }

        internal int Ammo = -1;
        internal int GearUp = -1;
        internal int WallX = -1;
        internal int WallP = -1;
        internal int WallI = -1;
        internal int AimAngle = 360;
        internal int AimAngleDraft = 360;
        internal int TrapDirection;
        internal int TrapDirectionDraft;
        internal bool AltAttackMode;
        internal bool AimRotateStep;
        internal bool AttackMode;
        internal bool AttackModeDraft;
        internal bool AltDirectionMode;
        internal bool AirMode;
        internal bool AirModeDraft;
        internal bool AltTrapAttackMode;
        internal bool NeedsRepair = true;

        internal void FillAmmo()
        {
            var ca = Parent.Level.Player;
            var bd = (BuildingData)Parent.Data;
            var rd = CSV.Tables.Get(Gamefile.Resources).GetData(bd.AmmoResource);
            if (ca.Resources.GetCountByData(rd) >= bd.AmmoCost[0])
            {
                ca.Resources.Minus(rd.GlobalId, bd.AmmoCost[0]);
                Ammo = bd.AmmoCount;
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

                if (WallI > Parent.Level.Player.WallGroupId)
                    Parent.Level.Player.WallGroupId = WallI;
            }


            if (JsonHelper.GetJsonBoolean(Json, "attack_mode", out bool AttackMode) && JsonHelper.GetJsonBoolean(Json, "attack_mode_draft", out bool AttackModeDraft))
            {
                AltAttackMode = true;
                this.AttackMode = AttackMode;
                this.AttackModeDraft = AttackModeDraft;
            }

            if (JsonHelper.GetJsonBoolean(Json, "air_mode", out bool AirMode) && JsonHelper.GetJsonBoolean(Json, "air_mode", out bool AirModeDraft))
            {
                AltTrapAttackMode = true;
                this.AirMode = AirMode;
                this.AirModeDraft = AirModeDraft;
            }


            if (JsonHelper.GetJsonNumber(Json, "aim_angle", out int AimAngle) && JsonHelper.GetJsonNumber(Json, "aim_angle_draft", out int AimAngleDraft))
            {
                AimRotateStep = true;
                this.AimAngle = AimAngle;
                this.AimAngleDraft = AimAngleDraft;
            }


            if (JsonHelper.GetJsonNumber(Json, "trapd", out int TrapDirection) && JsonHelper.GetJsonNumber(Json, "trapd_draft", out int TrapDirectionDraft))
            {
                AltDirectionMode = true;
                this.TrapDirection = TrapDirection;
                this.TrapDirectionDraft = TrapDirectionDraft;
            }

            base.Load(Json);
        }

        internal override void Save(JObject jsonObject)
        {
            if (GearUp >= 0)
                jsonObject.Add("gear", GearUp);

            if (Ammo >= 0)
                jsonObject.Add("ammo", Ammo);

            if (WallX >= 0)
                jsonObject.Add("wX", WallX);

            if (WallP >= 0)
                jsonObject.Add("wP", WallP);

            if (WallI >= 0)
                jsonObject.Add("wI", WallI);

            if (AltAttackMode)
            {
                jsonObject.Add("attack_mode", AttackMode);
                jsonObject.Add("attack_mode_draft", AttackModeDraft);
            }

            if (AltTrapAttackMode)
            {
                jsonObject.Add("air_mode", AirMode);
                jsonObject.Add("air_mode_draft", AirModeDraft);
            }

            if (AimRotateStep)
            {
                jsonObject.Add("aim_angle", AimAngle);
                jsonObject.Add("aim_angle_draft", AimAngle);
            }

            if (AltDirectionMode)
            {
                jsonObject.Add("trapd", TrapDirection);
                jsonObject.Add("trapd_draft", TrapDirectionDraft);
            }

            base.Save(jsonObject);
        }

    }
}
