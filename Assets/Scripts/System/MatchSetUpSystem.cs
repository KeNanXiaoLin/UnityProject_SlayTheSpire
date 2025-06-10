using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSetUpSystem : MonoBehaviour
{
    [SerializeField] private List<CardData> cards;

    private void Start()
    {
        CardSystem.Instance.SetUp(cards);
        DrawCardGA drawCardGA = new DrawCardGA(5);
        ActionSystem.Instance.Perform(drawCardGA, () =>
        {
            Debug.Log("Draw Card Action Finished");
        });
    }
}
