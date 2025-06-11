using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroView : CombatantView
{
    public void SetUp(HeroData data)
    {
        SetUpBase(data.Health, data.Image);
    }
}
