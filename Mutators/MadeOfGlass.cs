using System.Collections.Generic;
using Trainworks.BuildersV2;
using Trainworks.ConstantsV2;
using Trainworks.Managers;

namespace BigOlBagOfMutators.Mutators
{
    class MadeOfGlass
    {
        /// <summary>
        /// This method makes and registers the mutator, must be called Make and must be static.
        /// </summary>
        public static void Make()
        {
            StatusEffectStackData fragile1 = new StatusEffectStackData
            {
                statusId = VanillaStatusEffectIDs.Fragile,
                count = 1,
            };
            StatusEffectStackData damageshield1 = new StatusEffectStackData
            {
                statusId = VanillaStatusEffectIDs.DamageShield,
                count = 1,
            };

            CardUpgradeMaskDataBuilder filterUnits = new CardUpgradeMaskDataBuilder
            {
                CardUpgradeMaskDataID = "MadeOfGlassUnits",
                CardType = CardType.Monster,
            };

            CardUpgradeDataBuilder unitsFragileUpgrade = new CardUpgradeDataBuilder
            {
                UpgradeID = "MadeOfGlassFragile",
                StatusEffectUpgrades = { fragile1 },
                FiltersBuilders = {filterUnits},
            };
            CardUpgradeDataBuilder unitsDamageShieldUpgrade = new CardUpgradeDataBuilder
            {
                UpgradeID = "MadeOfGlassDamageShield",
                StatusEffectUpgrades = { damageshield1 },
                FiltersBuilders = {filterUnits},
            };

            RelicEffectDataBuilder unitsFragile = new RelicEffectDataBuilder
            {
                RelicEffectClassType = typeof(RelicEffectAddTempUpgrade),
                ParamSourceTeam = Team.Type.Monsters,
                ParamStatusEffects =
                {
                    // Note this is required, but unused.
                    fragile1
                },
                ParamCardUpgradeDataBuilder = unitsFragileUpgrade,
            };

            RelicEffectDataBuilder unitsDamageShield = new RelicEffectDataBuilder
            {
                RelicEffectClassType = typeof(RelicEffectAddTempUpgrade),
                ParamSourceTeam = Team.Type.Monsters,
                ParamStatusEffects =
                {
                    damageshield1,
                },
                ParamCardUpgradeDataBuilder = unitsDamageShieldUpgrade
            };

            RelicEffectDataBuilder heroesFragile = new RelicEffectDataBuilder
            {
                RelicEffectClassType = typeof(RelicEffectAddStatusEffectOnSpawn),
                ParamSourceTeam = Team.Type.Heroes,
                ParamStatusEffects =
                {
                    fragile1,
                }
            };

            RelicEffectDataBuilder heroesDamageShield = new RelicEffectDataBuilder
            {
                RelicEffectClassType = typeof(RelicEffectAddStatusEffectOnSpawn),
                ParamSourceTeam = Team.Type.Heroes,
                ParamStatusEffects =
                {
                    damageshield1,
                }
            };


            CardUpgradeDataBuilder removePierceUpgrade = new CardUpgradeDataBuilder
            {
                UpgradeID = "MadeOfGlassRemovePierce",
                RemoveTraitUpgrades = { "CardTraitIgnoreArmor" },
                FiltersBuilders = new List<CardUpgradeMaskDataBuilder>(),
            };

            RelicEffectDataBuilder removePiercing = new RelicEffectDataBuilder
            {
                RelicEffectClassType = typeof(RelicEffectAddTempUpgrade),
                ParamSourceTeam = Team.Type.Monsters,
                ParamCardUpgradeDataBuilder = removePierceUpgrade,
            };

            MutatorDataBuilder builder = new MutatorDataBuilder
            {
                MutatorID = "MadeOfGlass",
                Name = "Made of Glass",
                Description = "All units get [fragile] and [damageshield] 1. Bosses have additional [damageshield]. Piercing is removed.",
                Effects = FormBossDamageShieldData(),
                EffectBuilders = { unitsFragile, unitsDamageShield, heroesFragile, heroesDamageShield, removePiercing},
                BoonValue = -3,
                IconPath = "MTR_MadeOfGlass.png",
            };

            builder.BuildAndRegister();
        }

        public static List<RelicEffectData> FormBossDamageShieldData()
        {
            string[] bossSubtypes =
            {
                VanillaSubtypeIDs.BossT1,
                VanillaSubtypeIDs.BossT2,
                VanillaSubtypeIDs.BossT3,
            };

            string[] bigBossSubtypes =
            {
                VanillaSubtypeIDs.BigBoss2,
                VanillaSubtypeIDs.BigBossT1,
                VanillaSubtypeIDs.BigBossT3,
            };

            List<RelicEffectData> bossUpgrades = new List<RelicEffectData>();

            foreach (string subtype in bossSubtypes)
            {
                RelicEffectData relicEffect = new RelicEffectDataBuilder
                {
                    RelicEffectClassType = typeof(RelicEffectAddStatusEffectOnSpawn),
                    ParamSourceTeam = Team.Type.Heroes,
                    ParamCharacterSubtype = subtype,
                    ParamStatusEffects =
                    {
                        new StatusEffectStackData {statusId = StatusEffectDamageShieldState.StatusId, count = 9},
                    }
                }.Build();
                bossUpgrades.Add(relicEffect);
            }

            foreach (string subtype in bigBossSubtypes)
            {
                RelicEffectData relicEffect = new RelicEffectDataBuilder
                {
                    RelicEffectClassType = typeof(RelicEffectAddStatusEffectOnSpawn),
                    ParamSourceTeam = Team.Type.Heroes,
                    ParamCharacterSubtype = subtype,
                    ParamStatusEffects =
                    {
                        new StatusEffectStackData {statusId = StatusEffectDamageShieldState.StatusId, count = 19},
                    }
                }.Build();
                bossUpgrades.Add(relicEffect);
            }

            RelicEffectData divinityDamageShield = new RelicEffectDataBuilder
            {
                RelicEffectClassType = typeof(RelicEffectAddStatusEffectOnSpawn),
                ParamSourceTeam = Team.Type.Heroes,
                ParamCharacterSubtype = VanillaSubtypeIDs.FinalBoss,
                ParamStatusEffects =
                {
                    new StatusEffectStackData {statusId = StatusEffectDamageShieldState.StatusId, count = 29},
                }
            }.Build();
            bossUpgrades.Add(divinityDamageShield);

            return bossUpgrades;
        }
    }
}
