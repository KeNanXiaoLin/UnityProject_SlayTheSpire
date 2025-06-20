using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatantView : MonoBehaviour
{
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public int MaxHealth { get; private set; }

    public int CurrentHealth { get; private set; }

    protected virtual void SetUpBase(int health, Sprite image)
    {
        MaxHealth = CurrentHealth = health;
        spriteRenderer.sprite = image;
        UpdateHealthText();
    }

    public void UpdateHealthText()
    {
        if (hpText != null)
        {
            hpText.text = $"HP: {CurrentHealth}/{MaxHealth}";
        }
    }

    public void Damage(int amount)
    {
        CurrentHealth -= amount;
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }
        transform.DOShakePosition(0.2f, 0.5f);
        UpdateHealthText();
    }
}