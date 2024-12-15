using System;
using System.Collections.Generic;

namespace NewZLevels.LevelClasses
{
    public class Levels
    {
        public SortedSet<Level> levels { get; }

        public Levels()
        {
            levels = new SortedSet<Level>(new LevelComparer());
        }

        public bool Add(Level level)
        {
            return levels.Add(level);
        }

        public bool Remove(int zLevel)
        {
            foreach (Level level in levels)
            {
                if (level.zLevel == zLevel)
                {
                    return levels.Remove(level);
                }
            }

            return false;
        }

        public Level GetLevelFromZ(int zLevel)
        {
            foreach (Level level in levels)
            {
                if (level.zLevel == zLevel)
                {
                    return level;
                }
            }

            return null;
        }
    }

    internal class LevelComparer : IComparer<Level>
    {
        public int Compare(Level lvl, Level other)
        {
            if (lvl == null || other == null)
                throw new ArgumentNullException();

            return lvl.zLevel.CompareTo(other.zLevel);
        }
    }
}