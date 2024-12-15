using System;
using System.Linq;
using RimWorld;
using Verse;

namespace NewZLevels.GenSteps
{
    public class GenStep_Underground : GenStep
    {
        public override int SeedPart => 593456892;

        public override void Generate(Map map, GenStepParams parms)
        {
            Log.Message("Generating underground map...");
            for (int x = 0; x < map.Size.x; x++)
            {
                for (int z = 0; z < map.Size.z; z++)
                {
                    IntVec3 cell = new IntVec3(x, 0, z);
                    if (cell.InBounds(map))
                    {
                        if (cell.InBounds(map) && map.terrainGrid.TerrainAt(cell) == null)
                        {
                            map.terrainGrid.SetTerrain(cell, TerrainDefOf.Gravel);
                        }
                        if (map.terrainGrid.TerrainAt(cell) != null)
                        {
                            PlaceRockAtCell(cell, map);
                        }
                        else
                        {
                            Log.Warning($"Cell {cell} does not have valid terrain.");
                        }
                    }
                }
            }

            Log.Message("Amount of cells: " + map.AllCells.Count());
        }

        private void PlaceRockAtCell(IntVec3 cell, Map map)
        {
            ThingDef rockType = DefDatabase<ThingDef>.GetNamed("Granite");

            Thing rock = ThingMaker.MakeThing(rockType);

            GenSpawn.Spawn(rock, cell, map);
        }
    }
}