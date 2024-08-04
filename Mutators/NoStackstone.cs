using BigOlBagOfMutators.RelicEffects;

using Trainworks.BuildersV2;
using Trainworks.ConstantsV2;
using Trainworks.Managers;

namespace BigOlBagOfMutators.Mutators
{
    public class NoStackstone
    {
        public static void Make()
        {
            MutatorDataBuilder builder = new MutatorDataBuilder
            {
                MutatorID = "NoStackstone",
                Name = "No Stackstone",
                Description = "Stackstone no longer appears in Merchant of Magic.",
                EffectBuilders = {
                    new RelicEffectDataBuilder
                    {
                        RelicEffectClassType = typeof(RelicEffectBanEnhancer),
                        ParamString = VanillaEnhancerIDs.Stackstone,
                    },
                },
                BoonValue = -3,
                Tags = { "Magic" },
                IconPath = "Assets/MTR_NoStackstone.png",
            };
            builder.BuildAndRegister();
        }
    }
}
