using Magic.Logic;
using System.Data.Entity;

namespace Magic.Db
{
    public class LevelDb : DbContext
    {
        public LevelDb() : base()
        {
            // Space
        }

        public DbSet<Level> Levels { get; set; }
    }
}
