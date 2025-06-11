using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageEffect : Effect
{
    [SerializeField] private int damageAmount;
    public override GameAction GetGameAction()
    {
        List<CombatantView> enemies = new(EnemySystem.Instance.enemies);
        DealDamageGA dealDamageGA = new DealDamageGA(damageAmount,enemies);
        return dealDamageGA;
    }
}
