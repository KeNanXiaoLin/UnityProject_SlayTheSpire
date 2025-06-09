using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardViewHover : SingletonMono<CardViewHover>
{
    [SerializeField] private CardView hoverGo;

    public void Show(CardView view)
    {
        hoverGo.gameObject.SetActive(true);
        hoverGo.SetUp(view.Card);
        hoverGo.gameObject.transform.position = view.transform.position;
        hoverGo.gameObject.transform.rotation = view.transform.rotation;
    }

    public void Hide()
    {
        hoverGo.gameObject.SetActive(false);
    }
}
