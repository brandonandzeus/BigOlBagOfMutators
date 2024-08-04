using BigOlBagOfMutators.RelicEffects;

using Trainworks.BuildersV2;
using Trainworks.ConstantsV2;
using Trainworks.Managers;

namespace BigOlBagOfMutators.Mutators
{
    public class NoImmortalstone
    {
        public static void Make()
        {
            MutatorDataBuilder builder = new MutatorDataBuilder
            {
                MutatorID = "NoImmortalStone",
                Name = "No Immortalstone",
                Description = "Immortalstone no longer appears in Merchant of Steel.",
                EffectBuilders = {
                    new RelicEffectDataBuilder
                    {
                        RelicEffectClassType = typeof(RelicEffectBanEnhancer),
                        ParamString = VanillaEnhancerIDs.Immortalstone,
                    },
                },
                BoonValue = -4,
                Tags = { "Steel" },
                IconPath = "Assets/MTR_NoImmortalstone.png",
            };
            builder.BuildAndRegister();
        }
    }
}
