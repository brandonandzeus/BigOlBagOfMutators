using BigOlBagOfMutators.RelicEffects;

using Trainworks.BuildersV2;
using Trainworks.ConstantsV2;
using Trainworks.Managers;

namespace BigOlBagOfMutators.Mutators
{
    public class NoKeepstone
    {
        public static void Make()
        {
            MutatorDataBuilder builder = new MutatorDataBuilder
            {
                MutatorID = "NoKeepstone",
                Name = "No Keepstone",
                Description = "Keepstone no longer appears in Merchant of Magic.",
                EffectBuilders = {
                    new RelicEffectDataBuilder
                    {
                        RelicEffectClassType = typeof(RelicEffectBanEnhancer),
                        ParamString = VanillaEnhancerIDs.Keepstone,
                    },
                },
                BoonValue = -5,
                Tags = { "Magic" },
                IconPath = "Assets/MTR_NoKeepstone.png",
            };
            builder.BuildAndRegister();
        }
    }
}
