using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSystem : SingletonMono<ShieldSystem>
{
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<ShieldGA>(ShieldPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<ShieldGA>();
    }

    private IEnumerator ShieldPerformer(ShieldGA ga)
    {
        Debug.Log("这个方法被执行");
        foreach (var target in ga.Targets)
        {
            //如果是玩家，增加护盾
            if (target is HeroView heroView)
            {
                Debug.Log("玩家增加护盾");
                heroView.CurrentShield += ga.Amount;
                heroView.UpdateShieldText();
            }
            //如果是敌人不做处理
            else
            {
                Debug.Log("这不是玩家对象，不做处理");
            }
            yield return null;
        }
    }
}
