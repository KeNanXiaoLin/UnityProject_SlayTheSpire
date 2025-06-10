using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButtonUI : MonoBehaviour
{
    public void OnClick()
    {
        EnemyTurnGA ga = new EnemyTurnGA();
        ActionSystem.Instance.Perform(ga);
    }
}
