using System.Collections.Generic;
using NewZLevels.Buildings;
using Verse;
using Verse.AI;

namespace NewZLevels.Jobs
{
    public class JobDriver_GoToLadder : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            Log.Message($"Making pre-toil reservations for {pawn.Label}");
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            Log.Message($"Executing JobDriver_GoToLadder for {pawn.Label}");

            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);

            Toil goToLadder = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
            yield return goToLadder;

            Toil useLadder = new Toil
            {
                initAction = (() =>
                {
                    Log.Message($"Pawn {pawn.Label} is using the ladder.");
                    if (TargetA.Thing is Building_LadderDown buildingLadderDown)
                    {
                        buildingLadderDown.SendPawn(pawn, "down");
                    }
                    else if (TargetA.Thing is Building_LadderUp buildingLadderUp)
                    {
                        buildingLadderUp.SendPawn(pawn, "up");
                    }
                }),
                defaultCompleteMode = ToilCompleteMode.Instant
            };
            yield return useLadder;
        }
    }
}