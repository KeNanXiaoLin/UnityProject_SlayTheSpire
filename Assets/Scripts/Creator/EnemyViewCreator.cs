using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyViewCreator : SingletonMono<EnemyViewCreator>
{
    [SerializeField] private EnemyView enemyViewPrefab;

    public EnemyView CreateEnemyView(EnemyData data, Vector3 position,Quaternion rotation)
    {
        EnemyView enemyView = Instantiate(enemyViewPrefab, position, rotation);
        enemyView.SetUp(data);
        return enemyView;
    }
}
