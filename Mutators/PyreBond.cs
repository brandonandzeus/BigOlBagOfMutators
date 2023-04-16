

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

            MutatorDataBuilder builder = new MutatorDataBuilder
            {
                MutatorID = "Pyrebond",
                Name = "Pyrebond",
                Description = "Deal 1 damage to the Pyre whenever a friendly unit takes damage.",
                EffectBuilders = { addHitTrigger },
                BoonValue = -7,
                IconPath = "MTR_Pyrebond.png",
            };

            builder.BuildAndRegister();
        }
    }

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
}
