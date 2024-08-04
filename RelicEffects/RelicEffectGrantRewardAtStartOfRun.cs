using System.Collections;
using System.Collections.Generic;
using Trainworks.Custom.RelicEffects;
using Trainworks.Managers;

namespace BigOlBagOfMutators.RelicEffects
{
    public class RelicEffectGrantRewardAtStartOfRun : RelicEffectBase, IPostStartOfRunRelicEffect
    {
        private RewardData reward;
        
        private int times;
        private List<RewardItemUI> rewardUIs = new List<RewardItemUI>();
        private List<RewardState> rewards = new List<RewardState>();

        public override void Initialize(RelicState relicState, RelicData relicData, RelicEffectData relicEffectData)
        {
            base.Initialize(relicState, relicData, relicEffectData);
            reward = relicEffectData.GetParamReward();
            times = relicEffectData.GetParamInt();
            for (int i = 0; i < times; i++)
            {
                rewards.Add(new RewardState(reward as GrantableRewardData, ProviderManager.SaveManager));
                rewardUIs.Add(new RewardItemUI());
            }
        }

        public void ApplyEffect(RelicEffectParams relicEffectParams)
        {
            ScreenManager screenManager;
            ProviderManager.TryGetProvider<ScreenManager>(out screenManager);

            /*foreach (var rewardUI in rewardUIs)
            {
                rewardUI.SetFloatingRewardPosition();
            }*/
            
            screenManager.ShowScreen(ScreenName.Reward, delegate (IScreen screen)
            {
                (screen as RewardScreen).Show(rewards, GrantableRewardData.Source.Map, HandleRewardGranted);
            });
        }

        private void HandleRewardGranted(GrantResult result)
        {
            foreach (RewardItemUI rewardUI in rewardUIs)
            {
                rewardUI.RefreshClaimed();
            }
        }
    }
}
