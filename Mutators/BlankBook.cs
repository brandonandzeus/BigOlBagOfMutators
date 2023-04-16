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
    class BlankBook
    {
        public static void Make()
        {
            CollectableRelicData collectableRelicData = CustomCollectableRelicManager.GetRelicDataByID(VanillaRelicIDs.BlankPages);
            RelicEffectData addChampionEffect = collectableRelicData.GetEffects()[0];
            CardPool cardPool = addChampionEffect.GetParamCardPool();

            RelicEffectDataBuilder addChampionV1 = new RelicEffectDataBuilder
            {
                RelicEffectClassType = typeof(RelicEffectAddChampionCardToHandBasedOnDistance),
                ParamCardPool = cardPool,
                ParamRandomChampionPool = FormMegaPool(cardPool),
            };

            CollectableRelicDataBuilder relicEmptyBookV1 = new CollectableRelicDataBuilder
            {
                CollectableRelicID = "BlankBook",
                Name = "Blank Book",
                Description = "Get a random champion at the start of the turn if you don't have one in your hand.",
                IconPath = "BlankBook.png",
                RelicPoolIDs = new List<string>(),
                EffectBuilders = { addChampionV1 },
            };

            MutatorDataBuilder blankPages = new MutatorDataBuilder
            {
                MutatorID = "Mutator_BlankBook",
                Name = "Blank Book",
                Description = "Remove the Champion from your starting deck. Start with Blank Book.",
                EffectBuilders = new List<RelicEffectDataBuilder>
                {
                    new RelicEffectDataBuilder
                    {
                        RelicEffectClassType = typeof(RelicEffectPurgeChampion),
                    },
                    new RelicEffectDataBuilder
                    {
                        RelicEffectClassType = typeof(RelicEffectAddRelicStartOfRun),
                        ParamRelic = relicEmptyBookV1.BuildAndRegister(),
                    },
                },
                BoonValue = 2,
                Tags = new List<string> { "champion" },
                IconPath = "MTR_BlankBook.png",
            };

            blankPages.BuildAndRegister();
        }

        /// <summary>
        /// Generates a Mega Champion Pool with every champion with every pure upgrade path.
        /// </summary>
        /// <param name="champions">CardPool of champion cards to consider</param>
        /// <returns>RandomChampionPool</returns>
        public static RandomChampionPool FormMegaPool(CardPool champions)
        {
            RandomChampionPoolBuilder builder = new RandomChampionPoolBuilder();
            builder.ChampionPoolID = "BlankBookChampions";

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
    // TODO See if I can find a way to sneak FallenChampion's mutator data into SaveData.RelicStates.
    [HarmonyPatch(typeof(MapNodeBucketContainer), nameof(MapNodeBucketContainer.HasExcludedCovenantOrMutator))]
    class BlankBookBanChampionUpgrades
    {
        static readonly List<string> BannedMapNodes = new List<string> { VanillaMapNodePoolIDs.ChampionUpgradeI, VanillaMapNodePoolIDs.ChampionUpgradeII, VanillaMapNodePoolIDs.ChampionUpgradeIII };
        static string BlankBookID = GUIDGenerator.GenerateDeterministicGUID("Mutator_BlankBook");

        public static void Postfix(ref bool __result, string ___id, List<RelicState> relicStates)
        {
            if (relicStates == null || !BannedMapNodes.Contains(___id))
            {
                return;
            }
            foreach (RelicState state in relicStates)
            {
                if (state.GetRelicDataID() == BlankBookID)
                {
                    __result = true;
                    return;
                }
            }
        }
    }
}
