using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class HandView : MonoBehaviour
{
    //场景中设置好的Spline容器
    [SerializeField] private SplineContainer splineContainer;
    //所有的手牌
    private readonly List<CardView> cards = new();

    /// <summary>
    /// 添加卡牌的协程
    /// </summary>
    /// <param name="card">要添加的卡牌对象</param>
    /// <returns></returns>
    public IEnumerator AddCard(CardView card)
    {
        cards.Add(card);
        yield return UpdateCardPosition(0.15f);
    }

    public CardView RemoveCard(Card card)
    {
        CardView cardView = GetCard(card);
        if (cardView == null) return null;
        cards.Remove(cardView);
        StartCoroutine(UpdateCardPosition(0.15f));
        return cardView;
    }

    private CardView GetCard(Card card)
    {
        return cards.Where(cardView => cardView.Card == card).FirstOrDefault();
    }

    /// <summary>
    /// 更新卡牌位置
    /// </summary>
    /// <param name="time">平滑时间</param>
    /// <returns></returns>
    public IEnumerator UpdateCardPosition(float time)
    {
        if(cards.Count ==0) yield break;
        //这里最多允许10张牌，根据Spline在0-1上的插值计算出每张牌所在的位置
        float cardSpacing = 1f / 10f;
        //计算第一张牌的位置，因为我们希望牌的位置保持一定的对称性
        //所以第一张牌的位置=中间的位置-(卡牌的数量-1）/2*卡牌的间距
        float firstCardPosition = 0.5f-(cards.Count-1)*cardSpacing/2;
        Spline spline = splineContainer.Spline;
        //计算每一张牌的位置
        for (int i = 0; i < cards.Count; i++)
        {
            float p = firstCardPosition + i * cardSpacing;
            //这个方法传入的上一个0-1的值，返回在这个比例上曲线的位置
            Vector3 splinePosition = spline.EvaluatePosition(p);
            //这个方法传入的上一个0-1的值，返回在这个比例上切线的方向向量
            Vector3 forward = spline.EvaluateTangent(p);
            //这个方法传入的上一个0-1的值，返回在这个比例上法线的方向向量
            //这里计算出来好像始终是(0,0,-1),是指向z轴的一个向量
            Vector3 up = spline.EvaluateUpVector(p);
            //通过LookRotation方法，使用叉乘得到卡牌Y轴的朝向
            //Quaternion.LookRotation，第一个参数是面朝向，第二个参数是向上的朝向
            Quaternion rotation = Quaternion.LookRotation(-up, Vector3.Cross(-up, forward).normalized);
            //使用DoTween进行动画平滑过渡
            //第一个参数：目标值
            //第二个参数：平滑时间
            cards[i].transform.DOMove(splinePosition + transform.position + 0.01f * i * Vector3.back, time);
            cards[i].transform.DORotate(rotation.eulerAngles, time);
        }
        yield return new WaitForSeconds(time);
    }
}
