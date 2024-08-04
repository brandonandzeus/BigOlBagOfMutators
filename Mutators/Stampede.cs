using System;
using System.Collections.Generic;
using System.Text;
using Trainworks.BuildersV2;
using Trainworks.ConstantsV2;

namespace BigOlBagOfMutators.Mutators
{
    public class Stampede
    {
        public static void Make()
        {
            var mutator = new MutatorDataBuilder
            {
                MutatorID = "Stampede",
                Name = "Stampede!",
                Description = "All units get <b>Trample</b>",
                BoonValue = -5,
                Tags = { },
                EffectBuilders =
                {
                    new RelicEffectDataBuilder
                    {
                        RelicEffectClassType = typeof(RelicEffectAddTempUpgrade),
                        ParamSourceTeam = Team.Type.Monsters,
                        ParamStatusEffects =
                        {
                            new StatusEffectStackData {statusId = VanillaStatusEffectIDs.Trample, count = 1 },
                        },
                        ParamCardUpgradeDataBuilder = new CardUpgradeDataBuilder
                        {
                            UpgradeID = "StampedeTrampleUpgrade",
                            StatusEffectUpgrades =
                            {
                                new StatusEffectStackData {statusId = VanillaStatusEffectIDs.Trample, count = 1 },
                            }
                        }
                    },
                    new RelicEffectDataBuilder
                    {
                        RelicEffectClassType = typeof(RelicEffectAddStatusEffectOnSpawn),
                        ParamSourceTeam = Team.Type.Heroes,
                        ParamInt = -1,
                        ParamStatusEffects = 
                        {
                            new StatusEffectStackData {statusId = VanillaStatusEffectIDs.Trample, count = 1 },
                        },
                    }
                },
                IconPath = "Assets/MTR_Stampede.png",
            };
            mutator.BuildAndRegister();
        }
    }
}
