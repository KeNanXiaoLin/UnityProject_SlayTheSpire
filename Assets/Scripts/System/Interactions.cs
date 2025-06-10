using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactions : BaseManager<Interactions>
{
    private Interactions() { }

    public bool PlayerIsDragging { get; set; } = false;

    public bool PlayerCanInteract()
    {
        if (!ActionSystem.Instance.IsPerforming) return true;
        else return false;
    }

    public bool PlayerCanHover()
    {
        if(PlayerIsDragging) return false;
        else return true;
    }
}
