using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    private readonly CardData data;
    public string Title => data.CardName;
    public string Des => data.Des;
    public Sprite Image => data.CardImage;
    public List<Effect> Effects => data.Effects;
    public int Mana { get; private set; }


    public Card(CardData data)
    {
        this.data = data;
        Mana = data.Mana;
    }
}
