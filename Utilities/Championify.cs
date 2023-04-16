using System;
using System.Collections.Generic;
using System.Text;
using Trainworks.BuildersV2;
using Trainworks.ConstantsV2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using HarmonyLib;

namespace BigOlBagOfMutators.Utilities
{
    public static class Championify
    {
        public static Dictionary<CharacterData, CardUpgradeTreeData> FakeChampions = new Dictionary<CharacterData, CardUpgradeTreeData>();

        public static CardData TurnCharacterIntoChampion(CharacterData character, CardData card, CardUpgradeTreeData upgradeTreeData)
        {
            CharacterDataBuilder clone = new CharacterDataBuilder
            {
                CharacterID = character.name + "_Champion",
                NameKey = character.GetNameKey(),
                AttackDamage = character.GetAttackDamage(),
                Health = character.GetHealth(),
                Size = character.GetSize(),
                Triggers = character.GetTriggers(),
                StatusEffectImmunities = character.GetStatusEffectImmunities(),
                CharacterPrefabVariantRef = (AssetReferenceGameObject)AccessTools.Field(typeof(CharacterData), "characterPrefabVariantRef").GetValue(character),
                CanAttack = character.GetCanAttack(),
                CanBeHealed = character.GetCanBeHealed(),
                DeathSlidesBackwards = character.IsDeathSlidesBackwards(),
                CharacterLoreTooltipKeys = character.GetCharacterLoreTooltipKeys(),
                RoomModifiers = character.GetRoomModifiersData(),
                AttackTeleportsToDefender = character.IsAttackTeleportsToDefender(),
                SubtypeKeys = (List<string>)AccessTools.Field(typeof(CharacterData), "subtypeKeys").GetValue(character),
                CharacterSoundData = character.GetCharacterSoundData(),
                CharacterChatterData = character.GetCharacterChatterData(),
                DeathType = character.GetDeathType(),
                DeathVFX = character.GetDeathVfx(),
                ImpactVFX = character.GetImpactVfx(),
                ProjectilePrefab = character.GetProjectilePrefab(),
                BlockVisualSizeIncrease = character.BlockVisualSizeIncrease(),
                AscendsTrainAutomatically = character.GetAscendsTrainAutomatically(),
            };

            clone.StartingStatusEffects.AddRange(character.GetStartingStatusEffects());

            clone.SubtypeKeys.Add(VanillaSubtypeIDs.Champion);
            CharacterData cloneCharacter = clone.BuildAndRegister();

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
                ClanID = VanillaClanIDs.Hellhorned,

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
