using SerializeReferenceEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Data", menuName = "Data/Card Data", order = 0)]
public class CardData : ScriptableObject
{
    [field:SerializeField]public string Des { get; private set; }
    [field: SerializeField] public int Mana { get; private set; }
    [field: SerializeField] public string CardName { get; private set; }
    [field: SerializeField] public Sprite CardImage { get; private set; }
    [field: SerializeReference,SR] public List<Effect> Effects { get; private set; }

}
