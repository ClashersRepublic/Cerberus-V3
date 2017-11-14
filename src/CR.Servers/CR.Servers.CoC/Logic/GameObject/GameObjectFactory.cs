using CR.Servers.CoC.Files.CSV_Helpers;

namespace CR.Servers.CoC.Logic
{
    internal static class GameObjectFactory
    {
        internal static GameObject CreateGameObject(Data Data, Level Level)
        {
            switch (Data.GetDataType())
            {
                case 1:
                    return new Building(Data, Level);
                /*case 4:
                    return new Trap(Data, Level);
                case 6:
                    return new Deco(Data, Level);*/
                case 8:
                    return new Obstacle(Data, Level);

                default:
                    return null;
            }
        }
    }
}