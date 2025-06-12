using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    private readonly CardData data;
    public string Title => data.CardName;
    public string Des => data.Des;
    public Sprite Image => data.CardImage;
    public Effect ManualTargetEffect => data.ManualTargetEffect;
    public List<AutoTargetEffect> OtherEffects => data.OtherEffects;
    public int Mana { get; private set; }


    public Card(CardData data)
    {
        this.data = data;
        Mana = data.Mana;
    }
}
