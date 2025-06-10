using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<EnemyTurnGA>(EnemyTurnPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<EnemyTurnGA>();
    }

    private IEnumerator EnemyTurnPerformer(EnemyTurnGA ga)
    {
        Debug.Log("Enemy Turn");
        yield return new WaitForSeconds(1f);
        Debug.Log("End Enemy Turn");
    }
}
