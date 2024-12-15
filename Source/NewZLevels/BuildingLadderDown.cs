using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace NewZLevels
{
    public class BuildingLadderDown : Building
    {
        private int ZLevel = 0;

        public BuildingLadderDown(int zLevel)
        {
            ZLevel = zLevel;
        }

        public BuildingLadderDown()
        {
            ZLevel = 0;
        }

        [StaticConstructorOnStartup]
        public static class ModEntry
        {
            static ModEntry()
            {
                Log.Message("NewZLevels mod is loaded!");
            }
        }


        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
        {
            Log.Message("BuildingLadderDown GetFloatMenuOptions called");

            foreach (var item in base.GetFloatMenuOptions(selPawn))
            {
                yield return item;
            }

            yield return new FloatMenuOption(
                $"Send {selPawn.Label} down",
                delegate { GiveJob(selPawn); }, MenuOptionPriority.Default);
        }

        private void GiveJob(Pawn pawn)
        {
            Job job = JobMaker.MakeJob(NewZLevelsDefOf.GoToLadder, this);
            pawn.jobs.StartJob(job, JobCondition.InterruptForced);
        }

        public void SendPawnDown(Pawn pawn)
        {
            if (pawn == null)
            {
                Log.Warning("SendPawnDown was called, but the pawn is null.");
                return;
            }

            Log.Message(String.Format("Current map cells: {0}", this.Map.AllCells.Count()));
            Map pocketMap = GenerateMaps.CreateUndergroundPocketMap(this.Map, ZLevel - 1);
            if (pocketMap == null)
            {
                Log.Warning("Failed to generate pocket map.");
                return;
            }

            Messages.Message($"{pawn.Label} goes down the ladder.", MessageTypeDefOf.PositiveEvent);

            this.Map.mapPawns.DeRegisterPawn(pawn);
            pawn.DeSpawn();

            IntVec3 ladderPosition = new IntVec3(this.Position.x, this.Position.y, this.Position.z);

            if (!ladderPosition.IsValid || !pocketMap.AllCells.Contains(ladderPosition))
            {
                Log.Warning("Ladder position on the pocket map is invalid.");
                return;
            }

            bool ladderUpExists = pocketMap.thingGrid.ThingsListAt(ladderPosition)
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
                GenSpawn.Spawn(ladderUp, ladderPosition, pocketMap, WipeMode.FullRefund);
                Log.Message("Replaced impassable or null tile with a ladderUp.");
            }


            pawn.Position = ladderPosition;
            pawn.SpawnSetup(pocketMap, true);
        }
    }
}