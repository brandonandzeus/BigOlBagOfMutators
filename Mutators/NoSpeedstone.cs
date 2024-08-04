using BigOlBagOfMutators.RelicEffects;

using Trainworks.BuildersV2;
using Trainworks.ConstantsV2;
using Trainworks.Managers;

namespace BigOlBagOfMutators.Mutators
{
    public class NoSpeedstone
    {
        public static void Make()
        {
            MutatorDataBuilder builder = new MutatorDataBuilder
            {
                MutatorID = "NoSpeedstone",
                Name = "No Speedstone",
                Description = "Speedstone no longer appears in Merchant of Steel.",
                EffectBuilders = {
                    new RelicEffectDataBuilder
                    {
                        RelicEffectClassType = typeof(RelicEffectBanEnhancer),
                        ParamString = VanillaEnhancerIDs.Speedstone,
                    },
                },
                BoonValue = -4,
                Tags = { "Steel" },
                IconPath = "Assets/MTR_NoSpeedstone.png",
            };
            builder.BuildAndRegister();
        }
    }
}
