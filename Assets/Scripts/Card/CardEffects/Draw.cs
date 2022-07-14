using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Draw : CardEffect
{
    public int num;
    private CardManager cm;

    public override void ApplyEffect()
    {
        cm = FindObjectOfType<CardManager>();
        for (int i = 0; i < num; i++)
        {
            cm.DrawCard();
        }
    }

    public override string CardDescription()
    {
        return string.Format(effectText, num);
    }
}
