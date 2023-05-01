namespace BigOlBagOfMutators.RelicEffects
{
    interface IModifyTowerHealthRelicEffect : IRelicEffect
    {
        int ModifyTowerHealAmount(int amount);
    }
}
