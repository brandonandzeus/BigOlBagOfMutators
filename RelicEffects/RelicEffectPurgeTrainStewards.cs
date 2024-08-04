using System;
using System.Collections.Generic;
using System.Text;
using Trainworks.ConstantsV2;
using Trainworks.Custom.RelicEffects;

namespace BigOlBagOfMutators.RelicEffects
{
    /// Can't use the PurgeRelicEffect since it will execute before Convenents and Deck setup.
    public class RelicEffectPurgeTrainStewards : RelicEffectBase, IRelicEffect, IPostStartOfRunRelicEffect
    {
        public void ApplyEffect(RelicEffectParams relicEffectParams)
        {
            relicEffectParams.saveManager.GetDeckState().RemoveAll((CardState card) => card.GetCardDataID() == VanillaCardIDs.TrainSteward);
            foreach (var card in relicEffectParams.saveManager.GetDeckState())
            {
                Trainworks.Trainworks.Log("Deck: " + card.GetDebugName());
            }
        }
    }
}
