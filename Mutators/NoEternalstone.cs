using BigOlBagOfMutators.RelicEffects;

using Trainworks.BuildersV2;
using Trainworks.ConstantsV2;
using Trainworks.Managers;

namespace BigOlBagOfMutators.Mutators
{
    public class NoEternalstone
    {
        public static void Make()
        {
            MutatorDataBuilder builder = new MutatorDataBuilder
            {
                MutatorID = "NoEternalstone",
                Name = "No Eternalstone",
                Description = "Eternalstone no longer appears in Merchant of Magic.",
                EffectBuilders = {
                    new RelicEffectDataBuilder
                    {
                        RelicEffectClassType = typeof(RelicEffectBanEnhancer),
                        ParamString = VanillaEnhancerIDs.Eternalstone,
                    },
                },
                BoonValue = -2,
                Tags = {"Magic"},
                IconPath = "Assets/MTR_NoEternalstone.png",
            };
            builder.BuildAndRegister();
        }
    }
}
