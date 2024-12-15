using System;
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
            int depth = sourceMap.Size.y;

            Log.Message("Creating pocket map...");
            MapGeneratorDef undergroundDef = DefDatabase<MapGeneratorDef>.GetNamed("NZL_Underground");
            if (undergroundDef == null)
            {
                Log.Error("Failed to find MapGeneratorDef: NZL_Underground");
                return null;
            }

            Log.Message(String.Format("Y value of cells:") + sourceMap.AllCells.First().y);
            Log.Message(String.Format("Width: {0}", width));
            Log.Message(String.Format("Height: {0}", height));

            Map pocketMap = PocketMapUtility.GeneratePocketMap(
                new IntVec3(width, depth, height),
                undergroundDef,
                null,
                sourceMap
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