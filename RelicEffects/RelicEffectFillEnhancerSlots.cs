using System;
using System.Collections.Generic;
using System.Text;

namespace BigOlBagOfMutators.RelicEffects
{
    public class RelicEffectFillEnhancerSlots : RelicEffectBase, IAddStartingUpgradeToCardDrafts, IRelicEffect
    {
        private EnhancerPool enhancerPool;

        public override void Initialize(RelicState relicState, RelicData relicData, RelicEffectData relicEffectData)
        {
            base.Initialize(relicState, relicData, relicEffectData);
            enhancerPool = relicEffectData.GetParamEnhancerPool();
        }

        public void UpgradeCard(CardDrawnRelicEffectParams relicEffectParams)
        {
            CardState cardState = relicEffectParams.cardState;
            List<EnhancerData> allChoices = enhancerPool.GetAllChoices();
            int upgradeCount = cardState.GetVisibleUpgradeCount();
            int times = relicEffectParams.saveManager.GetAllGameData().GetBalanceData().GetUpgradeSlots(cardState.GetCardType(), relicEffectParams.relicManager.GetRelicEffects<IModifyCardUpgradeSlotCountRelicEffect>());
            Trainworks.Trainworks.Log("" + cardState + " " + times + " " + upgradeCount);
            for (int i = 0; i < times - upgradeCount; i++) 
            {
                for (int num = allChoices.Count - 1; num >= 0; num--)
                {
                    foreach (RelicEffectData effect in allChoices[num].GetEffects())
                    {
                        if (effect.GetParamCardType() != relicEffectParams.cardState.GetCardType())
                        {
                            allChoices.RemoveAt(num);
                            break;
                        }
                        CardUpgradeData paramCardUpgradeData = effect.GetParamCardUpgradeData();
                        if (!(paramCardUpgradeData != null))
                        {
                            continue;
                        }
                        foreach (CardUpgradeMaskData filter in paramCardUpgradeData.GetFilters())
                        {
                            if (!filter.FilterCard(cardState, relicEffectParams.relicManager))
                            {
                                allChoices.RemoveAt(num);
                                break;
                            }
                        }
                    }
                }
                if (allChoices.Count > 0)
                {
                    CardUpgradeData paramCardUpgradeData2 = allChoices.RandomElement(RngId.Rewards).GetEffects()[0].GetParamCardUpgradeData();
                    CardUpgradeState cardUpgradeState = Activator.CreateInstance<CardUpgradeState>();
                    cardUpgradeState.Setup(paramCardUpgradeData2);
                    relicEffectParams.cardState.Upgrade(cardUpgradeState, relicEffectParams.saveManager, ignoreUpgradeAnimation: true);
                }
            }
        }
    }
}
