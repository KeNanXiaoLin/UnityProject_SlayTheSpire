using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCardGA : GameAction
{
    public Card Card { get; private set; }

    public PlayCardGA(Card card)
    {
        this.Card = card;
    }
}
