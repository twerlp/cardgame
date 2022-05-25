using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardManager : MonoBehaviour
{
    public List<Card> deck = new List<Card>();          // List for deck
    public List<Card> discardPile = new List<Card>();   // List for discard

    public Transform[] cardSlots;                       // For placing cards neatly in hand
    public bool[] availableCardSlots;                   // Checking if there are any slots left in the hand

    public TMP_Text deckSizeText;                       // User feedback, lets them know how many cards left in deck
    public TMP_Text discardSizeText;                    // User feedback, lets them know how many cards left in discard

    //Draw a card and remove it from deck.
    public void DrawCard() {
        if (deck.Count >= 1) {
            Card randCard = deck[Random.Range(0, deck.Count)];      // Draws a random card, could change to just first card. (Shuffle first.)

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
