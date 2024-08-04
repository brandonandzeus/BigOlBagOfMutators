using BigOlBagOfMutators.RelicEffects;

using Trainworks.BuildersV2;
using Trainworks.ConstantsV2;
using Trainworks.Managers;

namespace BigOlBagOfMutators.Mutators
{
    public class ForeignGoods
    {
        public static void Make()
        {
            var allGameData = ProviderManager.SaveManager.GetAllGameData();
            var clanUpgradePool = new EnhancerPoolBuilder
            {
                EnhancerPoolID = "ClanUpgradePool",
                Enhancers = 
                {
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Furystone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Thornstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Runestone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Shieldstone),
                    allGameData.FindEnhancerData(VanillaEnhancerIDs.Wickstone),
                    //VanillaEnhancerIDs.EchoStone, Nah
                }
            }.BuildAndRegister();

            MutatorDataBuilder builder = new MutatorDataBuilder
            {
                MutatorID = "ForeignGoods",
                Name = "Foreign Goods",
                Description = "Merchant of Steel sells all upgrades regardless of Clan.",
                EffectBuilders = {
                    new RelicEffectDataBuilder
                    {
                        RelicEffectClassType = typeof(RelicEffectAddAdditionalMerchantEnhancers),
                        ParamEnhancerPool = clanUpgradePool,
                    },
                },
                BoonValue = 4,
                Tags = {"Steel"},
                IconPath = "Assets/MTR_ForeignGoods.png",
            };
            builder.BuildAndRegister();
        }
    }
}
