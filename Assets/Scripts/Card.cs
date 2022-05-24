using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public bool hasBeenPlayed;
    public int handIndex;

    private CardManager cm;

    private void Start()
    {
        cm = FindObjectOfType<CardManager>();
    }

    private void OnMouseDown()
    {
        if (hasBeenPlayed == false) {
            transform.position += Vector3.up * 5;
            hasBeenPlayed = true;
            cm.availableCardSlots[handIndex] = true;
            Invoke("MoveToDiscardPile", 2f); // Discard card after 2 seconds NOTE: We will change this to be done after a button press in the future
        }
    }

    void MoveToDiscardPile() {
        cm.discardPile.Add(this);
        gameObject.SetActive(false);
        cm.discardSizeText.text = cm.discardPile.Count.ToString(); //Update discard pile size text

    }
}
