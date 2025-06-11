using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpendManaGA : GameAction
{
    public int ManaCost { get; set; }

    public SpendManaGA(int manaCost)
    {
        ManaCost = manaCost;
    }
}
