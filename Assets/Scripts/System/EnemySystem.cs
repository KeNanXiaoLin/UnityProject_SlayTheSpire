using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : SingletonMono<EnemySystem>
{
    [SerializeField] private EnemyBoradView enemyBoradView;
    public List<EnemyView> enemies => enemyBoradView.EnemyViews;

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<EnemyTurnGA>(EnemyTurnPerformer);
        ActionSystem.AttachPerformer<AttackHeroGA>(AttackHeroPerformer);
        ActionSystem.AttachPerformer<KillEnemyGA>(KillEnemyPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<EnemyTurnGA>();
        ActionSystem.DetachPerformer<AttackHeroGA>();
        ActionSystem.DetachPerformer<KillEnemyGA>();
    }

    public void SetUp(List<EnemyData> datas)
    {
        foreach (var data in datas)
        {
            enemyBoradView.AddEnemy(data);
        }
    }

    private IEnumerator EnemyTurnPerformer(EnemyTurnGA ga)
    {
        foreach (var enemy in enemyBoradView.EnemyViews)
        {
            AttackHeroGA attackHeroGA = new AttackHeroGA(enemy);
            ActionSystem.Instance.AddReaction(attackHeroGA);
        }
        yield return null;
    }

    private IEnumerator AttackHeroPerformer(AttackHeroGA ga)
    {
        EnemyView attacker = ga.Attacker;
        Tween tween = attacker.transform.DOMoveX(attacker.transform.position.x-1f,0.15f);
        yield return tween.WaitForCompletion();
        attacker.transform.DOMoveX(attacker.transform.position.x + 1f, 0.25f);
        //处理造成伤害
        DealDamageGA dealDamageGA = new DealDamageGA(attacker.AtkAmount, new() { HeroSystem.Instance.HeroView }, ga.Caster);
        ActionSystem.Instance.AddReaction(dealDamageGA);
    }

    private IEnumerator KillEnemyPerformer(KillEnemyGA killEnemyGA)
    {
        yield return enemyBoradView.RemoveEnemy(killEnemyGA.EnemyView);
    }
}
