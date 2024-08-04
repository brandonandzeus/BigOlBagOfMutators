using BigOlBagOfMutators.RelicEffects;

using Trainworks.BuildersV2;
using Trainworks.ConstantsV2;
using Trainworks.Managers;

namespace BigOlBagOfMutators.Mutators
{
    public class NoFreezestone
    {
        public static void Make()
        {
            MutatorDataBuilder builder = new MutatorDataBuilder
            {
                MutatorID = "NoFreezestone",
                Name = "No Freezestone",
                Description = "Freezestone no longer appears in Merchant of Magic.",
                EffectBuilders = {
                    new RelicEffectDataBuilder
                    {
                        RelicEffectClassType = typeof(RelicEffectBanEnhancer),
                        ParamString = VanillaEnhancerIDs.Freezestone,
                    },
                },
                BoonValue = -2,
                Tags = { "Magic" },
                IconPath = "Assets/MTR_NoFreezestone.png",
            };
            builder.BuildAndRegister();
        }
    }
}
