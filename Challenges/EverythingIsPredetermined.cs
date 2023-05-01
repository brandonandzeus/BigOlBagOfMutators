using System;
using System.Collections.Generic;
using System.Text;
using Trainworks.BuildersV2;
using Trainworks.ConstantsV2;
using Trainworks.ManagersV2;

namespace BigOlBagOfMutators.Challenges
{
    public static class EverythingIsPredetermined
    {
        public static void MakeChallenge()
        {
            SpChallengeDataBuilder builder = new SpChallengeDataBuilder
            {
                ChallengeID = "Predetermined",
                Name = "Everything is Predetermined",
                Description = "Your destiny is predetermined! Pathing, Champions, and Card Upgrades already determined",
                Mutators =
                {
                    CustomMutatorManager.GetMutatorDataByID("Mutator_ScriptedPage"),
                    CustomMutatorManager.GetMutatorDataByID(VanillaMutatorIDs.OneTrackMind),
                    CustomMutatorManager.GetMutatorDataByID("ErraticReflection"),
                }
            };

            builder.BuildAndRegister();
        }
    }
}
