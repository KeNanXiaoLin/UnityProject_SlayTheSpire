using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : SingletonMono<EnemySystem>
{
    [SerializeField] private EnemyBoradView enemyBoradView;

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<EnemyTurnGA>(EnemyTurnPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<EnemyTurnGA>();
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
        Debug.Log("Enemy Turn");
        yield return new WaitForSeconds(1f);
        Debug.Log("End Enemy Turn");
    }
}
