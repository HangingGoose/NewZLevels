using System;
using System.Linq;
using NewZLevels.Defs;
using NewZLevels.LevelClasses;
using NewZLevels.mapUtil;
using RimWorld;
using Verse;
using Verse.AI;

namespace NewZLevels.Buildings
{
    public abstract class BuildingLadder : Building
    {
        protected int zLevel { get; }

        protected BuildingLadder(int zLevel)
        {
            this.zLevel = zLevel;
        }

        protected BuildingLadder()
        {
            this.zLevel = 0;
        }

        public void SendPawn(Pawn pawn, string direction)
        {
            if (pawn == null)
            {
                Log.Warning("SendPawn was called, but the pawn is null.");
                return;
            }

            Level pocketLevel = new Level(null, zLevel);
            
            Messages.Message($"{pawn.Label} goes " + direction + " the ladder.", MessageTypeDefOf.PositiveEvent);
            
            this.Map.mapPawns.DeRegisterPawn(pawn);
            pawn.DeSpawn();
            
            IntVec3 ladderPosition = new IntVec3(this.Position.x, this.Position.y, this.Position.z);
            if (!ladderPosition.IsValid || !pocketLevel.map.AllCells.Contains(ladderPosition))
            {
                Log.Warning("Ladder position on the pocket map is invalid.");
                return;
            }
            
            
            
            if (direction.Equals("down"))
            {
                pocketLevel.zLevel = zLevel - 1;
                pocketLevel.map = GenerateMaps.CreateUndergroundPocketMap(this.Map);
                
                bool ladderUpExists = pocketLevel.map.thingGrid.ThingsListAt(ladderPosition)
                    .Any(thing => thing.def.defName == "BuildingLadderUp");
                if (!ladderUpExists)
                {
                    ThingDef ladderUpDef = DefDatabase<ThingDef>.GetNamed("BuildingLadderUp", false);
                    if (ladderUpDef == null)
                    {
                        Log.Error("Failed to find ThingDef for BuildingLadderUp.");
                        return;
                    }

                    ThingDef defaultStuff = this.Stuff;
                    Building ladderUp = (Building)ThingMaker.MakeThing(ladderUpDef, defaultStuff);
                    GenSpawn.Spawn(ladderUp, ladderPosition, pocketLevel.map, WipeMode.FullRefund);
                    Log.Message("Replaced impassable or null tile with a ladderUp.");
                }
            }
            else if (direction.Equals("up"))
            {
                pocketLevel.zLevel = zLevel + 1;
                pocketLevel.map = GenerateMaps.CreateUndergroundPocketMap(this.Map);
                
                bool ladderUpExists = pocketLevel.map.thingGrid.ThingsListAt(ladderPosition)
                    .Any(thing => thing.def.defName == "BuildingLadderUp");
                if (!ladderUpExists)
                {
                    ThingDef ladderUpDef = DefDatabase<ThingDef>.GetNamed("BuildingLadderUp", false);
                    if (ladderUpDef == null)
                    {
                        Log.Error("Failed to find ThingDef for BuildingLadderUp.");
                        return;
                    }

                    ThingDef defaultStuff = this.Stuff;
                    Building ladderUp = (Building)ThingMaker.MakeThing(ladderUpDef, defaultStuff);
                    GenSpawn.Spawn(ladderUp, ladderPosition, pocketLevel.map, WipeMode.FullRefund);
                    Log.Message("Replaced impassable or null tile with a ladderUp.");
                }
            }
            
            pawn.Position = ladderPosition;
            pawn.SpawnSetup(pocketLevel.map, true);
        }
        
        protected void GiveJob(Pawn pawn)
        {
            Job job = JobMaker.MakeJob(NewZLevelsDefOf.GoToLadder, this);
            pawn.jobs.StartJob(job, JobCondition.InterruptForced);
        }
    }
}