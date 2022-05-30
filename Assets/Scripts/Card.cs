using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    public bool hasBeenClicked;         // Check if card is ready to be played
    public int handIndex;               // Where card is in hand

                                        // For visuals on Card
    public TMP_Text strengthTMP;        //
    public TMP_Text aoeTMP;             //
    public TMP_Text movementTMP;        //
    public TMP_Text healthTMP;          //
    public TMP_Text descriptTMP;        //

    public GameObject strengthIcon;
    public GameObject aoeIcon;
    public GameObject movementIcon;
    public GameObject healthIcon;
    public GameObject background;

    private CardManager cm;             // The CardManager for the arena
    public CardData cardData;           // Helps display all card-specific data / give cards functionality


    private void Start()
    {
        cm = FindObjectOfType<CardManager>();
    }

    public void UpdateText() { // Used to update all the text information on the card (UPDATE TO REMOVE / ADD ICONS DEPENDING ON IF THERE IS A STAT OR NOT!)
        strengthTMP.text = cardData.strength.ToString();
        aoeTMP.text = cardData.aoe.ToString();
        movementTMP.text = cardData.movement.ToString();
        healthTMP.text = cardData.health.ToString();

        if (cardData.cardEffects.Count > 0) descriptTMP.text = "";
        foreach (CardEffect effect in cardData.cardEffects)
            descriptTMP.text = effect.CardDescription() + "\n" + descriptTMP.text;

    }

    public void Clicked() //replace this area with new logic for clicking
    {
        if (hasBeenClicked == false) {
            hasBeenClicked = true;
            transform.position += Vector3.up * 2;
        }
        else if (hasBeenClicked == true) {
            hasBeenClicked = false;
            transform.position -= Vector3.up * 2;
        }
    }

    public void Play() { // Flesh out with playing logic
        cm.availableCardSlots[handIndex] = true;
        foreach (CardEffect effect in cardData.cardEffects)
            effect.ApplyEffect();
        Invoke("MoveToDiscardPile", 2f); // Discard card after 2 seconds NOTE: We will change this to be done after a button press in the future
    }

    // Move the Card to the discard pile after playing it.
    void MoveToDiscardPile() {
        cm.discardPile.Add(this);
        cm.hand.Remove(this);
        gameObject.SetActive(false);
        cm.discardSizeText.text = cm.discardPile.Count.ToString(); // Update discard pile size text
    }

}
