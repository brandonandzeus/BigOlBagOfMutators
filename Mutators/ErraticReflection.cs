using BigOlBagOfMutators.RelicEffects;
using System;
using System.Collections.Generic;
using System.Text;
using Trainworks.BuildersV2;
using Trainworks.ConstantsV2;
using Trainworks.Managers;

namespace BigOlBagOfMutators.Mutators
{
    public static class ErraticReflection
    {
        public static void Make()
        {
            var allGameData = ProviderManager.SaveManager.GetAllGameData();
            EnhancerPool enhancerPool = new EnhancerPoolBuilder
            {
                EnhancerPoolID = "ErraticReflectionPool",
                Enhancers =
                {
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Powerstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Powerstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Powerstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Surgestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Surgestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Emberstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Emberstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Emberstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Stackstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Freezestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Freezestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Keepstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Eternalstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Eternalstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.SpellRailspike),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.UnhingedPower),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Seekstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Truestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Truestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Purgestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Twinstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Twinstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Valuestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Valuestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Extremestone),

                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Powerstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Powerstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Powerstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Surgestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Surgestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Emberstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Emberstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Emberstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Stackstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Freezestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Freezestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Keepstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Eternalstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Eternalstone),
                    //allGameData.FindEnhancerData(VanillaEnhancerIDs.SpellRailspike),
                    //allGameData.FindEnhancerData(VanillaEnhancerIDs.UnhingedPower),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Seekstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Truestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Truestone),
                    //allGameData.FindEnhancerData(VanillaEnhancerIDs.Purgestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Twinstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Twinstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Valuestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Valuestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Extremestone),

                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Immortalstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Immortalstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Immortalstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Largestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Largestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Largestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Thornstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Thornstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Furystone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Furystone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Wickstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Runestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Runestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Shieldstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Shieldstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Frenzystone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Frenzystone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Frenzystone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Speedstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Speedstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Speedstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Strengthstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Strengthstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Battlestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Battlestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Heartstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Heartstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Summonstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Summonstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.EmberstoneUnit),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.EmberstoneUnit),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.MajorRefraction),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.MinorRefraction),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.MonsterRailspike),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Sunderstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Sapstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.HephsConsolation),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.HephsConsolation),


                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Immortalstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Immortalstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Immortalstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Largestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Largestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Largestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Thornstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Thornstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Furystone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Furystone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Wickstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Runestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Runestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Shieldstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Shieldstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Frenzystone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Frenzystone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Frenzystone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Speedstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Speedstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Speedstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Strengthstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Strengthstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Battlestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Battlestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Heartstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Heartstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Summonstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Summonstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.EmberstoneUnit),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.EmberstoneUnit),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.MajorRefraction),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.MinorRefraction),
                    //allGameData.FindEnhancerData(VanillaEnhancerIDs.MonsterRailspike),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Sunderstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Sapstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.HephsConsolation),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.HephsConsolation),
                }
            }.BuildAndRegister();

            RelicEffectDataBuilder fillAllUpgradeSlots = new RelicEffectDataBuilder
            {
                RelicEffectClassType = typeof(RelicEffectFillEnhancerSlots),
                ParamEnhancerPool = enhancerPool,
            };

            CollectableRelicDataBuilder relicChoaticReflection = new CollectableRelicDataBuilder
            {
                CollectableRelicID = "ChaoticReflection",
                Name = "Chaotic Reflection",
                Description = "Cards in reward packs and unit banners come with all upgrade slots prefilled.",
                IconPath = "Assets/ChaoticReflection.png",
                RelicPoolIDs = new List<string>(),
                EffectBuilders = { fillAllUpgradeSlots },
            };

            MutatorDataBuilder builder = new MutatorDataBuilder
            {
                MutatorID = "ErraticReflection",
                Name = "Erratic Reflection",
                Description = "Start with Chaotic Reflection.",
                EffectBuilders = {
                    new RelicEffectDataBuilder
                    {
                        RelicEffectClassType = typeof(RelicEffectAddRelicStartOfRun),
                        ParamRelic = relicChoaticReflection.BuildAndRegister(),
                    },
                },
                BoonValue = 2,
                RequiredDLC = ShinyShoe.DLC.Hellforged,
                IconPath = "Assets/MTR_infinityMirror.png",
            };
            builder.BuildAndRegister();
        }
    }
}
