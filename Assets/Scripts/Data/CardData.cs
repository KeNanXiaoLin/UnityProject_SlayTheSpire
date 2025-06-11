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
    [field: SerializeReference, SR] public Effect ManualTargetEffect { get; private set; } = null;
    [field: SerializeField] public List<AutoTargetEffect> OtherEffects { get; private set; } = null;

}
