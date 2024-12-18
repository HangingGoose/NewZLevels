using System.Collections;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace NewZLevels.Buildings
{
    public class Building_LadderUp : Building_Ladder
    {

        public Building_LadderUp(): base()
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