using Verse;

namespace NewZLevels.LevelClasses
{
    public class Level
    {
        public Map map { get; set; }
        public int zLevel { get; set; }

        public Level(Map map, int zLevel)
        {
            this.map = map;
            this.zLevel = zLevel;
        }
    }
}