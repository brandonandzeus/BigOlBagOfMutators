using System;
using System.Collections.Generic;
using System.Text;

namespace BigOlBagOfMutators.RelicEffects
{
    public class RelicEffectAddUpgradedCardsStartOfRun : RelicEffectAddCardBase, IStartOfRunRelicEffect, IRelicEffect
    {
        private int amount;
        private CardUpgradeData cardUpgrade;

        public override void Initialize(RelicState relicState, RelicData relicData, RelicEffectData relicEffectData)
        {
            base.Initialize(relicState, relicData, relicEffectData);
            amount = relicEffectData.GetParamInt();
            cardUpgrade = relicEffectData.GetParamCardUpgradeData();
            getAllCardsInPool = false;
        }

        public void ApplyEffect(RelicEffectParams relicEffectParams)
        {
            if (cardPool == null)
            {
                return;
            }
            for (int i = 0; i < amount; i++)
            {
                CardData randomChoice = cardPool.GetRandomChoice(RngId.SetupRun);
                CardState cardState = relicEffectParams.saveManager.AddCardToDeck(randomChoice);
                CardUpgradeState cardUpgradeState = new CardUpgradeState();
                cardUpgradeState.Setup(cardUpgrade);
                cardState.Upgrade(cardUpgradeState, relicEffectParams.saveManager);
            }
        }
    }
}
