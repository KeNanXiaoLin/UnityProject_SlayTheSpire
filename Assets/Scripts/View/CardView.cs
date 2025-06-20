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
        //由于前面修改了逻辑，所以这里存在一定的问题
        //问题描述：这里是否能够悬浮显示的关键点是PlayerIsDragging
        //如果玩家正在拖曳卡牌，那么这里就不应该显示悬浮提示，如果没有拖曳卡牌，那么就应该显示悬浮提示
        //但是由于我在OnMouseDown中修改了逻辑，直到OnMouseUp才会将PlayerIsDragging设置为false，
        //所以在第一次执行OnMouseExit方法时，PlayerIsDragging仍然为true，所以不会隐藏显示
        //在执行OnMouseUp方法时，PlayerIsDragging被设置为false，这时不会执行OnMouseExit方法，所以不会隐藏显示
        //解决方案：将隐藏悬浮的逻辑封装成一个方法，
        //在OnMouseUp中调用这个方法来隐藏悬浮提示
        //在OnMouseExit方法中调用这个方法来隐藏悬浮提示
        //这里再说一下为什么没有目标的卡牌和有目标的卡牌都可以正常悬浮显示
        //因为这两张卡牌都会跟着鼠标移动，所以执行OnMouseExit的时机应该在OnMouseUp之后
        //并且因为执行OnMouseDown，OnMouseDrag，OnMouseUp这3个方法时，我们将PlayerIsDragging置为true
        //所有也不会进不了OnMouseExit执行逻辑，只有在OnMouseUp之后，将PlayerIsDragging置为false
        //才会执行OnMouseExit方法中的逻辑
        //鼠标已经不在卡牌上了，所以会执行OnMouseEnter方法来显示悬浮提示
        if (!Interactions.Instance.PlayerCanHover()) return;
        HideHover();
    }

    private void HideHover()
    {
        CardViewHover.Instance.Hide();
        Wrapper.SetActive(true);
    }

    private void OnMouseDown()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        Interactions.Instance.PlayerIsDragging = true;
        if (Card.ManualTargetEffect !=null)
        {
            ManualTargetSystem.Instance.StartTargeting(transform.position);
        }
        else
        {
            Wrapper.SetActive(true);
            CardViewHover.Instance.Hide();
            cardStartPos = transform.position;
            cardStartRot = transform.rotation;
            transform.position = MouseUtil.GetMousePositionInWorldSpace(-1f);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
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
        if (Card.ManualTargetEffect != null) return;
        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1f);
    }

    private void OnMouseUp()
    {
        //这里也是同样的bug，必须要执行OnMouseDown、OnMouseDrag之后才能执行OnMouseUp方法，
        //否则会导致初始位置没有记录，导致放下卡牌时位置错误。
        if (!Interactions.Instance.PlayerCanInteract() ||
            !Interactions.Instance.PlayerIsDragging) return;
        
        if (Card.ManualTargetEffect !=null)
        {
            EnemyView target = ManualTargetSystem.Instance.EndTargeting(MouseUtil.GetMousePositionInWorldSpace(-1));
            if(target != null && ManaSystem.Instance.HasEnoughMana(Card.Mana))
            {
                PlayCardGA playCard = new PlayCardGA(Card, target);
                ActionSystem.Instance.Perform(playCard);
            }
        }
        else
        {
            if (ManaSystem.Instance.HasEnoughMana(Card.Mana) &&
            Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 100f, dropAreaLayer))
            {
                //Play Card
                PlayCardGA playCard = new PlayCardGA(Card);
                ActionSystem.Instance.Perform(playCard);
            }
            else
            {
                transform.position = cardStartPos;
                transform.rotation = cardStartRot;
            }
        }
        Interactions.Instance.PlayerIsDragging = false;
        HideHover();
    }
}
