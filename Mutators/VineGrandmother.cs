using BigOlBagOfMutators.CustomTriggers;
using BigOlBagOfMutators.RoomModifiers;
using BigOlBagOfMutators.Utilities;
using HarmonyLib;
using ShinyShoe.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Trainworks.BuildersV2;
using Trainworks.ConstantsV2;
using Trainworks.Custom.CardEffects;
using Trainworks.Enums.MTTriggers;
using Trainworks.Managers;
using Trainworks.Utilities;
using UnityEngine;


namespace BigOlBagOfMutators.Mutators
{
    public class VineGrandmother
    {
        public static void Make()
        {
            BuilderUtils.ImportStandardLocalization("Trigger_OnSting_CardText", "Stingcast");
            BuilderUtils.ImportStandardLocalization("Trigger_OnSting_CharacterText", "Stingcast");
            BuilderUtils.ImportStandardLocalization("Trigger_OnSting_TooltipText", "Triggers when a <b>Sting</b> spell is played.");
            CustomCharacterManager.AddCustomTriggerIcon(OnSting.Trigger, "Assets/trigger_sting.png", "Assets/trigger_sting_tooltip.png");

            BuilderUtils.ImportStandardLocalization("RoomModifierApplyTempUpgrade", "Sting Frenzy");
            CustomCharacterManager.AddCustomRoomModifierIcon(typeof(RoomModifierApplyTempUpgrade), "Assets/sting_dissolve_icon.png", "Assets/sting_dissolve_tooltip_icon.png");

            BuilderUtils.ImportStandardLocalization("RoomStateMaxHPOnSpawnModifier", "Implantation");
            CustomCharacterManager.AddCustomRoomModifierIcon(typeof(RoomStateMaxHPOnSpawnModifier), "Assets/engraft_icon.png", "Assets/engraft_tooltip_icon.png");


            CharacterData vinemother = CustomCharacterManager.GetCharacterDataByID(VanillaCharacterIDs.Vinemother);
            CardData vinemotherCard = CustomCardManager.GetCardDataByID(VanillaCardIDs.Vinemother);

            var frenziedChanneler = new CharacterDataBuilder
            {
                CharacterID = "FrenziedChanneler",
                Name = "Frenzied Channeler",
                AttackDamage = 0,
                Health = 2,
                Size = 1,
                StartingStatusEffects =
                {
                    new StatusEffectStackData { statusId = VanillaStatusEffectIDs.Multistrike, count = 1 }
                },
                TriggerBuilders =
                {
                    new CharacterTriggerDataBuilder
                    {
                        TriggerID = "FrenziedChannelerEnchant",
                        Trigger = CharacterTriggerData.Trigger.AfterSpawnEnchant,
                        Description = "Grant [multistrike] [effect0.status0.power]",
                        EffectBuilders =
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateType = typeof(CardEffectEnchant),
                                TargetMode = TargetMode.Room,
                                TargetTeamType = Team.Type.Monsters,
                                ParamStatusEffects =
                                {
                                    new StatusEffectStackData { statusId = VanillaStatusEffectIDs.Multistrike, count = 1 }
                                }
                            }
                        }
                    }
                },
                BundleLoadingInfo = new BundleAssetLoadingInfo
                {
                    BaseName = "PLR_FrenziedChanneler",
                    FilePath = "Assets/bbom",
                    SpriteName = "Assets/PLR_FrenziedChanneler.png",
                    SpineAnimationDict = new Dictionary<CharacterUI.Anim, string>
                    {
                        { CharacterUI.Anim.Idle, "Assets/PLR_FrenziedChanneler_Idle.prefab" },
                        { CharacterUI.Anim.Attack, "Assets/PLR_FrenziedChanneler_Attack.prefab" },
                        { CharacterUI.Anim.HitReact, "Assets/PLR_FrenziedChanneler_HitReact.prefab" },
                    },
                    AssetType = Trainworks.Builders.AssetRefBuilder.AssetTypeEnum.Character
                },
            }.BuildAndRegister();
            var edgePrior = CustomCharacterManager.GetCharacterDataByID(VanillaCharacterIDs.EdgePrior);
            var shardChanneler = CustomCharacterManager.GetCharacterDataByID(VanillaCharacterIDs.ShardChanneler);

