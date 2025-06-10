using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSetUpSystem : MonoBehaviour
{
    [SerializeField] private HeroData heroData;

    private void Start()
    {
        HeroSystem.Instance.SetUp(heroData);
        CardSystem.Instance.SetUp(heroData.Deck);
        DrawCardGA drawCardGA = new DrawCardGA(5);
        ActionSystem.Instance.Perform(drawCardGA, () =>
        {
            Debug.Log("Draw Card Action Finished");
        });
    }
}
