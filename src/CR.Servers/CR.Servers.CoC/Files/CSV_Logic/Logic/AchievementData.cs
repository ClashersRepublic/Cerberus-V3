using CR.Servers.CoC.Files.CSV_Helpers;
using CR.Servers.Files.CSV_Reader;
using CR.Servers.CoC.Logic.Enums

namespace CR.Servers.CoC.Files.CSV_Logic.Logic
{
  internal class AchievementData : Data
  {
    internal readonly int ActionType;
    internal BuildingData bd;
    internal CharacterData cd;
    internal ResourceData rd;

    public AchievementData(Row Row, DataTable DataTable) : base(Row, DataTable)
    {
    }

    public override string Name { get; set; }
    public int Level { get; set; }
    public int LevelCount { get; set; }
    public string TID { get; set; }
    public string InfoTID { get; set; }
    public string Action { get; set; }
    public int ActionCount { get; set; }
    public string ActionData { get; set; }
    public int ExpReward { get; set; }
    public int DiamondReward { get; set; }
    public string IconSWF { get; set; }
    public string IconExportName { get; set; }
    public string CompletedTID { get; set; }
    public bool ShowValue { get; set; }
    public string AndroidID { get; set; }

    internal override void Process()
    {
      switch(this.Action)
      {
        case "npc_stars":
          this.ActionType = AchievementTypes.NPC_STARS;
          break;

        case "upgrade":
          this.ActionType = AchievementTypes.UPGRADE;
          this.bd = (BuildingData) CSV.Tables.Get(Gamefile.Buildings).GetData(this.ActionData);
          break;

        case "victory_points":
          this.ActionType = AchievementTypes.VICTORY_POINTS;
          break;

        case "unit_unlock":
          this.ActionType = AchievementTypes.UNIT_UNLOCK;
          this.cd = (CharacterData) CSV.Tables.Get(Gamefile.Characters).GetData(this.ActionData);
          break;

        case "clear_obstacles":
          this.ActionType = AchievementTypes.CLEAR_OBSTACLES;
          break;

        case "donate_units":
          this.ActionType = AchievementTypes.DONATE_UNITS;
          break;

        case "loot":
          this.ActionType = AchievementTypes.LOOT;
          this.rd = (ResourceData) CSV.Tables.Get(Gamefile.Resources).GetData(this.ActionData);
          break;

        case "destroy":
          this.ActionType = AchievementTypes.DESTROY;
          this.bd = (BuildingData) CSV.Tables.Get(Gamefile.Buildings).GetData(this.ActionData);
          break;

        case "win_pvp_defense":
          this.ActionType = AchievementTypes.WIN_PVP_DEFENSE;
          break;

        case "win_pvp_attack":
          this.ActionType = AchievementTypes.WIN_PVP_ATTACK;
          break;

        case "league":
          this.ActionType = AchievementTypes.LEAGUE;
          break;

        case "war_stars":
          this.ActionType = AchievementTypes.WAR_STARS;
          break;
          
        case "war_loot":
          this.ActionType = AchievementTypes.WAR_LOOT;
          break;

        case "donate_spells":
          this.ActionType = AchievementTypes.DONATE_SPELLS;
          break;

        case "account_bound":
          this.ActionType = AchievementTypes.ACCOUNT_BOUND;
          break;

        case "vs_battle_trophies":
          this.ActionType = AchievementTypes.VERSUS_BATTLE_TROPHIES;
          break;

        case "gear_up":
          this.ActionType = AchievementTypes.GEAR_UP;
          break;

        case "repair_building":
          this.ActionType = AchievementTypes.REPAIR_BUILDING;
          this.bd = (BuildingData) CSV.Tables.Get(Gamefile.Buildings).GetData(this.ActionData);
          break;

        default :
          {
            throw new Exception("Unknown Action in achievements " + this.ActionData);
          }
      }
    }
  }
}
