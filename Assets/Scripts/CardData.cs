using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : MonoBehaviour
{
                            // Each type of effect
    public int strength;    //
    public int aoe;         //
    public int movement;    //
    public int health;      //

    public List<CardEffect> cardEffects = new List<CardEffect>;
}
