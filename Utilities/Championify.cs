using System;
using System.Collections.Generic;
using System.Text;
using Trainworks.BuildersV2;
using Trainworks.ConstantsV2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using HarmonyLib;
using Trainworks.Utilities;

namespace BigOlBagOfMutators.Utilities
{
    public static class Championify
    {
        public static Dictionary<CharacterData, CardUpgradeTreeData> FakeChampions = new Dictionary<CharacterData, CardUpgradeTreeData>();

        public static CardData TurnCharacterIntoChampion(CharacterData character, CardData card, CardUpgradeTreeData upgradeTreeData, int? overrideAttack = null, int? overrideHealth = null, int? overrideSize = null, BundleAssetLoadingInfo bundleInfo = null)
        {
            CharacterDataBuilder clone = new CharacterDataBuilder
            {
                CharacterID = character.name + "_Champion",
                NameKey = character.GetNameKey(),
                AttackDamage = overrideAttack ?? character.GetAttackDamage(),
                Health = overrideHealth ?? character.GetHealth(),
                Size = overrideSize ?? character.GetSize(),
                Triggers = new List<CharacterTriggerData>(character.GetTriggers()),
                StatusEffectImmunities = character.GetStatusEffectImmunities(),
                CharacterPrefabVariantRef = (bundleInfo == null) ? (AssetReferenceGameObject)AccessTools.Field(typeof(CharacterData), "characterPrefabVariantRef").GetValue(character) : null,
                CanAttack = character.GetCanAttack(),
                CanBeHealed = character.GetCanBeHealed(),
                DeathSlidesBackwards = character.IsDeathSlidesBackwards(),
                CharacterLoreTooltipKeys = character.GetCharacterLoreTooltipKeys(),
                RoomModifiers = new List<RoomModifierData>(character.GetRoomModifiersData()),
                AttackTeleportsToDefender = character.IsAttackTeleportsToDefender(),
                SubtypeKeys = new List<string>(((List<string>)AccessTools.Field(typeof(CharacterData), "subtypeKeys").GetValue(character))),
                CharacterSoundData = character.GetCharacterSoundData(),
                CharacterChatterData = character.GetCharacterChatterData(),
                DeathType = character.GetDeathType(),
                DeathVFX = character.GetDeathVfx(),
                ImpactVFX = character.GetImpactVfx(),
                ProjectilePrefab = character.GetProjectilePrefab(),
                BlockVisualSizeIncrease = character.BlockVisualSizeIncrease(),
                AscendsTrainAutomatically = character.GetAscendsTrainAutomatically(),
                BundleLoadingInfo = bundleInfo
            };
            
            clone.StartingStatusEffects.AddRange(character.GetStartingStatusEffects());

            clone.SubtypeKeys.Add(VanillaSubtypeIDs.Champion);
            CharacterData cloneCharacter = clone.BuildAndRegister();
            AccessTools.Field(typeof(CharacterData), "fallbackData").SetValue(cloneCharacter, AccessTools.Field(typeof(CharacterData), "fallbackData").GetValue(character));

            CardDataBuilder championCard = new CardDataBuilder
            {
                CardID = card.name + "_Champion",
                NameKey = card.GetNameKey(),
                Cost = 0,
                Rarity = CollectableRarity.Champion,

                EffectBuilders = {
                    new CardEffectDataBuilder
                    {
                        EffectStateType = typeof(CardEffectSpawnMonster),
                        TargetMode = TargetMode.DropTargetCharacter,
                        ParamCharacterData = cloneCharacter,
                    }
                },
                CardType = CardType.Monster,
                TargetsRoom = true,
                Targetless = false,
                CardArtPrefabVariantRef = (AssetReferenceGameObject)AccessTools.Field(typeof(CardData), "cardArtPrefabVariantRef").GetValue(card),
                IgnoreWhenCountingMastery = true,
                CardLoreTooltipKeys = card.GetCardLoreTooltipKeys(),
                ClanID = card.GetLinkedClassID(),
                RequiredDLC = card.GetRequiredDLC(),
                LinkedMasteryCard = card,
                SharedMasteryCards = {card},
            };

            CardData championCardData = championCard.BuildAndRegister();

            FakeChampions.Add(cloneCharacter, upgradeTreeData);

            return championCardData;
        }
    }

    [HarmonyPatch(typeof(ChampionUpgradeRewardData), nameof(ChampionUpgradeRewardData.GetUpgradeTree), new Type[] { typeof(CharacterData), typeof(BalanceData) })]
    class FakeChampionifyMutatorPatch
    {
        public static void Postfix(ref CardUpgradeTreeData __result, CharacterData championData)
        {
            if (Championify.FakeChampions.ContainsKey(championData))
            {
                __result = Championify.FakeChampions[championData];
            }
        }
    }

}
