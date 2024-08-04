using System;
using System.Collections.Generic;
using System.Text;

namespace BigOlBagOfMutators.RelicEffects
{
    public class RelicEffectAddAdditionalMerchantEnhancers : RelicEffectBase, IRelicEffect
    {
        private List<EnhancerData> upgradePool;

        public override void Initialize(RelicState relicState, RelicData relicData, RelicEffectData relicEffectData)
        {
            base.Initialize(relicState, relicData, relicEffectData);
            upgradePool = relicEffectData.GetParamEnhancerPool().GetAllChoices();
        }

        public void ModifyMerchantEnhancerList(List<EnhancerData> enhancers)
        {
            foreach (var enhancer in upgradePool)
            {
                if (!enhancers.Contains(enhancer))
                {
                    enhancers.Add(enhancer);
                }
            }
        }
    }

    /* Usage is over in RelicEffectBanEnhancer.cs */
}
