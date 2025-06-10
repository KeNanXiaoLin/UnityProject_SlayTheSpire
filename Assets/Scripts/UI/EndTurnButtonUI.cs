using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButtonUI : MonoBehaviour
{
    public void OnClick()
    {
        EnermyTurnGA ga = new EnermyTurnGA();
        ActionSystem.Instance.Perform(ga);
    }
}
