using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkSystem : SingletonMono<PerkSystem>
{
    private readonly List<Perk> perks = new List<Perk>();

    public void Add(Perk perk)
    {
        perks.Add(perk);
        perk.OnAdd();
    }

    public void Remove(Perk perk)
    {
        if (perks.Contains(perk))
        {
            perks.Remove(perk);
            perk.OnRemove();
        }
    }
}
