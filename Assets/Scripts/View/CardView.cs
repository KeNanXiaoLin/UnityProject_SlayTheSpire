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
    [SerializeField] private LayerMask dropAreaLayer;

    public Card Card { get; private set; }

    private Vector3 cardStartPos;
    private Quaternion cardStartRot;

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
        if (!Interactions.Instance.PlayerCanHover()) return;
        CardViewHover.Instance.Show(this);
        Wrapper.SetActive(false);
    }

    private void OnMouseExit()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        CardViewHover.Instance.Hide();
        Wrapper.SetActive(true);
    }

    private void OnMouseDown()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        Interactions.Instance.PlayerIsDragging = true;
        Wrapper.SetActive(true);
        CardViewHover.Instance.Hide();
        cardStartPos = transform.position;
        Debug.Log("记录卡牌初始位置" + cardStartPos);
        cardStartRot = transform.rotation;
        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1f);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void OnMouseDrag()
    {
        //这里存在的bug，如果我在发牌的过程中，鼠标已经按下了卡牌，那么在发牌完成后，
        //会直接执行OnMouseDrag方法而不会执行OnMouseDown方法，这会导致初始的位置没有记录，
        //导致下面执行OnMouseUp方法的时候使用错误的初始位置。
        //修复bug：必须要保证OnMouseDown方法执行在OnMouseDrag方法之前
        //添加判断!Interactions.Instance.PlayerIsDragging,玩家不处于拖曳状态时，直接返回
        if (!Interactions.Instance.PlayerCanInteract() || 
            !Interactions.Instance.PlayerIsDragging) return;
        Debug.Log("正在拖动卡牌");
        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1f);
    }

    private void OnMouseUp()
    {
        //这里也是同样的bug，必须要执行OnMouseDown、OnMouseDrag之后才能执行OnMouseUp方法，
        //否则会导致初始位置没有记录，导致放下卡牌时位置错误。
        if (!Interactions.Instance.PlayerCanInteract() ||
            !Interactions.Instance.PlayerIsDragging) return;
        if (ManaSystem.Instance.HasEnoughMana(Card.Mana) &&
            Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 100f,dropAreaLayer))
        {
            //Play Card
            PlayCardGA playCard = new PlayCardGA(Card);
            ActionSystem.Instance.Perform(playCard);
        }
        else
        {
            transform.position = cardStartPos;
            transform.rotation = cardStartRot;
            Debug.Log("放下卡牌"+ cardStartPos);
        }
        Interactions.Instance.PlayerIsDragging = false;
    }
}
