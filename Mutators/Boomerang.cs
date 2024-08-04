using System;
using System.Collections.Generic;
using System.Text;
using Trainworks.BuildersV2;
using Trainworks.ConstantsV2;
using Trainworks.Managers;

namespace BigOlBagOfMutators.Mutators
{
    public class Boomerang
    {
        public static void Make()
        {
            var allGameData = ProviderManager.SaveManager.GetAllGameData();

            var holdover = allGameData.FindEnhancerData(VanillaEnhancerIDs.Keepstone);
            var upgrade = holdover.GetFirstRelicEffectData<RelicEffectCardUpgrade>()?.GetParamCardUpgradeData();

            // Apparently Holdover doesn't filter out spells. Keepstone does.
            var holdoverUpgrade = new CardUpgradeDataBuilder
            {
                UpgradeID = "BoomerangHoldover",
                TraitDataUpgrades =
                {
                    upgrade.GetTraitDataUpgrades()[0],
                },
                Filters = upgrade.GetFilters(),
                FiltersBuilders =
                {
                    new CardUpgradeMaskDataBuilder
                    {
                        CardUpgradeMaskID = "OnlySpells",
                        CardType = CardType.Spell,
                    }
                }
            };

            var mutator = new MutatorDataBuilder
            {
                MutatorID = "Boomerang",
                Name = "Boomerang",
                Description = "All spells gain [retain]",
                BoonValue = 3,
                EffectBuilders =
                {
                    new RelicEffectDataBuilder
                    {
                        RelicEffectClassType = typeof(RelicEffectUpgradeCardOnGain),
                        ParamCardUpgradeDataBuilder = holdoverUpgrade,
                    }
                },
                IconPath = "Assets/MTR_Boomerang.png",
            };

            mutator.BuildAndRegister();
        }
    }
}
