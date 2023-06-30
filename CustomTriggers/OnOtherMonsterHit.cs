using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Trainworks.ConstantsV2;
using Trainworks.Enums.MTTriggers;

namespace BigOlBagOfMutators.CustomTriggers
{
    public static class OnOtherMonsterHit
    {
        public static CharacterTriggerData.Trigger MonsterTrigger = (new CharacterTrigger("Trigger_OnOtherMonsterHit")).GetEnum();
        public static CharacterTriggerData.Trigger ImpTrigger = (new CharacterTrigger("Trigger_OnOtherImpHit")).GetEnum();
    }

    [HarmonyPatch(typeof(CharacterState), nameof(CharacterState.ApplyDamage))]
    class OnOtherMonsterHitPatch
    {
        private struct CharacterHpState
        {
            public int hp;
            public int armor;
        }
        
        private static CharacterHpState hpState = new CharacterHpState();
        public static List<CharacterState> outCharacters = new List<CharacterState>();

        public static void Prefix(CharacterState __instance)
        {
            hpState.hp = __instance.GetHP();
            hpState.armor = __instance.GetStatusEffectStacks(VanillaStatusEffectIDs.Armor);
        }

        public static IEnumerator Postfix(IEnumerator enumerator, CharacterState __instance, CombatManager ___combatManager)
        {
            while (enumerator.MoveNext())
                yield return enumerator.Current;

            if (__instance == null)
                yield break;

            if (__instance.GetTeamType() == Team.Type.Heroes)
                yield break;

            var lastAttackerCharacter = __instance.GetLastAttackerCharacter();
            if (lastAttackerCharacter == null || lastAttackerCharacter.IsDead || lastAttackerCharacter.IsDestroyed)
                yield break;

            // Test if the character was hit. If the HP or Armor changes then it should trigger OnHit
            if (__instance.GetHP() == hpState.hp && __instance.GetStatusEffectStacks(VanillaStatusEffectIDs.Armor) == hpState.armor)
            {
                yield break;
            }

            outCharacters.Clear();
            RoomState room = __instance.GetSpawnPoint().GetRoomOwner();
            room.AddCharactersToList(outCharacters, Team.Type.Monsters);

            foreach (CharacterState character in outCharacters)
            {
                if (character == __instance)
                {
                    continue;
                }
                yield return ___combatManager.QueueAndRunTrigger(character, OnOtherMonsterHit.MonsterTrigger, fireTriggersData: new CharacterState.FireTriggersData
                {
                    // This hack is used to get the LastAttackerCharacter in CardEffects.
                    // Set TargetMode.LastAttackedCharacter, unintuitive, but that's how the code for overrideTargetCharacter was written.
                    overrideTargetCharacter = __instance.GetLastAttackerCharacter(),
                });
                if (__instance.GetHasSubtype(SubtypeManager.GetSubtypeData(VanillaSubtypeIDs.Imp)))
                {
                    yield return ___combatManager.QueueAndRunTrigger(character, OnOtherMonsterHit.ImpTrigger, fireTriggersData: new CharacterState.FireTriggersData
                    {
                        overrideTargetCharacter = __instance.GetLastAttackerCharacter(),
                    });
                }
            }
        }
    }
}
