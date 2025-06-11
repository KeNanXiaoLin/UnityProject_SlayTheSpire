using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSystem : SingletonMono<HeroSystem>
{
    [field:SerializeField] public HeroView HeroView { get; private set; }

    public void SetUp(HeroData data)
    {
        HeroView.SetUp(data);
    }
}
