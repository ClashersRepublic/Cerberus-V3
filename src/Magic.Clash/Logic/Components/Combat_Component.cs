using Magic.ClashOfClans.Files;
using Magic.ClashOfClans.Files.CSV_Logic;
using Magic.ClashOfClans.Logic.Enums;
using Magic.ClashOfClans.Logic.Structure;
using Newtonsoft.Json.Linq;

namespace Magic.ClashOfClans.Logic.Components
{
    internal class Combat_Component : Component
    {
        internal Construction_Item Item;

        public Combat_Component(Construction_Item ci) : base(ci)
        {
            Item = ci;

            if (ci.ClassId == 4 || ci.ClassId == 11)
            {
                var td = (Traps) ci.Data;
                if (td.HasAltMode)
                    AltTrapAttackMode = true;

                if (td.DirectionCount > 0)
                    AltDirectionMode = true;
            }
            else if (ci.ClassId == 0 || ci.ClassId == 7)
            {
                var bd = (Buildings) ci.Data;

                if (bd.AmmoCount > 0)
                    Ammo = bd.AmmoCount;

                if (bd.AltAttackMode)
                    AltAttackMode = true;

                if (bd.AimRotateStep > 0)
                    AimRotateStep = true;
            }
        }

        public override int Type => 1;

        internal int Ammo = -1;
        internal int GearUp = -1;
        internal int WallX = -1;
        internal int WallP = -1;
        internal int WallI = -1;
        internal int AimAngle = 315;
        internal int AimAngleDraft = 315;
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
            var ca = Parent.Level.Avatar;
            var bd = (Buildings) Parent.Data;
            var rd = CSV.Tables.Get(Gamefile.Resources).GetData(bd.AmmoResource);

            if (ca.HasEnoughResources(rd.GetGlobalId(), bd.AmmoCost[0]))
            {
                ca.Resources.Minus(rd.GetGlobalId(), bd.AmmoCost[0]);
                Ammo = bd.AmmoCount;
            }
        }

        public override void Load(JObject jsonObject)
        {
            if (jsonObject["gear"] != null)
                GearUp = jsonObject["gear"].ToObject<int>();


            if (jsonObject["ammo"] != null)
                Ammo = jsonObject["ammo"].ToObject<int>();


            if (jsonObject["wX"] != null)
                WallX = jsonObject["wX"].ToObject<int>();

            if (jsonObject["wP"] != null)
                WallP = jsonObject["wP"].ToObject<int>();

            if (jsonObject["wI"] != null)
            {
                WallI = jsonObject["wI"].ToObject<int>();
                if (WallI > Parent.Level.Avatar.Wall_Group_ID)
                    Parent.Level.Avatar.Wall_Group_ID = WallI;
            }


            if (jsonObject["attack_mode"] != null)
            {
                AltAttackMode = true;
                AttackMode = jsonObject["attack_mode"].ToObject<bool>();
                AttackModeDraft = jsonObject["attack_mode_draft"].ToObject<bool>();
            }

            if (jsonObject["air_mode"] != null)
            {
                AltTrapAttackMode = true;
                AirMode = jsonObject["air_mode"].ToObject<bool>();
                AirModeDraft = jsonObject["air_mode_draft"].ToObject<bool>();
            }

            if (jsonObject["aim_angle"] != null)
            {
                AimRotateStep = true;
                AimAngle = jsonObject["aim_angle"].ToObject<int>();
                AimAngleDraft = jsonObject["aim_angle_draft"].ToObject<int>();
            }

            if (jsonObject["trapd"] != null)
            {
                AltDirectionMode = true;
                TrapDirection = jsonObject["trapd"].ToObject<int>();
                TrapDirectionDraft = jsonObject["trapd_draft"].ToObject<int>();
            }
        }

        public override JObject Save(JObject jsonObject)
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

            return jsonObject;
        }
    }
}