using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyView : CombatantView
{
    [SerializeField] private TMP_Text atkText;

    public int AtkAmount { get; set; }

    public void SetUp(EnemyData data)
    {
        AtkAmount = data.Atk;
        UpdateAtkText();
        SetUpBase(data.Health, data.Image);
    }

    public void UpdateAtkText()
    {
        if (atkText != null)
        {
            atkText.text = $"ATK: {AtkAmount}";
        }
    }
}
