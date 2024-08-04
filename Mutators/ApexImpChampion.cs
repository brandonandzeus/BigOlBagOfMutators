using BigOlBagOfMutators.CustomTriggers;
using BigOlBagOfMutators.Utilities;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Trainworks.BuildersV2;
using Trainworks.ConstantsV2;
using Trainworks.Managers;
using UnityEngine;

namespace BigOlBagOfMutators.Mutators
{
    class ApexImpChampion
    {
        public static void Make()
        {
            CharacterData apexImp = CustomCharacterManager.GetCharacterDataByID(VanillaCharacterIDs.ApexImp);
            CardData apexImpCard = CustomCardManager.GetCardDataByID(VanillaCardIDs.ApexImp);

            CardData batteringRam = CustomCardManager.GetCardDataByID(VanillaCardIDs.BatteringRam);
            var batteringRamEffect = batteringRam.GetEffects()[0];

            CardEffectDataBuilder damageByArmorI = new CardEffectDataBuilder
            {
                EffectStateType = typeof(CardEffectShieldDamage),
                TargetMode = TargetMode.LastAttackedCharacter, // Hack the trigger sets OverrideTargetCharacter to the LastAttackerCharacter
                TargetTeamType = Team.Type.Heroes,
                ParamMultiplier = 0.5f,
                AppliedVFX = batteringRamEffect.GetAppliedVFX(),
            };
            CharacterTriggerDataBuilder shieldedIHit = new CharacterTriggerDataBuilder
            {
                TriggerID = "ProtectiveI",
                Trigger = OnOtherMonsterHit.ImpTrigger,
                AdditionalTextOnTrigger = "Counter!",
                AdditionalTextOnTriggerKey = "ProtectiveI_CharacterTriggerData_AdditionalTextOnTriggerKey",
                Description = "Deal damage equal to half of the unit's [armor].",
                EffectBuilders = { damageByArmorI },
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

            CardEffectDataBuilder damageByArmorII = new CardEffectDataBuilder
            {
                EffectStateType = typeof(CardEffectShieldDamage),
                TargetMode = TargetMode.LastAttackedCharacter, // Hack the trigger sets OverrideTargetCharacter to the LastAttackerCharacter
                TargetTeamType = Team.Type.Heroes,
                ParamMultiplier = 1f,
                AppliedVFX = batteringRamEffect.GetAppliedVFX(),
            };
            CharacterTriggerDataBuilder shieldedIIHit = new CharacterTriggerDataBuilder
            {
                TriggerID = "ProtectiveII",
                Trigger = OnOtherMonsterHit.ImpTrigger,
                AdditionalTextOnTrigger = "Counter!",
                AdditionalTextOnTriggerKey = "ProtectiveII_CharacterTriggerData_AdditionalTextOnTriggerKey",
                Description = "Deal damage equal to the unit's [armor].",
                EffectBuilders = { damageByArmorII },
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

            CardEffectDataBuilder damageByArmorIII = new CardEffectDataBuilder
            {
                EffectStateType = typeof(CardEffectShieldDamage),
                TargetMode = TargetMode.LastAttackedCharacter, // Hack the trigger sets OverrideTargetCharacter to the LastAttackerCharacter
                TargetTeamType = Team.Type.Heroes,
                ParamMultiplier = 2f,
                AppliedVFX = batteringRamEffect.GetAppliedVFX(),
            };
            CharacterTriggerDataBuilder shieldedIIIHit = new CharacterTriggerDataBuilder
            {
                TriggerID = "ProtectiveIII",
                Trigger = OnOtherMonsterHit.ImpTrigger,
                AdditionalTextOnTrigger = "Counter!",
                AdditionalTextOnTriggerKey = "ProtectiveIII_CharacterTriggerData_AdditionalTextOnTriggerKey",
                Description = "Deal damage equal to two times the unit's [armor].",
                EffectBuilders = { damageByArmorIII },
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
                ParamSubtype = VanillaSubtypeIDs.Imp,
                ParamStatusEffects =
                {
                    new StatusEffectStackData {statusId = StatusEffectBuffState.StatusId, count = 3}
                }
            };
            CharacterTriggerDataBuilder enrageI = new CharacterTriggerDataBuilder
            {
                TriggerID = "RousingI",
                Trigger = CharacterTriggerData.Trigger.OnHit,
                Description = "Apply [rage] [effect0.status0.power] to all imps in the room.",
                EffectBuilders = { enrageAllI },
            };
            CardUpgradeDataBuilder enragingI = new CardUpgradeDataBuilder
            {
                UpgradeID = "RousingI",
                UpgradeTitle = "Rousing I",
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
                ParamSubtype = VanillaSubtypeIDs.Imp,
                ParamStatusEffects =
                {
                    new StatusEffectStackData {statusId = StatusEffectBuffState.StatusId, count = 5}
                }
            };
            CharacterTriggerDataBuilder enrageII = new CharacterTriggerDataBuilder
            {
                TriggerID = "RousingII",
                Trigger = CharacterTriggerData.Trigger.OnHit,
                Description = "Apply [rage] [effect0.status0.power] to all imps in the room.",
                EffectBuilders = { enrageAllII },
            };
            CardUpgradeDataBuilder enragingII = new CardUpgradeDataBuilder
            {
                UpgradeID = "RousingII",
                UpgradeTitle = "Rousing II",
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
                ParamSubtype = VanillaSubtypeIDs.Imp,
                ParamStatusEffects =
                {
                    new StatusEffectStackData {statusId = StatusEffectBuffState.StatusId, count = 8}
                }
            };
            CharacterTriggerDataBuilder enrageIII = new CharacterTriggerDataBuilder
            {
                TriggerID = "RousingIII",
                Trigger = CharacterTriggerData.Trigger.OnHit,
                Description = "Apply [rage] [effect0.status0.power] to all imps in the room.",
                EffectBuilders = { enrageAllIII },
            };
            CardUpgradeDataBuilder enragingIII = new CardUpgradeDataBuilder
            {
                UpgradeID = "RousingIII",
                UpgradeTitle = "Rousing III",
                TriggerUpgradeBuilders = { enrageIII },
                StatusEffectUpgrades =
                {
                    new StatusEffectStackData {statusId = StatusEffectArmorState.StatusId, count = 15},
                },
            };

            List<CardUpgradeDataBuilder> enraging = new List<CardUpgradeDataBuilder> { enragingI, enragingII, enragingIII };


            CardUpgradeDataBuilder lightArmorI = new CardUpgradeDataBuilder
            {
                UpgradeID = "LightArmorI",
                UpgradeTitle = "Light Armor I",
                StatusEffectUpgrades = {
                    new StatusEffectStackData {statusId = StatusEffectArmorState.StatusId, count = 5},
                    new StatusEffectStackData {statusId = StatusEffectMultistrikeState.StatusId, count = 1},
                    new StatusEffectStackData {statusId = StatusEffectFragileState.StatusId, count = 1},
                }
            };

            CardUpgradeDataBuilder lightArmorII = new CardUpgradeDataBuilder
            {
                UpgradeID = "LightArmorII",
                UpgradeTitle = "Light Armor II",
                StatusEffectUpgrades = {
                    new StatusEffectStackData {statusId = StatusEffectArmorState.StatusId, count = 10},
                    new StatusEffectStackData {statusId = StatusEffectMultistrikeState.StatusId, count = 2},
                    new StatusEffectStackData {statusId = StatusEffectFragileState.StatusId, count = 1},
                }
            };

            CardUpgradeDataBuilder lightArmorIII = new CardUpgradeDataBuilder
            {
                UpgradeID = "LightArmorIII",
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
                Description = "Replaces your champion with <b>Champion<b> Apex Imp.",
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
                            Cards = { apexImpChampion },
                        },
                    }
                },
                BoonValue = 3,
                Tags = new List<string> { "champion" },
                IconPath = "Assets/MTR_ApexOfAllImps.png",
                RequiredDLC = ShinyShoe.DLC.Hellforged,
            };

