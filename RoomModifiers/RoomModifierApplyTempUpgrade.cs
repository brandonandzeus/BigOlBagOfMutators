using System;
using System.Collections.Generic;
using System.Text;
using Trainworks.Managers;

namespace BigOlBagOfMutators.RoomModifiers
{
    public class RoomModifierApplyTempUpgrade : RoomStateModifierBase, IRoomStateRoomSelectedModifier, IRoomStateSpawnPointsChangedModifier, IRoomStateCardManagerModifier
    {
        private class ModifiedCardTracker
        {
            private Dictionary<CardState, CardUpgradeState> modifiedCards = new Dictionary<CardState, CardUpgradeState>(20);

            private string trackerId = Guid.NewGuid().ToString();

            public void AddUpgradeToCard(CardState cardState, CardUpgradeData cardUpgradeData)
            {
                if (!HasCardBeenModified(cardState))
                {
                    CardUpgradeState cardUpgradeState = null;
                    foreach (CardUpgradeState cardUpgrade in cardState.GetTemporaryCardStateModifiers().GetCardUpgrades())
                    {
                        if (cardUpgrade.GetCardUpgradeDataId() == trackerId)
                        {
                            cardUpgradeState = cardUpgrade;
                            break;
                        }
                    }
                    if (cardUpgradeState == null)
                    {
                        cardUpgradeState = new CardUpgradeState();
                        cardUpgradeState.Setup(cardUpgradeData);
                        cardUpgradeState.SetCardUpgradeDataId(trackerId);
                        int magicPowerMultiplierFromTraits = cardState.GetMagicPowerMultiplierFromTraits();
                        if (magicPowerMultiplierFromTraits > 1 && cardUpgradeData.GetBonusDamage() == cardUpgradeState.GetAttackDamage())
                        {
                            cardUpgradeState.SetAttackDamage(cardUpgradeState.GetAttackDamage() * magicPowerMultiplierFromTraits);
                            cardUpgradeState.SetAdditionalHeal(cardUpgradeState.GetAdditionalHeal() * magicPowerMultiplierFromTraits);
                        }
                        cardState.GetTemporaryCardStateModifiers().AddUpgrade(cardUpgradeState);
                    }
                    modifiedCards.Add(cardState, cardUpgradeState);
                }
                cardState.UpdateDamageText();
                cardState.UpdateCardBodyText();
            }

            public void ClearTracking()
            {
                foreach (KeyValuePair<CardState, CardUpgradeState> item in modifiedCards)
                {
                    CardState key = item.Key;
                    key.GetTemporaryCardStateModifiers().RemoveUpgrade(item.Value);
                    key.UpdateDamageText();
                    key.UpdateCardBodyText();
                }
                modifiedCards.Clear();
            }

            public void ClearUpgrade(CardState cardState)
            {
                if (modifiedCards.ContainsKey(cardState))
                {
                    cardState.GetTemporaryCardStateModifiers().RemoveUpgrade(modifiedCards[cardState]);
                    cardState.UpdateDamageText();
                    cardState.UpdateCardBodyText();
                    modifiedCards.Remove(cardState);
                }
            }

            public bool HasCardBeenModified(CardState card)
            {
                return modifiedCards.ContainsKey(card);
            }

            public CardUpgradeState GetCardUpgradeState(CardState card)
            {
                if (modifiedCards.ContainsKey(card))
                {
                    return modifiedCards[card];
                }
                return null;
            }
        }

        private readonly ModifiedCardTracker modifiedCards = new ModifiedCardTracker();

        public bool CanApplyInPreviewMode => false;
        private CardUpgradeData cardUpgradeData;
        private bool removeIfDiscarded;

        public override void Initialize(RoomModifierData roomModifierData, RoomManager roomManager)
        {
            base.Initialize(roomModifierData, roomManager);
            cardUpgradeData = roomModifierData.GetParamCardUpgradeData();
            removeIfDiscarded = (roomModifierData.GetParamInt() == 0);
        }

        public void RoomSelectionChanged(bool roomSelected, CardManager cardManager)
        {
            if (roomSelected)
            {
                ApplyUpgradeToCardsInHand(cardManager);
            }
            else
            {
                modifiedCards.ClearTracking();
            }
        }

        public void SpawnPointChanged(CharacterState characterState, SpawnPoint prevPoint, SpawnPoint newPoint, CardManager cardManager)
        {
            bool num = newPoint?.GetRoomOwner().GetSelected() ?? false;
            bool flag = prevPoint?.GetRoomOwner().GetSelected() ?? false;
            if (num)
            {
                ApplyUpgradeToCardsInHand(cardManager);
            }
            else if (flag || ((object)characterState != null && characterState.IsDead))
            {
                modifiedCards.ClearTracking();
            }
        }

        public void CardDiscarded(CardState cardState)
        {
            if (!removeIfDiscarded)
                return;
            modifiedCards.ClearUpgrade(cardState);
        }

        public void CardDrawn(CardState cardState)
        {
            ApplyCardUpgrade(cardState);
        }

        private void ApplyUpgradeToCardsInHand(CardManager cardManager)
        {
            foreach (CardState item in cardManager.GetHand())
            {
                if (!modifiedCards.HasCardBeenModified(item))
                {
                    ApplyCardUpgrade(item);
                    cardManager?.RefreshCardInHand(item);
                }
            }
        }

        private void ApplyCardUpgrade(CardState cardState)
        {
            foreach (var filter in cardUpgradeData.GetFilters())
            {
                if (!filter.FilterCard(cardState, ProviderManager.SaveManager.RelicManager))
                {
                    return;
                }
            }
            modifiedCards.AddUpgradeToCard(cardState, cardUpgradeData);
        }
    }

}
