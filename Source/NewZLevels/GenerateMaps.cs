using System.Linq;
using Verse;

namespace NewZLevels
{
    public static class GenerateMaps
    {
        public static Map CreateUndergroundPocketMap(Map sourceMap, int pocketZLevel)
        {
            int width = sourceMap.Size.x;
            int height = sourceMap.Size.z;

            Log.Message("Creating pocket map...");
            MapGeneratorDef undergroundDef = DefDatabase<MapGeneratorDef>.GetNamed("NZL_Underground");
            if (undergroundDef == null)
            {
                Log.Error("Failed to find MapGeneratorDef: NZL_Underground");
                return null;
            }

            Map pocketMap = MapGenerator.GenerateMap(
                new IntVec3(width, 0, height),
                null, undergroundDef,
                null,
                null,
                true
            );

            if (pocketMap == null || !pocketMap.AllCells.Any())
            {
                Log.Error("Pocket map generation failed or resulted in an empty map.");
            }
            else
            {
                Log.Message($"Pocket map generated with size: {pocketMap.Size}");
            }

            return pocketMap;
        }
    }
}