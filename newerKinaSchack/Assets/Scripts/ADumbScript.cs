using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum B { n, e, r, y, w, g, b, d };

public class ADumbScript : MonoBehaviour
{

    // Board state, every position the board has with with the positions of the colors
    B[,] board2;

    B[,] board = new B[17, 17]
{
        {B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.r,B.n,B.n,B.n,B.n},
        {B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.r,B.r,B.n,B.n,B.n,B.n},
        {B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.r,B.r,B.r,B.n,B.n,B.n,B.n},
        {B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.r,B.r,B.r,B.r,B.n,B.n,B.n,B.n},
        {B.n,B.n,B.n,B.n,B.y,B.y,B.y,B.y,B.e,B.e,B.e,B.e,B.e,B.d,B.d,B.d,B.d},
        {B.n,B.n,B.n,B.n,B.y,B.y,B.y,B.e,B.e,B.e,B.e,B.e,B.e,B.d,B.d,B.d,B.n},
        {B.n,B.n,B.n,B.n,B.y,B.y,B.e,B.e,B.e,B.e,B.e,B.e,B.e,B.d,B.d,B.n,B.n},
        {B.n,B.n,B.n,B.n,B.y,B.e,B.e,B.e,B.e,B.e,B.e,B.e,B.e,B.d,B.n,B.n,B.n},
        {B.n,B.n,B.n,B.n,B.e,B.e,B.e,B.e,B.e,B.e,B.e,B.e,B.e,B.n,B.n,B.n,B.n},
        {B.n,B.n,B.n,B.w,B.e,B.e,B.e,B.e,B.e,B.e,B.e,B.e,B.b,B.n,B.n,B.n,B.n},
        {B.n,B.n,B.w,B.w,B.e,B.e,B.e,B.e,B.e,B.e,B.e,B.b,B.b,B.n,B.n,B.n,B.n},
        {B.n,B.w,B.w,B.w,B.e,B.e,B.e,B.e,B.e,B.e,B.b,B.b,B.b,B.n,B.n,B.n,B.n},
        {B.w,B.w,B.w,B.w,B.e,B.e,B.e,B.e,B.e,B.b,B.b,B.b,B.b,B.n,B.n,B.n,B.n},
        {B.n,B.n,B.n,B.n,B.g,B.g,B.g,B.g,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n},
        {B.n,B.n,B.n,B.n,B.g,B.g,B.g,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n},
        {B.n,B.n,B.n,B.n,B.g,B.g,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n},
        {B.n,B.n,B.n,B.n,B.g,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n,B.n}
};
    public B[,] Board2
    {
        set

        {
            if (value != null)
            {
                board2 = value;
            }

        }
    }
    // a list over the boards nodes and the pieces the player has.

    public NodeScript[,] rBoard = new NodeScript[17, 17];
    public PieceScript[,] Rpiece = new PieceScript[17, 17];

    public GameObject piece;
    public GameObject node;

    // Lists over the ammount of different pieces and different color

    public List<PieceScript> redPic = new List<PieceScript>();
    public List<PieceScript> bluePic = new List<PieceScript>();
    public List<PieceScript> yellowPic = new List<PieceScript>();
    public List<PieceScript> greenPic = new List<PieceScript>();
    public List<PieceScript> whitePic = new List<PieceScript>();
    public List<PieceScript> blackPic = new List<PieceScript>();

    // The form of the board.

    int[] boardForm = new int[17] { 1, 2, 3, 4, 13, 12, 11, 10, 9, 10, 11, 12, 13, 4, 3, 2, 1 };
    GameObject[,] nodes = new GameObject[17, 13];
    GameObject[,] pieces = new GameObject[17, 13];

    int players;


    // The realstart gets the parent component from Movescript and assign the ammount of players.

    public void RealStart()
    {
        players = GetComponentInParent<MoveScript>().numberOfPlayers;
        ExternalBoard();
    }
    // 
    public B[,] CreateSaveData()
    {
        B[,] berb = new B[17, 17];
        for (int i = 0; i < Rpiece.GetLength(0); i++)
        {
            for (int j = 0; j < Rpiece.GetLength(1); j++)
            {
                if (rBoard[i, j] != null)
                {
                    if (rBoard[i, j].piece != null)
                    {
                        switch (rBoard[i, j].piece.collr)
                        {
                            case Colour.black:
                                berb[i, j] = B.d;
                                break;
                            case Colour.red:
                                berb[i, j] = B.r;
                                break;
                            case Colour.yellow:
                                berb[i, j] = B.y;
                                break;
                            case Colour.white:
                                berb[i, j] = B.w;
                                break;
                            case Colour.green:
                                berb[i, j] = B.g;
                                break;
                            case Colour.blue:
                                berb[i, j] = B.b;
                                break;
                        }
                    }

                }
            }
        }
        return (berb);
    }


