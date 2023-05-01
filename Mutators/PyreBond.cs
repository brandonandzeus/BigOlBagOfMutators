

using BigOlBagOfMutators.RelicEffects;
using HarmonyLib;
using System.Collections;
using Trainworks.BuildersV2;

namespace BigOlBagOfMutators.Mutators
{
    class PyreBond
    {
        public static void Make()
        {
            CardEffectDataBuilder pyreDamage = new CardEffectDataBuilder
            {
                EffectStateType = typeof(CardEffectDamagePyre),
                ParamInt = -1,
            };

            CharacterTriggerDataBuilder hitTrigger = new CharacterTriggerDataBuilder
            {
                TriggerID = "Pyrebond",
                Trigger = CharacterTriggerData.Trigger.OnHit,
                Description = "Deal 1 damage to the Pyre when this unit takes damage.",
                HideTriggerTooltip = true,
                EffectBuilders = { pyreDamage },
            };

            RelicEffectDataBuilder addHitTrigger = new RelicEffectDataBuilder
            {
                RelicEffectClassType = typeof(RelicEffectAddTrigger),
                ParamSourceTeam = Team.Type.Monsters,
                TriggerBuilders = { hitTrigger },
            };

            RelicEffectDataBuilder doublePyreHeal = new RelicEffectDataBuilder
            {
                RelicEffectClassType = typeof(RelicEffectMultiplyPyreHealingRewards),
                ParamFloat = 2f,
            };

            MutatorDataBuilder builder = new MutatorDataBuilder
            {
                MutatorID = "Pyrebond",
                Name = "Pyrebond",
                Description = "Deal 1 damage to the Pyre whenever a friendly unit takes damage. <b>Pyre Remains</b> restores twice the usual amount.",
                EffectBuilders = { addHitTrigger, doublePyreHeal },
                BoonValue = -5,
                IconPath = "Assets/MTR_Pyrebond.png",
            };

            builder.BuildAndRegister();
        }
    }

    // Damages pyre without scrolling to the pyre room.
    class CardEffectDamagePyre : CardEffectBase
    {
        public override IEnumerator ApplyEffect(CardEffectState cardEffectState, CardEffectParams cardEffectParams)
        {
            if (cardEffectParams.selfTarget.HasStatusEffect(StatusEffectArmorState.StatusId))
                yield break;

            cardEffectParams.playerManager.AdjustTowerHP(cardEffectState.GetParamInt());
            yield break;
        }
    }

    [HarmonyPatch(typeof(HealthRewardData), nameof(HealthRewardData.GrantReward))]
    class PatchDoublePyreHeals
    {
        public static void Prefix(GrantableRewardData.GrantParams grantParams, ref int ____amount, ref int __state)
        {
            var relics = grantParams.saveManager.RelicManager.GetRelicEffects<IModifyTowerHealthRelicEffect>();
            __state = ____amount;
            foreach (var relic in relics)
            {
                ____amount = relic.ModifyTowerHealAmount(____amount);
            }
        }

        public static void Postfix(ref int ____amount, ref int __state)
        {
            // Reset after everything is said and done.
            ____amount = __state;
        }
    }
}
