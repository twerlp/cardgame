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

    public void Clicked() // When the player clicks the card (active / inactive)
    {
        if (hasBeenClicked == false) {
            hasBeenClicked = true;
            transform.position += Vector3.up * 2;
            cm.activeCard.Add(this);
        }
        else if (hasBeenClicked == true) {
            hasBeenClicked = false;
            transform.position -= Vector3.up * 2;
            cm.activeCard.Remove(this);
        }
    }

    public void Play() { // Plays the card, provides effects.
        cm.availableCardSlots[handIndex] = true;
        Invoke("MoveToDiscardPile", 2f); // Discard card after 2 seconds NOTE: We will change this to be done after a button press in the future
        foreach (CardEffect effect in cardData.cardEffects)
            effect.ApplyEffect();
    }

    // Move the Card to the discard pile after playing it.
    void MoveToDiscardPile() {
        cm.discardPile.Add(this);
        cm.activeCard.Remove(this);
        gameObject.SetActive(false);
        cm.UpdateUI();
    }

}
