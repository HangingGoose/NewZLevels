using System.Collections.Generic;
using Verse;

namespace NewZLevels.Buildings
{
    public class BuildingLadderDown : BuildingLadder
    {
        public BuildingLadderDown(int zLevel) : base(zLevel)
        {
        }

        public BuildingLadderDown(): base()
        {
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
    }
}