using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardManager : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public List<Card> discardPile = new List<Card>();

    public Transform[] cardSlots;
    public bool[] availableCardSlots;

    public TMP_Text deckSizeText;
    public TMP_Text discardSizeText;

    //Draw a card
    public void DrawCard() {
        if (deck.Count >= 1) {
            Card randCard = deck[Random.Range(0, deck.Count)];

            for (int i = 0; i < availableCardSlots.Length; i++) {
                if (availableCardSlots[i] == true) {
                    randCard.gameObject.SetActive(true);
                    randCard.handIndex = i;

                    randCard.transform.position = cardSlots[i].position;
                    availableCardSlots[i] = false;
                    deck.Remove(randCard);
                    deckSizeText.text = deck.Count.ToString(); //Update deck size text
                    return;
                }
            }
        }
    }
    private void Start()
    {
        deckSizeText.text = deck.Count.ToString(); //Update deck size text
        discardSizeText.text = discardPile.Count.ToString(); //Update discard pile size text
    }
}
