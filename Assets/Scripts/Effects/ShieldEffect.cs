using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEffect : Effect
{
    [SerializeField] private int ShieldCount;
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView Caster)
    {
        return new ShieldGA(ShieldCount,targets);
    }
}
