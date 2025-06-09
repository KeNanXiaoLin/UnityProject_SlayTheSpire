using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Data", menuName = "Data/Card Data", order = 0)]
public class CardData : ScriptableObject
{
    public string des;
    public int mana;
    public string cardName;
    public Sprite cardImage;

}
