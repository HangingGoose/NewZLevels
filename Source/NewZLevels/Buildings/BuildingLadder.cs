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
        protected static readonly LevelsController controller = LevelsController.Instance;
        protected int zLevel { get; }

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
            
            int targetZLevel = 0;
            switch (direction)
            {
                case "down":
                    targetZLevel = zLevel - 1; break;
                case "up":
                    targetZLevel = zLevel + 1; break;
            }

            Level pocketLevel = controller.GetLevel(targetZLevel) ?? new Level(null, targetZLevel);
            controller.AddLevel(pocketLevel);

            if (zLevel == 0 && controller.GetLevel(0) == null)
            {
                controller.AddLevel(new Level(this.Map, 0));
                Log.Message("Starting map (colony foundation map) added to controller at zLevel 0.");
            }

            Messages.Message($"{pawn.Label} goes {direction} the ladder to Z level {targetZLevel}.", MessageTypeDefOf.PositiveEvent);

            this.Map.mapPawns.DeRegisterPawn(pawn);
            pawn.DeSpawn();

            IntVec3 ladderPosition = this.Position;

            if (pocketLevel.map == null)
            {
                pocketLevel.map = GenerateMaps.CreateUndergroundPocketMap(this.Map);

                PlaceLadderOnMap(direction, ladderPosition, pocketLevel.map);
            }

            if (!ladderPosition.IsValid || !pocketLevel.map.AllCells.Contains(ladderPosition))
            {
                Log.Warning("Ladder position on the pocket map is invalid.");
                return;
            }

            pawn.Position = ladderPosition;
            pawn.SpawnSetup(pocketLevel.map, true);
        }

        private void PlaceLadderOnMap(string direction, IntVec3 position, Map map)
        {
            ThingDef ladderDef = null;
            switch (direction)
            {
                case "down":
                    ladderDef = DefDatabase<ThingDef>.GetNamed("BuildingLadderUp", false); break;
                case "up":
                    ladderDef = DefDatabase<ThingDef>.GetNamed("BuildingLadderDown", false); break;
            }

            if (ladderDef == null)
            {
                Log.Error($"Failed to find ThingDef for {direction} ladder.");
                return;
            }

            bool ladderExists = map.thingGrid.ThingsListAt(position).Any(thing => thing.def == ladderDef);

            if (!ladderExists)
            {
                Building ladder = (Building)ThingMaker.MakeThing(ladderDef, this.Stuff);
                ladder.SetFaction(this.Faction);

                GenSpawn.Spawn(ladder, position, map, WipeMode.FullRefund);
                Log.Message($"Placed {direction} ladder on the pocket map.");
            }
        }

        protected void GiveJob(Pawn pawn)
        {
            Job job = JobMaker.MakeJob(NewZLevelsDefOf.GoToLadder, this);
            pawn.jobs.StartJob(job, JobCondition.InterruptForced);
        }
    }
}