            ApexOfAllImps.BuildAndRegister();

            BuilderUtils.ImportStandardLocalization("Trigger_OnOtherImpHit_CardText", "Imp Counter");
            BuilderUtils.ImportStandardLocalization("Trigger_OnOtherImpHit_CharacterText", "Imp Counter");
            BuilderUtils.ImportStandardLocalization("Trigger_OnOtherImpHit_TooltipText", "Triggers when another [imp] is attacked.");
            CustomCharacterManager.AddCustomTriggerIcon(OnOtherMonsterHit.ImpTrigger, "Assets/trigger_impcounter.png", "Assets/trigger_impcounter_tooltip.png");
        }
    }

    public class CardEffectShieldDamage : CardEffectBase
    {
        public override bool TestEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
        {
            int intInRange = cardEffectState.GetIntInRange();
            bool flag = !cardEffectState.GetUseIntRange() || cardEffectState.GetParamMaxInt() > 0;
            bool flag2 = true;
            if (cardEffectState.GetTargetMode() == TargetMode.DropTargetCharacter)
            {
                flag2 = cardEffectParams.targets.Count > 0;
            }
            return intInRange >= 0 && flag2 && flag;
        }

        public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
        {
            int damageAmount = GetDamageAmount(cardEffectState, cardEffectParams);
            int soundGateId = SoundManager.INVALID_SOUND_GATE;
            if (cardEffectState.GetTargetMode() == TargetMode.Room)
            {
                soundGateId = cardEffectParams.combatManager.IgnoreDuplicateSounds(ignoreGameSpeed: true);
            }
            for (int i = 0; i < cardEffectParams.targets.Count; i++)
            {
                CharacterState target = cardEffectParams.targets[i];
                yield return cardEffectParams.combatManager.ApplyDamageToTarget(damageAmount, target, new CombatManager.ApplyDamageToTargetParameters
                {
                    playedCard = cardEffectParams.playedCard,
                    finalEffectInSequence = cardEffectParams.finalEffectInSequence,
                    relicState = cardEffectParams.sourceRelic,
                    selfTarget = cardEffectParams.selfTarget,
                    vfxAtLoc = cardEffectState.GetAppliedVFX(),
                    showDamageVfx = cardEffectParams.allowPlayingDamageVfx
                });
            }
            cardEffectParams.combatManager.ReleaseIgnoreDuplicateCuesHandle(soundGateId);
        }

        private int GetDamageAmount(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
        {
            int armor = cardEffectParams.selfTarget.GetStatusEffectStacks(VanillaStatusEffectIDs.Armor);
            return Mathf.FloorToInt(cardEffectState.GetParamMultiplier() * armor);
        }
    }

}
