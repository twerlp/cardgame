using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer myRenderer;
    [SerializeField] private GameObject highlighted;


    //used to make offset colors
    public void Init(bool isOffset)
    {
        myRenderer.color = isOffset ? offsetColor : baseColor;
    }



    void OnMouseEnter()
    {
        highlighted.SetActive(true);
    }


    void OnMouseExit()
    {
        highlighted.SetActive(false);
    }





}
