using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeroView : CombatantView
{
    [SerializeField] private TMP_Text ShieldTMP;
    public int CurrentShield { get; set; }

    public void SetUp(HeroData data)
    {
        SetUpBase(data.Health, data.Image);
        UpdateShieldText();
    }

    public void UpdateShieldText()
    {
        if (ShieldTMP != null)
        {
            ShieldTMP.text = $"Shield: {CurrentShield}";
        }
    }

    /// <summary>
    /// 增加护盾效果
    /// </summary>
    /// <param name="amount"></param>
    public void Shield(int amount)
    {
        CurrentShield += amount;
        transform.DOShakePosition(0.2f, 0.5f);
        UpdateShieldText();
    }

    /// <summary>
    /// 因为玩家存在护盾，所以重写伤害方法，优先攻击护盾
    /// </summary>
    /// <param name="amount"></param>
    public override void Damage(int amount)
    {
        if (CurrentShield > 0)
        {
            //先减护盾
            CurrentShield -= amount;
            //如果护盾不够，在扣除生命值
            if (CurrentShield < 0)
            {
                int overflowVlaue = -CurrentShield;
                CurrentShield = 0;
                CurrentHealth -= overflowVlaue;
                UpdateShieldText();
                ShowDamageEffect();
            }
        }
        else
        {
            base.Damage(amount);
        }
    }
}
