using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCardGA : GameAction
{
    public EnemyView ManualTarget { get; private set; }
    public Card Card { get; private set; }

    public PlayCardGA(Card card):this(card,null)
    {
        
    }

    public PlayCardGA(Card card, EnemyView manualTarget)
    {
        this.Card = card;
        this.ManualTarget = manualTarget;
    }
}
