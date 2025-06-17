using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualTargetSystem : SingletonMono<ManualTargetSystem>
{
    [SerializeField] private ArrowView arrowView;
    [SerializeField] private LayerMask targetLayerMask;

    public void StartTargeting(Vector3 startPosition)
    {
        arrowView.gameObject.SetActive(true);
        arrowView.SetUpArrow(startPosition);
    }

    public CombatantView EndTargeting(Vector3 endPosition)
    {
        arrowView.gameObject.SetActive(false);
        if (Physics.Raycast(endPosition, Vector3.forward, out RaycastHit hit, 100f, targetLayerMask)
            && hit.collider != null
            && hit.transform.TryGetComponent<CombatantView>(out CombatantView combatantView))
        {   
            return combatantView;
        }
        return null;
    }
}
