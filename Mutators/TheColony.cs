using BepInEx.Logging;
using BigOlBagOfMutators.RelicEffects;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using Trainworks.BuildersV2;
using Trainworks.ConstantsV2;
using Trainworks.Managers;

namespace BigOlBagOfMutators.Mutators
{
    public class TheColony
    {
        public static void Make()
        {
            var allGameData = ProviderManager.SaveManager.GetAllGameData();

            var spikedriverColony = allGameData.FindCardData(VanillaCardIDs.SpikedriverColony);

            var cardPool = CustomCardPoolManager.GetVanillaCardPool(VanillaCardPoolIDs.SpikedriverColony);
            var weakerUpgrade = new CardUpgradeDataBuilder
            {
                UpgradeID = "TheColonyWeakerUpgrade",
                BonusDamage = 5,
                BonusHP = 5,
            };

            var mutator = new MutatorDataBuilder
            {
                MutatorID = "TheColony",
                Name = "The Colony",
                Description = "Replaces Train Stewards with Spikedriver Colony with +5[attack] +5[health].",
                BoonValue = 4,
                Tags = { "subtype" }, // At your service uses this.
                EffectBuilders =
                {
                    new RelicEffectDataBuilder
                    {
                        RelicEffectClassType = typeof(RelicEffectPurgeTrainStewards),
                    },
                    new RelicEffectDataBuilder
                    {
                        RelicEffectClassType = typeof(RelicEffectAddUpgradedCardsStartOfRun),
                        ParamInt = 4,
                        ParamCardPool = cardPool,
                        ParamCardUpgradeDataBuilder = weakerUpgrade,
                    }
                },
                IconPath = "Assets/MTR_Colony.png",
            };
            var mutatorData = mutator.BuildAndRegister();

            var storyData = allGameData.FindStoryEventData(VanillaStoryEventIDs.UnitQuest);
            var existing = AccessTools.Field(typeof(StoryEventData), "excludedMutator").GetValue(storyData);
            if (existing != null)
            {
                Trainworks.Trainworks.Log(LogLevel.Error, "Someone's already set excluded mutator for UnitQuest aborting override.");
                return;
            }
            AccessTools.Field(typeof(StoryEventData), "excludedMutator").SetValue(storyData, mutatorData);
        }
    }
}
