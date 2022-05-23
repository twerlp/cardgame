using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer myRenderer;
    [SerializeField] private GameObject highlighted;
    [SerializeField] private int x, y;


    //used to make offset colors
    public void Init(bool isOffset, int xVal, int yVal)
    {
        myRenderer.color = isOffset ? offsetColor : baseColor;
        x = xVal;
        y = yVal;
    }



    void OnMouseEnter()
    {
        highlighted.SetActive(true);
    }


    void OnMouseExit()
    {
        highlighted.SetActive(false);
    }


    void OnMouseDown()
    {
        Debug.Log("(" + x + ", " + y +")");
    }


}
