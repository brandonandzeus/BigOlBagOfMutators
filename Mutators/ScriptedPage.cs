using Trainworks.BuildersV2;
using Trainworks.Managers;
using Trainworks.ConstantsV2;
using Trainworks.Utilities;
using System.Collections.Generic;
using System;
using HarmonyLib;
using Malee;
using System.Collections;
using UnityEngine;
using BigOlBagOfMutators.RelicEffects;

namespace BigOlBagOfMutators.Mutators
{
    class ScriptedPage
    {
        public static void Make()
        {

            CollectableRelicData collectableRelicData = CustomCollectableRelicManager.GetRelicDataByID(VanillaRelicIDs.BlankPages);
            RelicEffectData addChampionEffect = collectableRelicData.GetEffects()[0];
            CardPool cardPool = addChampionEffect.GetParamCardPool();

            RelicEffectDataBuilder addChampion = new RelicEffectDataBuilder
            {
                RelicEffectClassType = typeof(RelicEffectAddChampionCardToHandAtTheStartOfBattle),
                ParamCardPool = cardPool,
                ParamRandomChampionPool = FormMegaPool(cardPool),
            };

            CollectableRelicDataBuilder relicScriptedPage = new CollectableRelicDataBuilder
            {
                CollectableRelicID = "ScriptedPage",
                Name = "Scripted Page",
                Description = "Get a random champion at the start of battle.",
                IconPath = "Assets/ScriptedPage.png",
                RelicPoolIDs = new List<string>(),
                EffectBuilders = { addChampion },
            };

            MutatorDataBuilder scriptedPage = new MutatorDataBuilder
            {
                MutatorID = "Mutator_ScriptedPage",
                Name = "Scripted Page",
                Description = "Remove the Champion from your starting deck. Start with Scripted Page.",
                EffectBuilders = new List<RelicEffectDataBuilder>
                {
                    new RelicEffectDataBuilder
                    {
                        RelicEffectClassType = typeof(RelicEffectPurgeChampion),
                    },
                    new RelicEffectDataBuilder
                    {
                        RelicEffectClassType = typeof(RelicEffectAddRelicStartOfRun),
                        ParamRelic = relicScriptedPage.BuildAndRegister(),
                    }
                },
                BoonValue = -3,
                Tags = new List<string> { "champion" },
                IconPath = "Assets/MTR_ScriptedPage.png",
                RequiredDLC = ShinyShoe.DLC.Hellforged,
            };

            scriptedPage.BuildAndRegister();
        }

        /// <summary>
        /// Generates a Mega Champion Pool with every champion with every pure upgrade path.
        /// </summary>
        /// <param name="champions">CardPool of champion cards to consider</param>
        /// <returns>RandomChampionPool</returns>
        public static RandomChampionPool FormMegaPool(CardPool champions)
        {
            RandomChampionPoolBuilder builder = new RandomChampionPoolBuilder();
            builder.ChampionPoolID = "ScriptedPageChampions";

            for (int level = 0; level < 3; level++)
            {
                foreach (CardData card in champions.GetAllChoices())
                {
                    ChampionData champion = FindChampion(card);
                    if (champion == null)
                    {
                        Trainworks.Trainworks.Log("Unexpectedly could not find ChampionData for card: " + card);
                        continue;
                    }
                    foreach (var upgradeTree in champion.upgradeTree.GetUpgradeTrees())
                    {
                        RandomChampionPool.GrantedChampionInfo championInfo = new RandomChampionPool.GrantedChampionInfo
                        {
                            championCard = card,
                            upgrades = new List<CardUpgradeData>() { upgradeTree.GetCardUpgrades()[level] }
                        };
                        builder.ChampionInfoList.Add(championInfo);
                    }
                }
            }

            return builder.Build();
        }

        private static ChampionData FindChampion(CardData card)
        {
            foreach (ClassData classData in ProviderManager.SaveManager.GetAllGameData().GetAllClassDatas())
            {
                var standard = classData.GetChampionData(0);
                if (standard.championCardData == card)
                    return standard;
                var exiled = classData.GetChampionData(1);
                if (exiled.championCardData == card)
                    return exiled;
            }
            return null;
        }
    }

    // Unfortunately the only way to do this.
    [HarmonyPatch(typeof(MapNodeBucketContainer), nameof(MapNodeBucketContainer.HasExcludedCovenantOrMutator))]
    class ScriptedPageBanChampionUpgrades
    {
        static readonly List<string> BannedMapNodes = new List<string> { VanillaMapNodePoolIDs.ChampionUpgradeI, VanillaMapNodePoolIDs.ChampionUpgradeII, VanillaMapNodePoolIDs.ChampionUpgradeIII };
        static string ScriptedPageID = GUIDGenerator.GenerateDeterministicGUID("Mutator_ScriptedPage");

        public static void Postfix(ref bool __result, string ___id, List<RelicState> relicStates)
        {
            if (relicStates == null || !BannedMapNodes.Contains(___id))
            {
                return;
            }
            foreach (RelicState state in relicStates)
            {
                if (state.GetRelicDataID() == ScriptedPageID)
                {
                    __result = true;
                    return;
                }
            }
        }
    }

}