            // Frenzied
            // I)    +10 Attack +10 Health | Sting spells in hand on the floor gain Consume. When a sting spell is played all friendly units gain +2 attack.
            // II)   +10 Attack +30 Health | Summon: Spawn Frenzied Channeler. Sting spells in hand on the floor gain Consume. When a sting spell is played all friendly units gain +3 attack.
            // III)  +25 Attack +60 Health | Summon: Spawn Frenzied Channeler. Sting spells in hand on the floor gain Consume. When a sting spell is played all friendly units gain +4 attack.

            var applyConsumeToSting = new CardUpgradeDataBuilder
            {
                UpgradeID = "FrenzyApplyConsumeToSting",
                TraitDataUpgradeBuilders =
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateType = typeof(CardTraitExhaustState),
                    }
                },
                FiltersBuilders =
                {
                    new CardUpgradeMaskDataBuilder
                    {
                        CardUpgradeMaskID = "ConsumeStingSpells",
                        CardType = CardType.Spell,
                        AllowedCardPools =
                        {
                            CustomCardPoolManager.GetVanillaCardPool(VanillaCardPoolIDs.StingOnlyPool)
                        }
                    }
                }
            }.Build();

            var frenzyI = new CardUpgradeDataBuilder
            {
                UpgradeID = "VinemotherFrenzyI",
                UpgradeTitle = "Frenzy",
                BonusDamage = 10,
                BonusHP = 10,
                TriggerUpgradeBuilders =
                {
                    new CharacterTriggerDataBuilder
                    {
                        TriggerID = "FrenzyITrigger",
                        Trigger = OnSting.Trigger,
                        Description = "Apply +[effect0.upgrade.bonusdamage][attack] to friendly units.",
                        EffectBuilders =
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateType = typeof(CardEffectAddTempCardUpgradeToUnits),
                                TargetMode = TargetMode.Room,
                                TargetTeamType = Team.Type.Monsters,
                                ParamCardUpgradeDataBuilder = new CardUpgradeDataBuilder
                                {
                                    UpgradeID = "FrenzyIUpgrade",
                                    BonusDamage = 2,
                                }
                            }
                        }
                    }
                },
                RoomModifierUpgradeBuilders =
                {
                    new RoomModifierDataBuilder
                    {
                        RoomModifierID = "FrenzyIStingConsume",
                        RoomModifierClassType = typeof(RoomModifierApplyTempUpgrade),
                        Description = "<b>Sting</b> spells have <b>Consume</b> on this floor.",
                        ParamCardUpgradeData = applyConsumeToSting,
                        ParamInt = 1,
                    }
                }
            };

            var frenzyII = new CardUpgradeDataBuilder
            {
                UpgradeID = "VinemotherFrenzyII",
                UpgradeTitle = "Frenzy II",
                BonusDamage = 10,
                BonusHP = 30,
                TriggerUpgradeBuilders =
                {
                    new CharacterTriggerDataBuilder
                    {
                        TriggerID = "FrenzyIISpawn",
                        Trigger = CharacterTriggerData.Trigger.OnSpawn,
                        Description = "Summon 1 <b>Frenzied Channeler</b> with +[effect0.upgrade.bonusdamage][attack] and +[effect0.upgrade.bonushp][health].",
                        EffectBuilders =
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateType = typeof(CardEffectSpawnMonsterAnywhere),
                                TargetMode = TargetMode.Room,
                                TargetTeamType = Team.Type.None,
                                ParamCharacterData = frenziedChanneler,
                                ParamCardUpgradeDataBuilder = new CardUpgradeDataBuilder
                                {
                                    UpgradeID = "FrenzyIIBonusChannelerUpgrade",
                                    BonusDamage = 10,
                                    BonusHP = 5,
                                },
                                ParamInt = (int)SpawnMode.BackSlot,
                            }
                        }
                    },
                    new CharacterTriggerDataBuilder
                    {
                        TriggerID = "FrenzyIITrigger",
                        Trigger = OnSting.Trigger,
                        Description = "Apply +[effect0.upgrade.bonusdamage][attack] to friendly units.",
                        EffectBuilders =
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateType = typeof(CardEffectAddTempCardUpgradeToUnits),
                                TargetMode = TargetMode.Room,
                                TargetTeamType = Team.Type.Monsters,
                                ParamCardUpgradeDataBuilder = new CardUpgradeDataBuilder
                                {
                                    UpgradeID = "FrenzyIIUpgrade",
                                    BonusDamage = 3,
                                }
                            }
                        }
                    }
                },
                RoomModifierUpgradeBuilders =
                {
                    new RoomModifierDataBuilder
                    {
                        RoomModifierID = "FrenzyIIStingConsume",
                        RoomModifierClassType = typeof(RoomModifierApplyTempUpgrade),
                        Description = "<b>Sting</b> spells have <b>Consume</b> on this floor.",
                        ParamCardUpgradeData = applyConsumeToSting,
                        ParamInt = 1,
                    }
                }
            };

            var frenzyIII = new CardUpgradeDataBuilder
            {
                UpgradeID = "VinemotherFrenzyIII",
                UpgradeTitle = "Frenzy III",
                BonusDamage = 25,
                BonusHP = 60,
                TriggerUpgradeBuilders =
                {
                    new CharacterTriggerDataBuilder
                    {
                        TriggerID = "FrenzyIIISpawn",
                        Trigger = CharacterTriggerData.Trigger.OnSpawn,
                        Description = "Summon 1 <b>Frenzied Channeler</b> with +[effect0.upgrade.bonusdamage][attack] and +[effect0.upgrade.bonushp][health].",
                        EffectBuilders =
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateType = typeof(CardEffectSpawnMonsterAnywhere),
                                TargetMode = TargetMode.Room,
                                TargetTeamType = Team.Type.None,
                                ParamCharacterData = frenziedChanneler,
                                ParamCardUpgradeDataBuilder = new CardUpgradeDataBuilder
                                {
                                    UpgradeID = "FrenzyIIIBonusChannelerUpgrade",
                                    BonusDamage = 20,
                                    BonusHP = 10,
                                },
                                ParamInt = (int)SpawnMode.BackSlot,
                            }
                        }
                    },
                    new CharacterTriggerDataBuilder
                    {
                        TriggerID = "FrenzyIIITrigger",
                        Trigger = OnSting.Trigger,
                        Description = "Apply +[effect0.upgrade.bonusdamage][attack] to friendly units.",
                        EffectBuilders =
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateType = typeof(CardEffectAddTempCardUpgradeToUnits),
                                TargetMode = TargetMode.Room,
                                TargetTeamType = Team.Type.Monsters,
                                ParamCardUpgradeDataBuilder = new CardUpgradeDataBuilder
                                {
                                    UpgradeID = "FrenzyIIIUpgrade",
                                    BonusDamage = 4,
                                }
                            }
                        }
                    }
                },
                RoomModifierUpgradeBuilders =
                {
                    new RoomModifierDataBuilder
                    {
                        RoomModifierID = "FrenzyIIIStingConsume",
                        RoomModifierClassType = typeof(RoomModifierApplyTempUpgrade),
                        Description = "<b>Sting</b> spells have <b>Consume</b> on this floor.",
                        ParamCardUpgradeData = applyConsumeToSting,
                        ParamInt = 1,
                    }
                }
            };

            List<CardUpgradeDataBuilder> frenzied = new List<CardUpgradeDataBuilder> { frenzyI, frenzyII, frenzyIII };

            // Ensnaring
            // I)    +0 Attack +20 Health  | Incant: if played card was "Sting" gain Spikes 2. Revenge: Apply Rooted 1 to the attacking enemy.
            // II)   +0 Attack +50 Health  | Summon: Spawn Shard Channeler. if played card was "Sting" gain Spikes 3. Revenge: Apply Rooted 1 to the attacking enemy.
            // III)  +0 Attack +110 Health | Summon: Spawn Shard Channeler. if played card was "Sting" gain Spikes 5. Revenge: Apply Rooted 1 to the attacking enemy.

            var ensnaringI = new CardUpgradeDataBuilder
            {
                UpgradeID = "VinemotherEnsnaringI",
                UpgradeTitle = "Ensnaring",
                BonusDamage = 0,
                BonusHP = 20,
                TriggerUpgradeBuilders =
                {
                    new CharacterTriggerDataBuilder
                    {
                        TriggerID = "EnsnaringITrigger",
                        Trigger = OnSting.Trigger,
                        Description = "Gain [spikes] [effect0.status0.power].",
                        EffectBuilders =
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateType = typeof(CardEffectAddStatusEffect),
                                TargetMode = TargetMode.Self,
                                TargetTeamType = Team.Type.Monsters,
                                ParamStatusEffects =
                                {
                                    new StatusEffectStackData {statusId = VanillaStatusEffectIDs.Spikes, count = 2}
                                }
                            }
                        }
                    },
                    new CharacterTriggerDataBuilder
                    {
                        TriggerID = "EnsnaringITriggerRevenge",
                        Trigger = CharacterTriggerData.Trigger.OnHit,
                        Description = "Apply [rooted] [effect0.status0.power] to the attacking unit.",
                        EffectBuilders =
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateType = typeof(CardEffectAddStatusEffect),
                                TargetMode = TargetMode.LastAttackerCharacter,
                                TargetTeamType = Team.Type.Heroes,
                                ParamStatusEffects =
                                {
                                    new StatusEffectStackData {statusId = VanillaStatusEffectIDs.Rooted, count = 1 }
                                }
                            }
                        }
                    }
                }
            };

            var ensnaringII = new CardUpgradeDataBuilder
            {
                UpgradeID = "VinemotherEnsnaringII",
                UpgradeTitle = "Ensnaring II",
                BonusDamage = 0,
                BonusHP = 50,
                TriggerUpgradeBuilders =
                {
                    new CharacterTriggerDataBuilder
                    {
                        TriggerID = "EnsnaringIISpawn",
                        Trigger = CharacterTriggerData.Trigger.OnSpawn,
                        Description = "Summon 1 <b>Shard Channeler</b>.",
                        EffectBuilders =
                        {
                            new CardEffectDataBuilder
                            {   
                                EffectStateType = typeof(CardEffectSpawnMonsterAnywhere),
                                TargetMode = TargetMode.Room,
                                TargetTeamType = Team.Type.None,
                                ParamInt = (int)SpawnMode.BackSlot,
                                ParamCharacterData = shardChanneler,
                            }
                        }
                    },
                    new CharacterTriggerDataBuilder
                    {
                        TriggerID = "EnsnaringIITrigger",
                        Trigger = OnSting.Trigger,
                        Description = "Gain [spikes] [effect0.status0.power].",
                        EffectBuilders =
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateType = typeof(CardEffectAddStatusEffect),
                                TargetMode = TargetMode.Self,
                                TargetTeamType = Team.Type.Monsters,
                                ParamStatusEffects =
                                {
                                    new StatusEffectStackData {statusId = VanillaStatusEffectIDs.Spikes, count = 3}
                                }
                            }
                        }
                    },
                    new CharacterTriggerDataBuilder
                    {
                        TriggerID = "EnsnaringIITriggerRevenge",
                        Trigger = CharacterTriggerData.Trigger.OnHit,
                        Description = "Apply [rooted] [effect0.status0.power] to the attacking unit.",
                        EffectBuilders =
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateType = typeof(CardEffectAddStatusEffect),
                                TargetMode = TargetMode.LastAttackerCharacter,
                                TargetTeamType = Team.Type.Heroes,
                                ParamStatusEffects =
                                {
                                    new StatusEffectStackData {statusId = VanillaStatusEffectIDs.Rooted, count = 1 }
                                }
                            }
                        }
                    }
                }
            };

            var ensnaringIII = new CardUpgradeDataBuilder
            {
                UpgradeID = "VinemotherEnsnaringIII",
                UpgradeTitle = "Ensnaring III",
                BonusDamage = 0,
                BonusHP = 110,
                TriggerUpgradeBuilders =
                {
                    new CharacterTriggerDataBuilder
                    {
                        TriggerID = "EnsnaringIIISpawn",
                        Trigger = CharacterTriggerData.Trigger.OnSpawn,
                        Description = "Summon 1 <b>Shard Channeler</b>.",
                        EffectBuilders =
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateType = typeof(CardEffectSpawnMonsterAnywhere),
                                TargetMode = TargetMode.Room,
                                TargetTeamType = Team.Type.None,
                                ParamInt = (int)SpawnMode.BackSlot,
                                ParamCharacterData = shardChanneler,
                            }
                        }
                    },
                    new CharacterTriggerDataBuilder
                    {
                        TriggerID = "EnsnaringIIITrigger",
                        Trigger = OnSting.Trigger,
                        Description = "Gain [spikes] [effect0.status0.power].",
                        EffectBuilders =
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateType = typeof(CardEffectAddStatusEffect),
                                TargetMode = TargetMode.Self,
                                TargetTeamType = Team.Type.Monsters,
                                ParamStatusEffects =
                                {
                                    new StatusEffectStackData {statusId = VanillaStatusEffectIDs.Spikes, count = 5}
                                }
                            }
                        }
                    },
                    new CharacterTriggerDataBuilder
                    {
                        TriggerID = "EnsnaringIIITriggerRevenge",
                        Trigger = CharacterTriggerData.Trigger.OnHit,
                        Description = "Apply [rooted] [effect0.status0.power] to the attacking unit.",
                        EffectBuilders =
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateType = typeof(CardEffectAddStatusEffect),
                                TargetMode = TargetMode.LastAttackerCharacter,
                                TargetTeamType = Team.Type.Heroes,
                                ParamStatusEffects =
                                {
                                    new StatusEffectStackData {statusId = VanillaStatusEffectIDs.Rooted, count = 1 }
                                }
                            }
                        }
                    }
                }
            };


            List<CardUpgradeDataBuilder> ensnaring = new List<CardUpgradeDataBuilder> { ensnaringI, ensnaringII, ensnaringIII };


            // Grafting
            // I)     +5 Attack +15 Health | Sweep Units summoned gain 10 max health, Harvest: Apply Regen 1 to friendly units.
            // II)   +15 Attack +35 Health | Sweep Summon: Spawn Edge Prior. Units summoned gain 20 max health, Harvest: Apply Regen 1 to friendly units.
            // III)  +25 Attack +85 Health | Sweep Summon: Spawn Edge Prior. Units summoned gain 40 max health, Harvest: Apply Regen 2 to friendly units.

            var graftingI = new CardUpgradeDataBuilder
            {
                UpgradeID = "VinemotherGraftingI",
                UpgradeTitle = "Grafting",
                BonusDamage = 5,
                BonusHP = 15,
                StatusEffectUpgrades =
                {
                    new StatusEffectStackData {statusId = VanillaStatusEffectIDs.Sweep, count = 1},
                },
                TriggerUpgradeBuilders =
                {
                    new CharacterTriggerDataBuilder
                    {
                        TriggerID = "GraftingITrigger",
                        Trigger = CharacterTriggerData.Trigger.OnAnyHeroDeathOnFloor,
                        Description = "Apply [regen] [effect0.status0.power] to friendly units.",
                        EffectBuilders =
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateType = typeof(CardEffectAddStatusEffect),
                                TargetMode = TargetMode.Room,
                                TargetTeamType = Team.Type.Monsters,
                                ParamStatusEffects =
                                {
                                    new StatusEffectStackData {statusId = VanillaStatusEffectIDs.Regen, count = 1}
                                }
                            }
                        }
                    }
                },
                RoomModifierUpgradeBuilders =
                {
                    new RoomModifierDataBuilder
                    {
                        RoomModifierID = "GraftingIRoomModifier",
                        RoomModifierClassType = typeof(RoomStateMaxHPOnSpawnModifier),
                        Description = "Friendly units spawned on this floor gain +[paramint][health]",
                        ParamInt = 10,
                    }
                }
            };

            var graftingII = new CardUpgradeDataBuilder
            {
                UpgradeID = "VinemotherGraftingII",
                UpgradeTitle = "Grafting II",
                BonusDamage = 15,
                BonusHP = 35,
                StatusEffectUpgrades =
                {
                    new StatusEffectStackData {statusId = VanillaStatusEffectIDs.Sweep, count = 1},
                },
                TriggerUpgradeBuilders =
                {
                    new CharacterTriggerDataBuilder
                    {
                        TriggerID = "GraftingIISpawn",
                        Trigger = CharacterTriggerData.Trigger.OnSpawn,
                        Description = "Summon 1 <b>Edge Prior</b>.",
                        EffectBuilders =
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateType = typeof(CardEffectSpawnMonsterAnywhere),
                                TargetMode = TargetMode.Room,
                                TargetTeamType = Team.Type.None,
                                ParamInt = (int)SpawnMode.BackSlot,
                                ParamCharacterData = edgePrior,
                            }
                        }
                    },
                    new CharacterTriggerDataBuilder
                    {
                        TriggerID = "GraftingIITrigger",
                        Trigger = CharacterTriggerData.Trigger.OnAnyHeroDeathOnFloor,
                        Description = "Apply [regen] [effect0.status0.power] to friendly units.",
                        EffectBuilders =
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateType = typeof(CardEffectAddStatusEffect),
                                TargetMode = TargetMode.Room,
                                TargetTeamType = Team.Type.Monsters,
                                ParamStatusEffects =
                                {
                                    new StatusEffectStackData {statusId = VanillaStatusEffectIDs.Regen, count = 1}
                                }
                            }
                        }
                    }
                },
                RoomModifierUpgradeBuilders =
                {
                    new RoomModifierDataBuilder
                    {
                        RoomModifierID = "GraftingIIRoomModifier",
                        RoomModifierClassType = typeof(RoomStateMaxHPOnSpawnModifier),
                        Description = "Friendly units spawned on this floor gain +[paramint][health]",
                        ParamInt = 20,
                    }
                }
            };

            var graftingIII = new CardUpgradeDataBuilder
            {
                UpgradeID = "VinemotherGraftingIII",
                UpgradeTitle = "Grafting III",
                BonusDamage = 25,
                BonusHP = 85,
                StatusEffectUpgrades =
                {
                    new StatusEffectStackData {statusId = VanillaStatusEffectIDs.Sweep, count = 1},
                },
                TriggerUpgradeBuilders =
                {
                    new CharacterTriggerDataBuilder
                    {
                        TriggerID = "GraftingIIISpawn",
                        Trigger = CharacterTriggerData.Trigger.OnSpawn,
                        Description = "Summon 1 <b>Edge Prior</b>.",
                        EffectBuilders =
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateType = typeof(CardEffectSpawnMonsterAnywhere),
                                TargetMode = TargetMode.Room,
                                TargetTeamType = Team.Type.None,
                                ParamInt = (int)SpawnMode.BackSlot,
                                ParamCharacterData = edgePrior,
                            }
                        }
                    },
                    new CharacterTriggerDataBuilder
                    {
                        TriggerID = "GraftingIIITrigger",
                        Trigger = CharacterTriggerData.Trigger.OnAnyHeroDeathOnFloor,
                        Description = "Apply [regen] [effect0.status0.power] to friendly units.",
                        EffectBuilders =
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateType = typeof(CardEffectAddStatusEffect),
                                TargetMode = TargetMode.Room,
                                TargetTeamType = Team.Type.Monsters,
                                ParamStatusEffects =
                                {
                                    new StatusEffectStackData {statusId = VanillaStatusEffectIDs.Regen, count = 2}
                                }
                            }
                        }
                    }
                },
                RoomModifierUpgradeBuilders =
                {
                    new RoomModifierDataBuilder
                    {
                        RoomModifierID = "GraftingIIIRoomModifier",
                        RoomModifierClassType = typeof(RoomStateMaxHPOnSpawnModifier),
                        Description = "Friendly units spawned on this floor gain +[paramint][health]",
                        ParamInt = 40,
                    }
                }
            };

            List<CardUpgradeDataBuilder> grafting = new List<CardUpgradeDataBuilder> { graftingI, graftingII, graftingIII };

            CardUpgradeTreeData treeData = new CardUpgradeTreeDataBuilder
            {
                UpgradeTrees = { frenzied, ensnaring, grafting },
            }.Build();


            var BundleLoadingInfo = new BundleAssetLoadingInfo
            {
                BaseName = "PLR_VineGrandmother",
                FilePath = "Assets/bbom",
                SpriteName = "Assets/PLR_VineGrandMother.png",
                SpineAnimationDict = new Dictionary<CharacterUI.Anim, string>
                {
                    { CharacterUI.Anim.Idle, "Assets/PLR_VineGrandMother_Idle.prefab" },
                    { CharacterUI.Anim.Attack, "Assets/PLR_VineGrandMother_Attack.prefab" },
                    { CharacterUI.Anim.HitReact, "Assets/PLR_VineGrandMother_HitReact.prefab" },
                },
                AssetType = Trainworks.Builders.AssetRefBuilder.AssetTypeEnum.Character
            };

            CardData vinemotherChampion = Championify.TurnCharacterIntoChampion(vinemother, vinemotherCard, treeData, 0, 10, 2, BundleLoadingInfo);

            MutatorDataBuilder VineGrandmother = new MutatorDataBuilder
            {
                MutatorID = "Mutator_VineGrandmother",
                Name = "VineGrandmother",
                Description = "Replaces your champion with <b>Champion</b> Vinemother.",
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
                            CardPoolID = "VinemotherChampionOnly",
                            Cards = { vinemotherChampion },
                        },
                    }
                },
                BoonValue = 2,
                Tags = new List<string> { "champion" },
                IconPath = "Assets/MTR_VineGrandmother.png",
            };

            VineGrandmother.BuildAndRegister();
        }
    }

    public sealed class CardEffectSpawnMonsterAnywhere : CardEffectBase
    {
        public override bool CanApplyInPreviewMode => false;

        public override bool CanPlayInEngineRoom => false;

        public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
        {
            RoomState roomState = cardEffectParams.GetSelectedRoom();
            CharacterData monsterData = cardEffectState.GetParamCharacterData();
            CardUpgradeData paramCardUpgradeData = cardEffectState.GetParamCardUpgradeData();
            CharacterState newMonster = null;
            SpawnMode spawnMode = (SpawnMode) cardEffectState.GetParamInt();
            yield return cardEffectParams.monsterManager.CreateMonsterState(monsterData, null, cardEffectParams.selectedRoom, delegate (CharacterState character)
            {
                newMonster = character;
            }, spawnMode, null, null, recruitedUnit: false, isCardless: true);
            if (newMonster != null && paramCardUpgradeData != null)
            {
                CardUpgradeState upgradeState = new CardUpgradeState();
                upgradeState.Setup(paramCardUpgradeData);
                yield return newMonster.ApplyCardUpgrade(upgradeState);
            }
            if (!cardEffectParams.saveManager.PreviewMode)
            {
                yield return cardEffectParams.roomManager.GetRoomUI().CenterCharacters(roomState, skipDelay: true, fromEndOfRoomCombat: false, forceRecenter: true);
            }
        }

        public override bool TestEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
        {
            RoomState selectedRoom = cardEffectParams.GetSelectedRoom();
            if (selectedRoom.GetIsPyreRoom())
            {
                return false;
            }
            if (selectedRoom.GetFirstEmptyMonsterPoint() == null)
            {
                return false;
            }
            return true;
        }
    }

}