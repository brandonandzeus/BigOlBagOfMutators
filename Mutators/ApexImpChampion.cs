using BigOlBagOfMutators.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using Trainworks.BuildersV2;
using Trainworks.ConstantsV2;
using Trainworks.Managers;

namespace BigOlBagOfMutators.Mutators
{
    class ApexImpChampion
    {
        public static void Make()
        {
            CharacterData apexImp = CustomCharacterManager.GetCharacterDataByID(VanillaCharacterIDs.ApexImp);
            CardData apexImpCard = CustomCardManager.GetCardDataByID(VanillaCardIDs.ApexImp);

            CardEffectDataBuilder gainArmorI = new CardEffectDataBuilder
            {
                EffectStateType = typeof(CardEffectAddStatusEffect),
                TargetMode = TargetMode.Self,
                TargetTeamType = Team.Type.Monsters,
                ParamStatusEffects =
                {
                    new StatusEffectStackData { statusId = StatusEffectArmorState.StatusId, count = 3 }
                },
            };
            CharacterTriggerDataBuilder shieldedIHit = new CharacterTriggerDataBuilder
            {
                TriggerID = "ProtectiveI",
                Trigger = CharacterTriggerData.Trigger.CardMonsterPlayed,
                Description = "Gain [armor] [effect0.status0.power].",
                EffectBuilders = { gainArmorI },
            };
            CardUpgradeDataBuilder shieldedI = new CardUpgradeDataBuilder
            {
                UpgradeID = "ProtectiveI",
                UpgradeTitle = "Protective I",
                StatusEffectUpgrades =
                {
                    new StatusEffectStackData { statusId = StatusEffectArmorState.StatusId, count = 5 },
                },
                TriggerUpgradeBuilders = { shieldedIHit }
            };

            CardEffectDataBuilder gainArmorII = new CardEffectDataBuilder
            {
                EffectStateType = typeof(CardEffectAddStatusEffect),
                TargetMode = TargetMode.Self,
                TargetTeamType = Team.Type.Monsters,
                ParamStatusEffects =
                {
                    new StatusEffectStackData { statusId = StatusEffectArmorState.StatusId, count = 5 }
                },
            };
            CharacterTriggerDataBuilder shieldedIIHit = new CharacterTriggerDataBuilder
            {
                TriggerID = "ProtectiveII",
                Trigger = CharacterTriggerData.Trigger.CardMonsterPlayed,
                Description = "Gain [armor] [effect0.status0.power].",
                EffectBuilders = { gainArmorII },
            };
            CardUpgradeDataBuilder shieldedII = new CardUpgradeDataBuilder
            {
                UpgradeID = "ProtectiveII",
                UpgradeTitle = "Protective II",
                StatusEffectUpgrades =
                {
                    new StatusEffectStackData {statusId = StatusEffectArmorState.StatusId, count = 15},
                },
                TriggerUpgradeBuilders = { shieldedIIHit }
            };

            CardEffectDataBuilder gainArmorIII = new CardEffectDataBuilder
            {
                EffectStateType = typeof(CardEffectAddStatusEffect),
                TargetMode = TargetMode.Self,
                TargetTeamType = Team.Type.Monsters,
                ParamStatusEffects =
                {
                    new StatusEffectStackData { statusId = StatusEffectArmorState.StatusId, count = 7 }
                },
            };
            CharacterTriggerDataBuilder shieldedIIIHit = new CharacterTriggerDataBuilder
            {
                TriggerID = "ProtectiveIII",
                Trigger = CharacterTriggerData.Trigger.CardMonsterPlayed,
                Description = "Gain [armor] [effect0.status0.power].",
                EffectBuilders = { gainArmorIII },
            };
            CardUpgradeDataBuilder shieldedIII = new CardUpgradeDataBuilder
            {
                UpgradeID = "ProtectiveIII",
                UpgradeTitle = "Protective III",
                StatusEffectUpgrades =
                {
                    new StatusEffectStackData {statusId = StatusEffectArmorState.StatusId, count = 25},
                },
                TriggerUpgradeBuilders = { shieldedIIIHit }
            };

            List<CardUpgradeDataBuilder> shielded = new List<CardUpgradeDataBuilder> { shieldedI, shieldedII, shieldedIII };


            CardEffectDataBuilder enrageAllI = new CardEffectDataBuilder
            {
                EffectStateType = typeof(CardEffectAddStatusEffect),
                TargetMode = TargetMode.Room,
                TargetTeamType = Team.Type.Monsters,
                ParamStatusEffects =
                {
                    new StatusEffectStackData {statusId = StatusEffectBuffState.StatusId, count = 2}
                }
            };
            CharacterTriggerDataBuilder enrageI = new CharacterTriggerDataBuilder
            {
                TriggerID = "EnragingI",
                Trigger = CharacterTriggerData.Trigger.OnHit,
                Description = "Apply [rage] [effect0.status0.power] to friendly units.",
                EffectBuilders = { enrageAllI },
            };
            CardUpgradeDataBuilder enragingI = new CardUpgradeDataBuilder
            {
                UpgradeID = "EnragingI",
                UpgradeTitle = "Enraging I",
                TriggerUpgradeBuilders = { enrageI },
                StatusEffectUpgrades =
                {
                    new StatusEffectStackData {statusId = StatusEffectArmorState.StatusId, count = 5},
                },
            };

            CardEffectDataBuilder enrageAllII = new CardEffectDataBuilder
            {
                EffectStateType = typeof(CardEffectAddStatusEffect),
                TargetMode = TargetMode.Room,
                TargetTeamType = Team.Type.Monsters,
                ParamStatusEffects =
                {
                    new StatusEffectStackData {statusId = StatusEffectBuffState.StatusId, count = 3}
                }
            };
            CharacterTriggerDataBuilder enrageII = new CharacterTriggerDataBuilder
            {
                TriggerID = "EnragingII",
                Trigger = CharacterTriggerData.Trigger.OnHit,
                Description = "Apply [rage] [effect0.status0.power] to friendly units.",
                EffectBuilders = { enrageAllII },
            };
            CardUpgradeDataBuilder enragingII = new CardUpgradeDataBuilder
            {
                UpgradeID = "EnragingII",
                UpgradeTitle = "Enraging II",
                TriggerUpgradeBuilders = { enrageII },
                StatusEffectUpgrades =
                {
                    new StatusEffectStackData {statusId = StatusEffectArmorState.StatusId, count = 10},
                },
            };

            CardEffectDataBuilder enrageAllIII = new CardEffectDataBuilder
            {
                EffectStateType = typeof(CardEffectAddStatusEffect),
                TargetMode = TargetMode.Room,
                TargetTeamType = Team.Type.Monsters,
                ParamStatusEffects =
                {
                    new StatusEffectStackData {statusId = StatusEffectBuffState.StatusId, count = 5}
                }
            };
            CharacterTriggerDataBuilder enrageIII = new CharacterTriggerDataBuilder
            {
                TriggerID = "EnragingIII",
                Trigger = CharacterTriggerData.Trigger.OnHit,
                Description = "Apply [rage] [effect0.status0.power] to friendly units.",
                EffectBuilders = { enrageAllIII },
            };
            CardUpgradeDataBuilder enragingIII = new CardUpgradeDataBuilder
            {
                UpgradeID = "EnragingIII",
                UpgradeTitle = "Enraging III",
                TriggerUpgradeBuilders = { enrageIII },
                StatusEffectUpgrades =
                {
                    new StatusEffectStackData {statusId = StatusEffectArmorState.StatusId, count = 15},
                },
            };

            List<CardUpgradeDataBuilder> enraging = new List<CardUpgradeDataBuilder> { enragingI, enragingII, enragingIII };


            CardUpgradeDataBuilder lightArmorI = new CardUpgradeDataBuilder
            {
                UpgradeID = "LightAmrorI",
                UpgradeTitle = "Light Armor I",
                StatusEffectUpgrades = {
                    new StatusEffectStackData {statusId = StatusEffectArmorState.StatusId, count = 5},
                    new StatusEffectStackData {statusId = StatusEffectMultistrikeState.StatusId, count = 1},
                    new StatusEffectStackData {statusId = StatusEffectFragileState.StatusId, count = 1},
                }
            };

            CardUpgradeDataBuilder lightArmorII = new CardUpgradeDataBuilder
            {
                UpgradeID = "LightAmrorII",
                UpgradeTitle = "Light Armor II",
                StatusEffectUpgrades = {
                    new StatusEffectStackData {statusId = StatusEffectArmorState.StatusId, count = 10},
                    new StatusEffectStackData {statusId = StatusEffectMultistrikeState.StatusId, count = 2},
                    new StatusEffectStackData {statusId = StatusEffectFragileState.StatusId, count = 1},
                }
            };

            CardUpgradeDataBuilder lightArmorIII = new CardUpgradeDataBuilder
            {
                UpgradeID = "LightAmrorIII",
                UpgradeTitle = "Light Armor III",
                StatusEffectUpgrades = {
                    new StatusEffectStackData {statusId = StatusEffectArmorState.StatusId, count = 15},
                    new StatusEffectStackData {statusId = StatusEffectMultistrikeState.StatusId, count = 3},
                    new StatusEffectStackData {statusId = StatusEffectFragileState.StatusId, count = 1},
                }
            };

            List<CardUpgradeDataBuilder> lightArmor = new List<CardUpgradeDataBuilder> { lightArmorI, lightArmorII, lightArmorIII };

            CardUpgradeTreeData treeData = new CardUpgradeTreeDataBuilder
            {
                UpgradeTrees = { shielded, enraging, lightArmor },
            }.Build();


            CardData apexImpChampion = Championify.TurnCharacterIntoChampion(apexImp, apexImpCard, treeData);

            MutatorDataBuilder ApexOfAllImps = new MutatorDataBuilder
            {
                MutatorID = "Mutator_ApexOfAllImps",
                Name = "Apex of all Imps",
                Description = "Apex imps time to shine. Start with Apex Imp.",
                EffectBuilders = new List<RelicEffectDataBuilder>
                {
                    new RelicEffectDataBuilder
                    {
                        RelicEffectClassType = typeof(RelicEffectPurgeChampion),
                    },
                    new RelicEffectDataBuilder
                    {
                        RelicEffectClassType = typeof(RelicEffectAddCardsStartOfRun),
                        ParamBool = true,
                        ParamCardPoolBuilder = new CardPoolBuilder
                        {
                            CardPoolID = "ApexImpChampionOnly",
                            CardIDs = {apexImpChampion.GetID() },
                        },
                    }
                },
                BoonValue = 5,
                Tags = new List<string> { "champion" },
                IconPath = "PRT_ApexImp_01.png",
            };

            ApexOfAllImps.BuildAndRegister();
        }
    }
}
