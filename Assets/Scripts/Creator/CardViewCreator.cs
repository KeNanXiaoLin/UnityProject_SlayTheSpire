using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardViewCreator : SingletonMono<CardViewCreator>
{
    [SerializeField] private CardView cardView;

    public CardView CreateCardView(Card card,Vector3 position,Quaternion rotation)
    {
        CardView cardObj = Instantiate(cardView, position, rotation);
        cardObj.transform.localScale = Vector3.zero;
        cardObj.transform.DOScale(Vector3.one, 0.15f);
        cardObj.SetUp(card);
        return cardObj;
    }
}
