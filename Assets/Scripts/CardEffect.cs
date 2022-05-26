using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardEffect : MonoBehaviour
{
    [TextArea(2, 5)]
    public string effectText;               // Text for description
    public bool targetsActiveCharacter;     // Whether or not it affects the self

    public virtual void ApplyEffect()
    {
    }

    public virtual string CardDescription()
    {
        return "";
    }

}
