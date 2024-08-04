using ShinyShoe.Logging;
using System.Collections;
using Trainworks.Managers;

namespace BigOlBagOfMutators.RoomModifiers
{
    public class RoomStateMaxHPOnSpawnModifier : RoomStateModifierBase, IRoomStateSpawnModifier
    {
        public static readonly string ActivatedKey = "CardEffectAdjustMaxHealth_Activated";

        private int maxHPAdjustment;

        public override void Initialize(RoomModifierData roomModifierData, RoomManager roomManager)
        {
            base.Initialize(roomModifierData, roomManager);
            maxHPAdjustment = roomModifierData.GetParamInt();
        }

        public IEnumerator CharacterSpawned(CharacterState spawnedChar, MonsterManager monsterManager, HeroManager heroManager)
        {
            if (spawnedChar.GetTeamType() != Team.Type.Monsters)
                yield break;

            PopupNotificationManager popupNotificationManager = null;
            ProviderManager.TryGetProvider(out popupNotificationManager);
            NotifyHealthEffectTriggered(ProviderManager.SaveManager, popupNotificationManager, GetActivatedDescription(), spawnedChar.GetCharacterUI());
            spawnedChar.SetHealth(spawnedChar.GetHP(), spawnedChar.GetMaxHP() + maxHPAdjustment);
            spawnedChar.UpdateCharacterStateUI();
            yield break;
        }

        private void NotifyHealthEffectTriggered(SaveManager saveManager, PopupNotificationManager popupNotificationManager, string text, CharacterUI source)
        {
            if (popupNotificationManager == null || saveManager == null)
            {
                Log.Warning(LogGroups.Gameplay, "Failed to show card effect notification, missing dependencies.");
            }
            else if (!saveManager.PreviewMode && !text.IsNullOrEmpty())
            {
                PopupNotificationUI.NotificationData notificationData = default(PopupNotificationUI.NotificationData);
                notificationData.text = text;
                notificationData.colorType = ColorDisplayData.ColorType.Default;
                notificationData.source = PopupNotificationUI.Source.General;
                notificationData.forceUseCountLabel = true;
                PopupNotificationUI.NotificationData notificationData2 = notificationData;
                popupNotificationManager.ShowNotification(notificationData2, source);
            }
        }

        public string GetActivatedDescription()
        {
            if (!ActivatedKey.HasTranslation())
            {
                return null;
            }
            return string.Format(ActivatedKey.Localize(), maxHPAdjustment);
        }
    }
}
