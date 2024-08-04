using BepInEx.Logging;
using BigOlBagOfMutators.RelicEffects;

using Trainworks.BuildersV2;
using Trainworks.ConstantsV2;
using Trainworks.Managers;

namespace BigOlBagOfMutators.Mutators
{
    public class NoFrenzystone
    {
        public static void Make()
        {
            MutatorDataBuilder builder = new MutatorDataBuilder
            {
                MutatorID = "NoFrenzystone",
                Name = "No Frenzystone",
                Description = "Frenzystone no longer appears in Merchant of Steel.",
                EffectBuilders = {
                    new RelicEffectDataBuilder
                    {
                        RelicEffectClassType = typeof(RelicEffectBanEnhancer),
                        ParamString = VanillaEnhancerIDs.Frenzystone,
                    },
                },
                BoonValue = -4,
                Tags = { "Steel", "ShopMultistrike"},
                IconPath = "Assets/MTR_NoFrenzystone.png",
            };
            builder.BuildAndRegister();
        }
    }
}
