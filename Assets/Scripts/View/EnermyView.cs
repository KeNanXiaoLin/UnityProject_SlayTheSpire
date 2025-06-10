using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnermyView : CombatantView
{
    [SerializeField] private TMP_Text atkText;

    public int AtkAmount { get; set; }

    public void SetUp(int atkAmount)
    {
        AtkAmount = atkAmount;
        UpdateAtkText();
        SetUpBase(100, null);
    }

    public void UpdateAtkText()
    {
        if (atkText != null)
        {
            atkText.text = $"ATK: {AtkAmount}";
        }
    }
}
