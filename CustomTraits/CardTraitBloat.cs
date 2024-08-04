using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BigOlBagOfMutators.CustomTraits
{
    public class CardTraitBloat : CardTraitState
    {
        public override IEnumerator OnCardDiscarded(CardManager.DiscardCardParams discardCardParams, CardManager cardManager, RelicManager relicManager, CombatManager combatManager, RoomManager roomManager, SaveManager saveManager)
        {
            if (!discardCardParams.wasPlayed)
            {
                yield break;
            }
            CardData cardData = saveManager.GetAllGameData().FindCardData(discardCardParams.discardCard.GetCardDataID());
            // Just in case. Technically the card data SHOULD exist though.
            if (cardData == null)
            {
                yield break;
            }
            CardManager.AddCardUpgradingInfo addCardUpgradingInfo = new CardManager.AddCardUpgradingInfo();
            if (GetCardTraitData().GetCardUpgradeDataParam() != null)
            {
                addCardUpgradingInfo.upgradeDatas.Add(GetCardTraitData().GetCardUpgradeDataParam());
            }
            addCardUpgradingInfo.tempCardUpgrade = true;
            addCardUpgradingInfo.upgradingCardSource = discardCardParams.discardCard;
            addCardUpgradingInfo.ignoreTempUpgrades = false;
            addCardUpgradingInfo.copyModifiersFromCard = discardCardParams.discardCard;
            cardManager.AddCard(cardData, CardPile.DiscardPile, 1, 1, fromRelic: false, permanent: false, addCardUpgradingInfo);
        }
    }
}
