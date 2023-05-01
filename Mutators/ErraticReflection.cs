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
            CollectableRelicData capriciousReflection = CustomCollectableRelicManager.GetRelicDataByID(VanillaRelicIDs.CapriciousReflection);
            var effect = capriciousReflection.GetFirstRelicEffectData<RelicEffectAddStartingUpgradeToCardDrafts>();

            RelicEffectDataBuilder fillAllUpgradeSlots = new RelicEffectDataBuilder
            {
                RelicEffectClassType = typeof(RelicEffectFillEnhancerSlots),
                ParamEnhancerPool = effect.GetParamEnhancerPool(),
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
                BoonValue = 0,
                IconPath = "Assets/MTR_infinityMirror.png",
            };
            builder.BuildAndRegister();
        }
    }
}
