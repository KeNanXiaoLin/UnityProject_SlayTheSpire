using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private HandView handView;
    [SerializeField] private CardData cardData;
    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Card card = new Card(cardData);
            CardView spawn_card = CardViewCreator.Instance.CreateCardView(card,transform.position, transform.rotation);
            StartCoroutine(handView.AddCard(spawn_card));
        }
    }
}
