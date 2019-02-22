using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//enum over the different colors the checkers is going to use
public enum Colour { none, red, yellow, white, green, blue, black };
//Adjecent nodes.
public class AdjecentNeighbours
{
    NodeScript Nodes;
    Dir dire;

    // We want to get and return the current nodes
    public NodeScript nodes
    {
        get { return this.Nodes; }
    }
    public Dir Dire
    {
        get { return this.dire; }
    }


    public AdjecentNeighbours(NodeScript nod, Dir dire)
    {
        this.Nodes = nod;
        this.dire = dire;
    }
}

public class NodeScript : MonoBehaviour
{

    public PieceScript piece;

    public Colour clr;

    // Creates a collection of nodes.
    public List<AdjecentNeighbours> addNode = new List<AdjecentNeighbours>();
    // Method to create the neighbours to this tile. 
    public void addNodes(NodeScript node, Dir dir)
    {
        addNode.Add(new AdjecentNeighbours(node, dir));
    }
}
