using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoradView : MonoBehaviour
{
    [SerializeField] private List<Transform> slots;

    public List<EnemyView> EnemyViews { get; private set; } = new List<EnemyView>();

    public void AddEnemy(EnemyData data)
    {
        Transform slot = slots[EnemyViews.Count];
        EnemyView enemyView = EnemyViewCreator.Instance.CreateEnemyView(data, slot.position,slot.rotation);
        enemyView.transform.SetParent(slot);
        EnemyViews.Add(enemyView);
    }
}
