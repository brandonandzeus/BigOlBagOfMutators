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
                FiltersBuilders = { filterUnits },
            };
            CardUpgradeDataBuilder unitsDamageShieldUpgrade = new CardUpgradeDataBuilder
            {
                UpgradeID = "MadeOfGlassDamageShield",
                StatusEffectUpgrades = { damageshield1 },
                FiltersBuilders = { filterUnits },
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
                RelicEffectClassType = typeof(RelicEffectUpgradeCardOnGain),
                ParamSourceTeam = Team.Type.Monsters,
                ParamCardUpgradeDataBuilder = removePierceUpgrade,
            };

            MutatorDataBuilder builder = new MutatorDataBuilder
            {
                MutatorID = "MadeOfGlass",
                Name = "Made of Glass",
                Description = "All units get [fragile] and [damageshield] 1. Bosses have additional [damageshield]. Piercing is removed.",
                Effects = FormBossDamageShieldData(),
                EffectBuilders = { unitsFragile, unitsDamageShield, heroesFragile, heroesDamageShield, removePiercing },
                BoonValue = -3,
                IconPath = "Assets/MTR_MadeOfGlass.png",
            };

            builder.BuildAndRegister();
        }

        public static List<RelicEffectData> FormBossDamageShieldData()
        {
            Dictionary<string, int> SubtypeToDamageShield = new Dictionary<string, int>
            {
                [VanillaSubtypeIDs.BossT1] = 9,
                [VanillaSubtypeIDs.BossT2] = 14,
                [VanillaSubtypeIDs.BossT3] = 19,
                [VanillaSubtypeIDs.BigBossT1] = 14,
                // IDK why its just BigBoss2 instead of T2.
                [VanillaSubtypeIDs.BigBoss2] = 19,
                [VanillaSubtypeIDs.BigBossT3] = 39,
                [VanillaSubtypeIDs.FinalBoss] = 49,
            };

            List<RelicEffectData> bossUpgrades = new List<RelicEffectData>();

            foreach (var subtypeDamageShield in SubtypeToDamageShield)
            {
                RelicEffectData relicEffect = new RelicEffectDataBuilder
                {
                    RelicEffectClassType = typeof(RelicEffectAddStatusEffectOnSpawn),
                    ParamSourceTeam = Team.Type.Heroes,
                    ParamCharacterSubtype = subtypeDamageShield.Key,
                    ParamStatusEffects =
                    {
                        new StatusEffectStackData {statusId = StatusEffectDamageShieldState.StatusId, count = subtypeDamageShield.Value},
                    }
                }.Build();
                bossUpgrades.Add(relicEffect);
            }

            return bossUpgrades;
        }
    }
}
