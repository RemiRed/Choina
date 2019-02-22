using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceScript : MonoBehaviour
{
    ~PieceScript()
    {
        print("byeee");
        if (nodes != null)
            nodes.piece = null;
    }

    public NodeScript nodes;
    public Colour collr;
    public int playa;

    public void ChangeColor()
    {
        switch (collr)
        {
            case Colour.red:
                gameObject.GetComponent<Renderer>().material.color = Color.red;
                break;
            case Colour.green:
                gameObject.GetComponent<Renderer>().material.color = Color.green;
                break;
            case Colour.blue:
                gameObject.GetComponent<Renderer>().material.color = Color.blue;
                break;
            case Colour.black:
                gameObject.GetComponent<Renderer>().material.color = Color.black;
                break;
            case Colour.yellow:
                gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                break;
            case Colour.white:
                gameObject.GetComponent<Renderer>().material.color = Color.magenta;
                break;
        }
    }
}
