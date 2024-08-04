using BigOlBagOfMutators.RelicEffects;

using Trainworks.BuildersV2;
using Trainworks.ConstantsV2;
using Trainworks.Managers;

namespace BigOlBagOfMutators.Mutators
{
    public class NoLargestone
    {
        public static void Make()
        {
            MutatorDataBuilder builder = new MutatorDataBuilder
            {
                MutatorID = "NoLargestone",
                Name = "No Largestone",
                Description = "Largestone no longer appears in Merchant of Steel.",
                EffectBuilders = {
                    new RelicEffectDataBuilder
                    {
                        RelicEffectClassType = typeof(RelicEffectBanEnhancer),
                        ParamString = VanillaEnhancerIDs.Largestone,
                    },
                },
                BoonValue = -4,
                Tags = { "Steel" },
                IconPath = "Assets/MTR_NoLargestone.png",
            };
            builder.BuildAndRegister();
        }
    }
}
