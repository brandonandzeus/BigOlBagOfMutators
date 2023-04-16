using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BigOlBagOfMutators.RelicEffects
{
    public class RelicEffectAddChampionCardToHandBasedOnDistanceBase : RelicEffectBase
    {
        private RandomChampionPool cardPool;

        private RandomChampionPool.GrantedChampionInfo championToAdd;
        private int numChoices;

        public override void Initialize(RelicState relicState, RelicData relicData, RelicEffectData relicEffectData)
        {
            base.Initialize(relicState, relicData, relicEffectData);
            cardPool = relicEffectData.GetParamRandomChampionPool();
            numChoices = cardPool.GetAllChampions().Count / 3;
        }

        public List<CardData> GetAllPossibleCardsGivenByRelic()
        {
            return cardPool.GetAllUniqueCards();
        }

        public bool TestEffect(RelicEffectParams relicEffectParams)
        {
            championToAdd = null;
            if (relicEffectParams.cardManager.GetBelowHandSize() && !DoesHandHaveChampion(relicEffectParams.cardManager))
            {
                championToAdd = GetChampionToAdd(relicEffectParams.saveManager);
            }
            return championToAdd != null;
        }

        public IEnumerator ApplyEffect(RelicEffectParams relicEffectParams)
        {
            if (championToAdd == null)
            {
                yield break;
            }
            CardManager cardManager = relicEffectParams.cardManager;
            bool pileUpdated = false;
            cardManager.cardPilesChangedSignal.AddOnce(delegate
            {
                pileUpdated = true;
            });
            CardManager.AddCardUpgradingInfo addCardUpgradingInfo = new CardManager.AddCardUpgradingInfo();
            foreach (CardUpgradeData upgrade in championToAdd.upgrades)
            {
                addCardUpgradingInfo.upgradeDatas.Add(upgrade);
            }
            if (cardManager.AddCard(championToAdd.championCard, CardPile.HandPile, 1, 1, fromRelic: true, permanent: false, addCardUpgradingInfo) == null)
            {
                pileUpdated = true;
            }
            yield return new WaitUntil(() => pileUpdated);
        }

        protected virtual bool DoesHandHaveChampion(CardManager cardManager)
        {
            foreach (CardState item in cardManager.GetHand())
            {
                if (item.IsChampionCard())
                {
                    return true;
                }
            }
            return false;
        }

        private RandomChampionPool.GrantedChampionInfo GetChampionToAdd(SaveManager saveManager)
        {
            if (cardPool == null)
            {
                return null;
            }

            int distance = saveManager.GetCurrentDistance();
            int level = GetLevelForDistance(distance);
            List<RandomChampionPool.GrantedChampionInfo> allChampions = cardPool.GetAllChampions();
            if (allChampions.Count == 0)
            {
                return null;
            }
            int choice = RandomManager.Range(0, numChoices, RngId.Battle);

            return allChampions[choice + level * numChoices];
        }

        private int GetLevelForDistance(int distance)
        {
            if (distance <= 2)
                return 0;
            if (distance <= 5)
                return 1;
            else
                return 2;
        }
    }

    public class RelicEffectAddChampionCardToHandBasedOnDistance : RelicEffectAddChampionCardToHandBasedOnDistanceBase, IStartOfPlayerTurnAfterDrawRelicEffect, ITurnTimingRelicEffect, IRelicEffect, IRelicGiveCardsEffect
    {

    }

    public class RelicEffectAddChampionCardToHandAtTheStartOfBattle : RelicEffectAddChampionCardToHandBasedOnDistanceBase, IStartOfCombatRelicEffect, IRelicEffect, IRelicGiveCardsEffect
    {
        // Disable.
        protected override bool DoesHandHaveChampion(CardManager cardManager)
        {
            return false;
        }
    }

}
