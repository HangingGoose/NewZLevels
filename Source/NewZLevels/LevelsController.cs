using System;
using NewZLevels.LevelClasses;

namespace NewZLevels
{
    public class LevelsController
    {
        private Levels levels { get; }

        private static LevelsController _instance;
        
        public static LevelsController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LevelsController();
                }
                return _instance;
            }
        }
        
        private LevelsController()
        {
            levels = new Levels();
        }

        public void AddLevel(Level level)
        {
            if (level == null)
            {
                throw new ArgumentNullException(nameof(level));
            }

            levels.Add(level);
        }
        
        public Level GetLevel(int zLevel)
        {
            return levels.GetLevelFromZ(zLevel);
        }
    }
}