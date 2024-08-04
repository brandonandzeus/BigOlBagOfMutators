using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using Trainworks.ConstantsV2;
using Trainworks.Enums.MTTriggers;

namespace BigOlBagOfMutators.CustomTriggers
{
    public static class OnSting
    {
        public static CharacterTrigger Trigger = new CharacterTrigger("Trigger_OnSting");
    }

    [HarmonyPatch(typeof(CardManager), nameof(CardManager.FireUnitTriggersForCardPlayed))]
    class StingcastPatch
    {
        public static void Prefix(ICharacterManager characterManager, CardState playedCard, int playedRoom, List<CharacterState> overrideCharacterList, CombatManager ___combatManager)
        {
            if (playedCard?.GetCardDataID() != VanillaCardIDs.Sting)
            {
                return;
            }
            for (int i = 0; i < characterManager.GetNumCharacters(); i++)
            {
                CharacterState character = characterManager.GetCharacter(i);
                if (overrideCharacterList != null)
                {
                    if (character != null && !character.IsDestroyed && character.IsAlive && overrideCharacterList.Contains(character))
                    {
                        ___combatManager.QueueTrigger(character, OnSting.Trigger);
                    }
                }
                else if (playedCard.CharacterInRoomAtTimeOfCardPlay(character))
                {
                    ___combatManager.QueueTrigger(character, OnSting.Trigger);
                }
            }
        }
    }
}
