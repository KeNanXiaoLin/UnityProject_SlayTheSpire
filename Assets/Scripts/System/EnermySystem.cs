using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermySystem : MonoBehaviour
{
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<EnermyTurnGA>(EnermyTurnPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<EnermyTurnGA>();
    }

    private IEnumerator EnermyTurnPerformer(EnermyTurnGA ga)
    {
        Debug.Log("Enermy Turn");
        yield return new WaitForSeconds(1f);
        Debug.Log("End Enermy Turn");
    }
}
