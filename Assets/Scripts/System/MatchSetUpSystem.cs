using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSetUpSystem : MonoBehaviour
{
    [SerializeField] private HeroData heroData;
    [SerializeField] private List<EnemyData> enemyData;

    private void Start()
    {
        HeroSystem.Instance.SetUp(heroData);
        EnemySystem.Instance.SetUp(enemyData);
        CardSystem.Instance.SetUp(heroData.Deck);
        DrawCardGA drawCardGA = new DrawCardGA(5);
        ActionSystem.Instance.Perform(drawCardGA, () =>
        {
            Debug.Log("Draw Card Action Finished");
        });
    }
}
