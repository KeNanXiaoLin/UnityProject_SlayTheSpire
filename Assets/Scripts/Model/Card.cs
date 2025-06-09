using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    private readonly CardData data;
    public string Title => data.cardName;
    public string Des => data.des;
    public Sprite Image => data.cardImage;
    public int Mana { get; private set; }

    public Card(CardData data)
    {
        this.data = data;
        Mana = data.mana;
    }
}
