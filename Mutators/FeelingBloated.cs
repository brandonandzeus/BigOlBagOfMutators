using BigOlBagOfMutators.CustomTraits;
using System;
using System.Collections.Generic;
using System.Text;
using Trainworks.BuildersV2;

namespace BigOlBagOfMutators.Mutators
{
    public class FeelingBloated
    {
        public static void Make()
        {
            var upgrade = new CardUpgradeDataBuilder
            {
                UpgradeID = "AddSpellBloat",
                TraitDataUpgradeBuilders =
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateType = typeof(CardTraitBloat),
                    }
                },
                FiltersBuilders =
                {
                    new CardUpgradeMaskDataBuilder
                    {
                        CardUpgradeMaskID = "FilterSpells",
                        CardType = CardType.Spell,
                    }
                }
            };

            var mutator = new MutatorDataBuilder
            {
                MutatorID = "FeelingBloated",
                Name = "Feeling Bloated",
                Description = "When any spell card is played, a copy of the spell card is added to your discard pile.",
                BoonValue = 0,
                Tags = { },
                EffectBuilders =
                {
                    new RelicEffectDataBuilder
                    {
                        RelicEffectClassType = typeof(RelicEffectUpgradeCardOnGain),
                        ParamCardUpgradeDataBuilder = upgrade,
                    }
                },
                IconPath = "Assets/MTR_FeelingBloated.png",
            };
            mutator.BuildAndRegister();
        }
    }
}
