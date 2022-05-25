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
    public void DrawCard() 
    {
        if (deck.Count >= 1)
        {
            Card topCard = deck[0];      // Draws top card

            for (int i = 0; i < availableCardSlots.Length; i++)
            {
                if (availableCardSlots[i] == true)
                {
                    topCard.gameObject.SetActive(true);
                    topCard.handIndex = i;

                    topCard.transform.position = cardSlots[i].position;
                    topCard.hasBeenPlayed = false;

                    availableCardSlots[i] = false;
                    deck.Remove(topCard);
                    deckSizeText.text = deck.Count.ToString(); //Update deck size text
                    return;
                }
            }
        }
        else
        {
            DiscardtoDeck();
            ShuffleDeck();
        }
    }

    // Shuffle the Deck
    public void ShuffleDeck()
    {
        if (deck.Count >= 1)
        {
            System.Random random = new System.Random();

            for (int i = 0; i < deck.Count; i++)
            {
                int j = random.Next(i, deck.Count);
                Card temporary = deck[i];
                deck[i] = deck[j];
                deck[j] = temporary;
            }
        }
    }

    // Move the Discard to the Deck
    public void DiscardtoDeck() 
    {
        if(discardPile.Count >= 1)
        {
            foreach(Card card in discardPile)
            {
                deck.Add(card);
            }
        }
    }

    private void Start()
    {
        deckSizeText.text = deck.Count.ToString(); //Update deck size text
        discardSizeText.text = discardPile.Count.ToString(); //Update discard pile size text
        ShuffleDeck();
    }
}
