using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private HandView handView;
    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CardView card = CardViewCreator.Instance.CreateCardView(transform.position, transform.rotation);
            StartCoroutine(handView.AddCard(card));
        }
    }
}
