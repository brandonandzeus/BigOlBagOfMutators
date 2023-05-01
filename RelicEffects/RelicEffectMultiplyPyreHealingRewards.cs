using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BigOlBagOfMutators.RelicEffects
{
    class RelicEffectMultiplyPyreHealingRewards : RelicEffectBase, IModifyTowerHealthRelicEffect
    {
        private float healingMultiplier;
        public override void Initialize(RelicState relicState, RelicData srcRelicData, RelicEffectData relicEffectData)
        {
            base.Initialize(relicState, srcRelicData, relicEffectData);
            healingMultiplier = relicEffectData.GetParamFloat();
        }
        public int ModifyTowerHealAmount(int amount)
        {
            return Mathf.FloorToInt(amount * healingMultiplier);
        }


    }
}
