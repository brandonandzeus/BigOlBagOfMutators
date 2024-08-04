using System;
using System.Collections.Generic;
using System.Text;
using Trainworks.BuildersV2;
using Trainworks.Constants;

namespace BigOlBagOfMutators.Mutators
{
    public class WereAllRobots
    {
        public static void Make()
        {
            var mutator = new MutatorDataBuilder
            {
                MutatorID = "WereAllRobots",
                Name = "We're All Robots",
                Description = "Friendly Units enter with <b>Inert</b> and '<b>Incant</b>: Gain <b>Fuel 1</b>'",
                BoonValue = -4,
                EffectBuilders =
                {
                    new RelicEffectDataBuilder
                    {
                        RelicEffectClassType = typeof(RelicEffectAddStatusEffectOnSpawn),
                        ParamSourceTeam = Team.Type.Monsters,
                        ParamInt = -1,
                        ParamStatusEffects =
                        {
                            new StatusEffectStackData {statusId = VanillaStatusEffectIDs.Inert, count = 1},
                        }
                    },
                    new RelicEffectDataBuilder
                    {
                        RelicEffectClassType = typeof(RelicEffectAddTrigger),
                        ParamSourceTeam = Team.Type.Monsters,
                        TriggerBuilders =
                        {
                            new CharacterTriggerDataBuilder
                            {
                                TriggerID = "WereAllRobotsTrigger",
                                Trigger = CharacterTriggerData.Trigger.CardSpellPlayed,
                                Description = "Gain <b>Fuel 1</b>",
                                HideTriggerTooltip = true,
                                EffectBuilders =
                                {
                                    new CardEffectDataBuilder
                                    {
                                        EffectStateType = typeof(CardEffectAddStatusEffect),
                                        TargetMode = TargetMode.Self,
                                        TargetTeamType = Team.Type.Monsters,
                                        ParamStatusEffects =
                                        {
                                            new StatusEffectStackData {statusId = VanillaStatusEffectIDs.Fuel, count = 1 }
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                IconPath = "Assets/MTR_WereAllRobots.png",
            };

            mutator.BuildAndRegister();
        }
    }
}
