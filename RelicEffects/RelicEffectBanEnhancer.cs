using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using Trainworks.ConstantsV2;
using Trainworks.Managers;
using UnityEngine.Events;

namespace BigOlBagOfMutators.RelicEffects
{
    public class RelicEffectBanEnhancer : RelicEffectBase, IRelicEffect
    {
        private string enhancerID;

        public override void Initialize(RelicState relicState, RelicData relicData, RelicEffectData relicEffectData)
        {
            base.Initialize(relicState, relicData, relicEffectData);
            enhancerID = relicEffectData.GetParamString();
        }

        public string GetBannedEnhancerID() { return enhancerID; }
    }


    [HarmonyPatch(typeof(EnhancerPool), nameof(EnhancerPool.GetFilteredChoices))]
    class BanEnhancerPatch
    {
        private static readonly List<String> MerchantEnhancerPoolIDs = new List<string>
        {
            VanillaEnhancerPoolIDs.SpellUpgradePoolCostReduction,
            VanillaEnhancerPoolIDs.SpellUpgradePoolCommon,
            VanillaEnhancerPoolIDs.SpellUpgradePool,
            VanillaEnhancerPoolIDs.UnitUpgradePoolCommon,
            VanillaEnhancerPoolIDs.UnitUpgradePool,
            VanillaEnhancerPoolIDs.SpellUpgradePoolDarkPactCommon,
            VanillaEnhancerPoolIDs.SpellUpgradePoolDarkPactUncommon,
        };

        public static void Postfix(EnhancerPool __instance, ref List<EnhancerData> __result)
        {
            var name = __instance.name;
            if (MerchantEnhancerPoolIDs.Contains(name))
            {
                foreach (RelicEffectBanEnhancer effect in ProviderManager.SaveManager.RelicManager.GetRelicEffects<RelicEffectBanEnhancer>())
                {
                    __result.RemoveAll((EnhancerData enhancer) => enhancer.GetID() == effect.GetBannedEnhancerID());
                }
            }
            if (name == VanillaEnhancerPoolIDs.UnitUpgradePoolCommon)
            {
                foreach (var effect in ProviderManager.SaveManager.RelicManager.GetRelicEffects<RelicEffectAddAdditionalMerchantEnhancers>())
                {
                    effect.ModifyMerchantEnhancerList(__result);
                }
            }
        }
    }
}