    //The visual part of the game.
    public void ExternalBoard()
    {
        for (int i = 0; i < boardForm.Length; i++)
        {
            for (int j = 0; j < boardForm[i]; j++)
            {
                nodes[i, j] = Instantiate(node, new Vector3(-(boardForm[i] + 1) + j * 2, 0, -16 + i * 2), Quaternion.identity);
            }
        }

        // the internal part, everything that happends within the code.
        InternalBoard();
        
        for (int i = 0; i < nodes.GetLength(0); i++)

        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            {
                if (nodes[i, j] != null && nodes[i, j].GetComponent<NodeScript>().clr != Colour.none)
                {
                    pieces[i, j] = Instantiate(piece, new Vector3(nodes[i, j].transform.position.x, 0, nodes[i, j].transform.position.z), Quaternion.identity);
                }


            }
        }




        // calling the methods to the external board.
        InternalPieces();
        CreatePlayers();
        FindNodes();

    }

    // Just a method that creates the players. Depending on the ammount, certian cases with the correct layout will be shown.

    void CreatePlayers()
    {
        List<int> thesePlayers = new List<int>();
        switch (players)
        {
            case 2:
                thesePlayers.Add(1);
                thesePlayers.Add(2);
                RemovePlayers(thesePlayers);
                break;
            case 3:
                thesePlayers.Add(1);
                thesePlayers.Add(6);
                thesePlayers.Add(4);
                RemovePlayers(thesePlayers);
                break;
            case 4:
                thesePlayers.Add(1);
                thesePlayers.Add(2);
                thesePlayers.Add(6);
                thesePlayers.Add(5);
                RemovePlayers(thesePlayers);
                break;
            default:
                break;
        }
    }

    void RemovePlayers(List<int> wichOne)
    {
        for (int i = 0; i < pieces.GetLength(0); i++)
        {
            for (int j = 0; j < pieces.GetLength(1); j++)
            {
                if (pieces[i, j] != null)
                {

                    if (!wichOne.Contains(pieces[i, j].GetComponent<PieceScript>().playa))
                    {
                        nodes[i, j].GetComponent<NodeScript>().piece = null;
                        Destroy(pieces[i, j]);
                    }
                }

            }

        }
    }
    // The code "game", this creates the board within the code so whenever the player visually sees the board one is also created at the same time.  
    void InternalBoard()
    {
        int counting;
        for (int i = 0; i < board.GetLength(0); i++)
        {
            counting = 0;
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] != B.n)
                {
                    // This part paint each node.
                    switch (board[i, j])
                    {
                        case B.e:
                            rBoard[i, j] = nodes[i, counting].GetComponent<NodeScript>();
                            counting++;
                            break;
                        case B.r:
                            rBoard[i, j] = nodes[i, counting].GetComponent<NodeScript>();
                            rBoard[i, j].clr = Colour.red;
                            counting++;
                            break;
                        case B.b:
                            rBoard[i, j] = nodes[i, counting].GetComponent<NodeScript>();
                            rBoard[i, j].clr = Colour.blue;
                            counting++;
                            break;
                        case B.g:
                            rBoard[i, j] = nodes[i, counting].GetComponent<NodeScript>();
                            rBoard[i, j].clr = Colour.green;
                            counting++;
                            break;
                        case B.d:
                            rBoard[i, j] = nodes[i, counting].GetComponent<NodeScript>();
                            rBoard[i, j].clr = Colour.black;
                            counting++;
                            break;
                        case B.y:
                            rBoard[i, j] = nodes[i, counting].GetComponent<NodeScript>();
                            rBoard[i, j].clr = Colour.yellow;
                            counting++;
                            break;
                        case B.w:
                            rBoard[i, j] = nodes[i, counting].GetComponent<NodeScript>();
                            rBoard[i, j].clr = Colour.white;
                            counting++;
                            break;
                    }
                }
            }
        }
    }
    // Same as the internal board, this puts the pieces in each corresponding node. 
    void InternalPieces()
    {
        B[,] board2use;
        if (board2 != null)
        {
            board2use = board2;
        }
        else
        {
            board2use = board;
        }
        int counting;
        for (int i = 0; i < rBoard.GetLength(0); i++)
        {
            counting = 0;
            for (int j = 0; j < rBoard.GetLength(1); j++)
            {
                if (counting > 12)
                    counting = 12;
                if (rBoard[i, j] != null)
                {
                    if (rBoard[i, j].clr != Colour.none)
                    {
                        // This part paints each piece. 
                        switch (board2use[i, j])
                        {
                            case B.r:
                                Rpiece[i, j] = pieces[i, counting].GetComponent<PieceScript>();
                                CreatePieces(Colour.red, 1, Rpiece[i, j], rBoard[i, j]);
                                redPic.Add(Rpiece[i, j]);
                                break;
                            case B.g:
                                Rpiece[i, j] = pieces[i, counting].GetComponent<PieceScript>();
                                CreatePieces(Colour.green, 2, Rpiece[i, j], rBoard[i, j]);
                                greenPic.Add(Rpiece[i, j]);
                                break;
                            case B.y:
                                Rpiece[i, j] = pieces[i, counting].GetComponent<PieceScript>();
                                CreatePieces(Colour.yellow, 3, Rpiece[i, j], rBoard[i, j]);
                                yellowPic.Add(Rpiece[i, j]);
                                break;
                            case B.d:
                                Rpiece[i, j] = pieces[i, counting].GetComponent<PieceScript>();
                                CreatePieces(Colour.black, 5, Rpiece[i, j], rBoard[i, j]);
                                blackPic.Add(Rpiece[i, j]);
                                break;
                            case B.w:
                                Rpiece[i, j] = pieces[i, counting].GetComponent<PieceScript>();
                                CreatePieces(Colour.white, 6, Rpiece[i, j], rBoard[i, j]);
                                whitePic.Add(Rpiece[i, j]);
                                break;
                            case B.b:
                                Rpiece[i, j] = pieces[i, counting].GetComponent<PieceScript>();
                                CreatePieces(Colour.blue, 4, Rpiece[i, j], rBoard[i, j]);
                                bluePic.Add(Rpiece[i, j]);
                                break;
                        }
                    }
                }
                if (rBoard[i, j] != null)
                    counting++;
            }
        }
    }

    // This method finds a single nodes corresponding neighbour. 
    void FindNodes()
    {
        for (int i = 0; i < rBoard.GetLength(0); i++)
        {
            for (int j = 0; j < rBoard.GetLength(1); j++)
            {
                if (rBoard[i, j] != null)
                {
                    NodeScript nde = rBoard[i, j];
                    if (j > 0)
                    {
                        if (rBoard[i, j - 1] != null)
                            nde.addNodes(rBoard[i, j - 1], Dir.W);
                    }
                    if (j < rBoard.GetLength(1) - 1)
                    {
                        if (rBoard[i, j + 1] != null)
                            nde.addNodes(rBoard[i, j + 1], Dir.E);
                    }
                    if (i > 0)
                    {
                        if (rBoard[i - 1, j] != null)
                            nde.addNodes(rBoard[i - 1, j], Dir.NW);
                        if (j < rBoard.GetLength(1) - 1)
                        {
                            if (rBoard[i - 1, j + 1] != null)
                                nde.addNodes(rBoard[i - 1, j + 1], Dir.NE);
                        }

                    }
                    if (i < rBoard.GetLength(0) - 1)
                    {
                        if (rBoard[i + 1, j] != null)
                            nde.addNodes(rBoard[i + 1, j], Dir.SE);
                        if (j > 0)
                        {
                            if (rBoard[i + 1, j - 1] != null)
                                nde.addNodes(rBoard[i + 1, j - 1], Dir.SW);
                        }
                    }
                }
            }
        }
    }
    // Creates pieces. 

    void CreatePieces(Colour colar, int playyr, PieceScript piece, NodeScript node)
    {
        piece.collr = colar;
        piece.playa = playyr;
        piece.nodes = node;
        node.piece = piece;
        piece.ChangeColor();
    }
}


