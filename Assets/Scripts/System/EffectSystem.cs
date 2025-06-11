using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSystem : MonoBehaviour
{

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<PerformEffectGA>(PerformEffectPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<PerformEffectGA>();
    }

    private IEnumerator PerformEffectPerformer(PerformEffectGA performEffectGA)
    {
        Effect effect = performEffectGA.Effect;
        GameAction effectAction = effect.GetGameAction(performEffectGA.Targets);
        ActionSystem.Instance.AddReaction(effectAction);
        yield return null;
    }
}
