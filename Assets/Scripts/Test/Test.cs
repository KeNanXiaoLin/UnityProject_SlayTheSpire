using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private List<CardData> cards;

    private void Start()
    {
        CardSystem.Instance.SetUp(cards);
    }
}
