using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using Trainworks.BuildersV2;
using Trainworks.Managers;
using static CardUpgradeTreeData;

namespace BigOlBagOfMutators.Mutators
{
    public class JackofAllTrades
    {
        public static void Make()
        {

            MutatorDataBuilder builder = new MutatorDataBuilder
            {
                MutatorID = "JackofAllTrades",
                Name = "Jack of all trades",
                Description = "Your champion can only select an upgrade path once.",
                EffectBuilders =
                {
                    new RelicEffectDataBuilder
                    {
                        RelicEffectClassType = typeof(RelicEffectJackofAllTrades),
                    }
                },
                BoonValue = 0,
                IconPath = "Assets/MTR_JackOfAllTrades.png",
            };

            builder.BuildAndRegister();
        }
    }

    public class RelicEffectJackofAllTrades : RelicEffectBase, IRelicEffect
    {
    }

    [HarmonyPatch(typeof(CardUpgradeTreeData), nameof(CardUpgradeTreeData.GetRandomChoices))]
    class JackOfAllTradesPatch
    {
        public static bool Prefix(CardUpgradeTreeData __instance, int numOptionsDisplayed, CardState championCard, ref List<CardUpgradeData> __result)
        {
            var effect = ProviderManager.SaveManager.RelicManager.GetRelicEffect<RelicEffectJackofAllTrades>();
            if (effect == null)
                return true;

            List<CardUpgradeData> list = new List<CardUpgradeData>();
            List<CardUpgradeState> cardUpgrades = championCard.GetCardStateModifiers().GetCardUpgrades();
            foreach (UpgradeTree upgradeTree in __instance.GetUpgradeTrees())
            {
                bool flag = false;
                foreach (CardUpgradeState item in cardUpgrades)
                {
                    if (upgradeTree.GetNextUpgradeIfHasUpgrade(item, out var nextUpgrade))
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    var upgrade = upgradeTree.GetFirstUpgrade();
                    if (upgrade != null)
                        list.Add(upgrade);
                }
            }
            list.Shuffle(RngId.Rewards);

            if (list.Count > numOptionsDisplayed)
            {
                list.RemoveRange(numOptionsDisplayed, list.Count - numOptionsDisplayed);
            }
            __result = list;
            return false;
        }
    }
}
