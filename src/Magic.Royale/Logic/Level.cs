using Magic.ClashOfClans;

namespace Magic.ClashOfClans.Logic
{
    internal class Level
    {
        public Level()
        {
            Avatar = new Avatar();
        }

        public Level(long id)
        {
            Avatar = new Avatar(id);
        }

        public Avatar Avatar { get; set; }
        public Device Device { get; set; }
    }
}
