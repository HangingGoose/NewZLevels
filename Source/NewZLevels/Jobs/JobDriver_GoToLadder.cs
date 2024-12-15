using System.Collections.Generic;
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
                    if (TargetA.Thing is BuildingLadderDown buildingLadderDown)
                    {
                        Log.Message($"Pawn {pawn.Label} is using the ladder.");
                        buildingLadderDown.SendPawnDown(pawn);
                    }
                    else
                    {
                        Log.Warning($"TargetA is not a BuildingLadderDown for pawn {pawn.Label}");
                    }
                }),
                defaultCompleteMode = ToilCompleteMode.Instant
            };
            yield return useLadder;
        }
    }
}