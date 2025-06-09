using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text mana;
    [SerializeField] private TMP_Text des;
    [SerializeField] private TMP_Text title;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject Wrapper;

    public Card Card { get; private set; }

    public void SetUp(Card card)
    {
        this.Card = card;
        title.text = card.Title;
        des.text = card.Des;
        mana.text = card.Mana.ToString();
        spriteRenderer.sprite = card.Image;
    }

    private void OnMouseEnter()
    {
        CardViewHover.Instance.Show(this);
        Wrapper.SetActive(false);
    }

    private void OnMouseExit()
    {
        CardViewHover.Instance.Hide();
        Wrapper.SetActive(true);
    }
}
