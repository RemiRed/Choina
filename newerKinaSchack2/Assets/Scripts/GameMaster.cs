using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//n = null, e = empty, r = red, y = yellow, w = white, g = green, b = blue, d = black

public enum Dir { E, NE, SE, W, NW, SW };

public class GameMaster : MonoBehaviour
{
    int players;
    public List<PLayer> pLayers = new List<PLayer>();
    public List<Colour> deColor = new List<Colour>();
    public Board board;
    Minimax maxi;
    public int depth = 1;
    // Whenever the game starts the game calls all the components it needs.
    public void RealStart()
    {
        maxi = new Minimax();
        players = GetComponentInParent<MoveScript>().numberOfPlayers;
        board = new Board();
        CreatePlayers();
        board.CreateBoard();
        board.CreatePieces(players, deColor);
        board.FindNodes();
        foreach (PLayer plr in pLayers)
            board.HardKodedmal(plr);
        FindPlayersPieces();
        board.CreateCoordinates();
    }
    // Method that checks whos turn it is. 
    public void ComPlayerTurn(int thisPlayr)
    {
        List<PLayer> othPlyrs = new List<PLayer>();
        PLayer ccurplur = null;
        foreach (PLayer plrs in pLayers)
        {
            if (plrs.thisPlayer != thisPlayr)
                othPlyrs.Add(plrs);
            if (plrs.thisPlayer == thisPlayr)
                ccurplur = plrs;
        }
        maxi.miniMax(ccurplur, othPlyrs, board, depth);
    }

    void CreatePlayers()
    {
        switch (players)
        {
            case 2:
                deColor.Add(Colour.red);
                deColor.Add(Colour.green);
                break;
            case 3:
                deColor.Add(Colour.red);
                deColor.Add(Colour.blue);
                deColor.Add(Colour.white);
                break;
            case 4:
                deColor.Add(Colour.red);
                deColor.Add(Colour.black);
                deColor.Add(Colour.green);
                deColor.Add(Colour.white);
                break;
            case 6:
                deColor.Add(Colour.red);
                deColor.Add(Colour.black);
                deColor.Add(Colour.blue);
                deColor.Add(Colour.green);
                deColor.Add(Colour.white);
                deColor.Add(Colour.yellow);
                break;
        }
        for (int i = 0; i < players; i++)
        {
            pLayers.Add(new PLayer() { thisPlayer = i + 1, siColr = deColor[i] });
        }
    }
    void FindPlayersPieces()
    {
        foreach (PLayer playa in pLayers)
        {
            foreach (Piece pic in board.allPieces)
            {
                if (pic.player == playa.thisPlayer)
                    playa.myPieces.Add(pic);
            }
        }

    }
    // Wincondition
    public int Win()
    {
        foreach (PLayer plris in pLayers)
        {
            int winCon = new int();
            foreach (Node nids in plris.goals)
            {
                if (nids.piece != null)
                {
                    if (nids.piece.player == plris.thisPlayer)
                        winCon++;
                }
            }
            if (winCon > 9)
            {
                return (plris.thisPlayer);
            }
        }
        return (0);
    }
}
// The minimax algorithm. This algorithm takes the points into account for the AI so it plays basicly just chasing points. On 
public class Minimax
{
    public void miniMax(PLayer curPlr, List<PLayer> othPlrs, Board boarde, int depth)
    {
        Node bestNode = null;
        Piece bestPiece = null;
        float bestValue = float.MinValue;
        foreach (Piece pic in curPlr.myPieces)
        {
            Node tBestNod = null;
            Node orgPos;
            List<Node> positions = new List<Node>();
            boarde.FindPositions(pic.node, true, positions);
            if (positions.Count > 0)
            {
                foreach (Node nod in positions)
                {
                    float tempValue = Value(pic, curPlr, nod, boarde);
                    orgPos = pic.node;
                    boarde.MovePieces(pic, nod);
                    if (depth > 0)
                    {
                        float[] miniVal = Mini(curPlr, othPlrs, boarde, depth - 1);
                        tempValue = tempValue - (miniVal[0] / 3) + miniVal[1];
                    }
                    if (tempValue > pic.value)
                    {
                        pic.value = tempValue;
                        tBestNod = nod;
                    }

                    boarde.MovePieces(pic, orgPos);
                }
                if (pic.value > bestValue)
                {
                    bestValue = pic.value;
                    bestNode = tBestNod;
                    bestPiece = pic;
                }
                pic.value = float.MinValue;
            }

        }
        boarde.MovePieces(bestPiece, bestNode);
    }
    float Max(PLayer curPlr, List<PLayer> othPlrs, Board boarde, int depth)
    {
        float Maxint = float.MinValue;
        foreach (Piece pic in curPlr.myPieces)
        {
            float picValue = float.MinValue;

            Node orgPos;
            List<Node> positions = new List<Node>();
            boarde.FindPositions(pic.node, true, positions);
            if (positions.Count != 0)
            {
                foreach (Node nod in positions)
                {
                    float tempValue = Value(pic, curPlr, nod, boarde);
                    orgPos = pic.node;
                    boarde.MovePieces(pic, nod);
                    float[] miniMaxValue = new float[2];
                    if (depth > 0)
                    {
                        miniMaxValue = Mini(curPlr, othPlrs, boarde, depth - 1);
                    }
                    tempValue = tempValue - (miniMaxValue[0] / 3) + miniMaxValue[1];
                    if (tempValue > picValue)
                        picValue = tempValue;
                    boarde.MovePieces(pic, orgPos);
                }
            }


            if (picValue > Maxint)
                Maxint = picValue;
        }
        return (Maxint);
    }
    float[] Mini(PLayer curPlr, List<PLayer> othPlrs, Board boarde, int depth)
    {
        float[] Maxint = new float[2] { float.MinValue, float.MinValue };
        foreach (PLayer enPlr in othPlrs)
        {
            foreach (Piece pic in enPlr.myPieces)
            {
                float picValue = float.MinValue;

                Node orgPos;
                List<Node> positions = new List<Node>();
                boarde.FindPositions(pic.node, true, positions);
                if (positions.Count != 0)
                {
                    foreach (Node nod in positions)
                    {
                        float tempValue = Value(pic, enPlr, nod, boarde);
                        orgPos = pic.node;
                        boarde.MovePieces(pic, nod);
                        float maxValue = 0;
                        if (depth > 0)
                        {
                            maxValue = Max(curPlr, othPlrs, boarde, depth - 1);
                        }
                        if (tempValue > picValue)
                            picValue = tempValue;
                        if (maxValue > Maxint[1])
                            Maxint[1] = maxValue;
                        boarde.MovePieces(pic, orgPos);
                    }
                }
                if (picValue > Maxint[0])
                    Maxint[0] = picValue;
            }
        }
        return (Maxint);
    }

    // The Value method gives each move the AI does point and then depending on the position of the marbles or peieces, the ai looks out for the move that rewards with most points. 

    public float Value(Piece piece, PLayer pLayer, Node node, Board boarde)
    {
        float combinedValues = 0;
        Vector2 goalPos = new Vector2();
        bool inGoal = false;
        for (int k = 0; k < pLayer.goals.Length; k++)
        {
            if (pLayer.goals[k].piece == null)
            {
                goalPos = pLayer.goals[k].coordinate;
            }
            else if (pLayer.goals[k].piece.player != pLayer.thisPlayer)
            {
                goalPos = pLayer.goals[k].coordinate;
            }
            if (piece.node.coordinate == pLayer.goals[k].coordinate)
                inGoal = true;
        }
        float thisValue = Vector2.Distance(piece.node.coordinate, goalPos) - Vector2.Distance(node.coordinate, goalPos);
        float extraValue = Vector2.Distance(piece.node.coordinate, node.coordinate) / 3f;
        combinedValues = thisValue + extraValue;
        if (inGoal == true)
            combinedValues -= 3;
        return (combinedValues);
    }
}

public class PLayer
{
    public int thisPlayer;
    public Colour siColr;
    public Node[] goals = new Node[10];
    public List<Piece> myPieces = new List<Piece>();
}

public class Board
{
    B[,] boarde = new B[17, 17]
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

    public Node[,] board = new Node[17, 17];
    public List<Piece> allPieces = new List<Piece>();
    public void StartMetod(int thisMany, List<Colour> aList)
    {
        CreateBoard();
        CreatePieces(thisMany, aList);
    }

    public void CreateBoard()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (boarde[i, j] != B.n)
                {
                    switch (boarde[i, j])
                    {
                        case B.e:
                            board[i, j] = new Node();
                            board[i, j].colr = Colour.none;
                            break;
                        case B.r:
                            board[i, j] = new Node();
                            board[i, j].colr = Colour.red;
                            break;
                        case B.y:
                            board[i, j] = new Node();
                            board[i, j].colr = Colour.yellow;
                            break;
                        case B.w:
                            board[i, j] = new Node();
                            board[i, j].colr = Colour.white;
                            break;
                        case B.g:
                            board[i, j] = new Node();
                            board[i, j].colr = Colour.green;
                            break;
                        case B.b:
                            board[i, j] = new Node();
                            board[i, j].colr = Colour.blue;
                            break;
                        case B.d:
                            board[i, j] = new Node();
                            board[i, j].colr = Colour.black;
                            break;
                    }

                }
            }
        }
    }
    public void FindNodes()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] != null)
                {
                    Node nde = board[i, j];
                    if (j > 0)
                    {
                        if (board[i, j - 1] != null)
                            nde.AddNode(board[i, j - 1], Dir.W);
                    }
                    if (j < board.GetLength(1) - 1)
                    {
                        if (board[i, j + 1] != null)
                            nde.AddNode(board[i, j + 1], Dir.E);
                    }
                    if (i > 0)
                    {
                        if (board[i - 1, j] != null)
                        {
                            nde.AddNode(board[i - 1, j], Dir.NW);
                        }
                        if (j < board.GetLength(1) - 1)
                        {
                            if (board[i - 1, j + 1] != null)
                                nde.AddNode(board[i - 1, j + 1], Dir.NE);
                        }
                    }
                    if (i < board.GetLength(0) - 1)
                    {
                        if (board[i + 1, j] != null)
                        {
                            nde.AddNode(board[i + 1, j], Dir.SE);
                        }
                        if (j > 0)
                        {
                            if (board[i + 1, j - 1] != null)
                                nde.AddNode(board[i + 1, j - 1], Dir.SW);
                        }
                    }
                }
            }
        }
    }

    public void CreatePieces(int playas, List<Colour> theColor)
    {
        for (int i = 0; i < playas; i++)
        {
            switch (playas)
            {
                case 2:
                    CreateAPiece(i + 1, theColor[i]);
                    break;
                case 3:
                    CreateAPiece(i + 1, theColor[i]);
                    break;
                case 4:
                    CreateAPiece(i + 1, theColor[i]);
                    break;
                case 6:
                    CreateAPiece(i + 1, theColor[i]);
                    break;
                default:
                   
                    break;
            }
        }
    }
    void CreateAPiece(int playa, Colour deColr)
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] != null)
                {
                    if (board[i, j].colr == deColr)
                    {
                        board[i, j].piece = new Piece();
                        board[i, j].piece.player = playa;
                        board[i, j].piece.node = board[i, j];
                        allPieces.Add(board[i, j].piece);
                    }
                }

            }
        }
    }
    public List<Node> FindPositions(Node theNode, bool first, List<Node> nodeas)
    {
        foreach (AdjacentNodes nodes in theNode.nodePals)
        {
            if (nodes.node.piece == null && first == true)
                nodeas.Add(nodes.node);
            if (nodes.node.piece != null)
            {
                foreach (AdjacentNodes node2 in nodes.node.nodePals)
                {
                    if (node2.Dir == nodes.Dir)
                    {
                        if (node2.node.piece == null)
                        {
                            if (!nodeas.Contains(node2.node))
                            {
                                nodeas.Add(node2.node);
                                nodeas.AddRange(FindPositions(node2.node, false, nodeas));
                                break;
                            }
                        }

                    }
                }
            }
        }
        return (nodeas);
    }
    public void MovePieces(Piece piece, Node node)
    {
        piece.node.piece = null;
        piece.node = node;
        node.piece = piece;
    }
    public void HardKodedmal(PLayer pLayer)
    {
        Node[] redGol = new Node[10] { board[0, 12], board[1, 11], board[1, 12], board[2, 10], board[2, 11], board[2, 12], board[3, 9], board[3, 10], board[3, 11], board[3, 12] };
        Node[] greenGol = new Node[10] { board[16, 4], board[15, 4], board[15, 5], board[14, 4], board[14, 5], board[14, 6], board[13, 4], board[13, 5], board[13, 6], board[13, 7] };
        Node[] blueGol = new Node[10] { board[12, 12], board[12, 11], board[11, 12], board[12, 10], board[11, 11], board[10, 12], board[12, 9], board[11, 10], board[10, 11], board[9, 12] };
        Node[] yellowGol = new Node[10] { board[4, 4], board[4, 5], board[5, 4], board[4, 6], board[5, 5], board[6, 4], board[4, 7], board[5, 6], board[6, 5], board[7, 4] };
        Node[] whiteGol = new Node[10] { board[12, 0], board[12, 1], board[11, 1], board[12, 2], board[11, 2], board[10, 2], board[12, 3], board[11, 3], board[10, 3], board[9, 9] };
        Node[] blackGol = new Node[10] { board[4, 16], board[4, 15], board[5, 15], board[4, 14], board[5, 14], board[6, 14], board[4, 13], board[5, 13], board[6, 13], board[7, 13] };

        switch (pLayer.siColr)
        {
            case Colour.red:
                pLayer.goals = greenGol;
                break;
            case Colour.black:
                pLayer.goals = whiteGol;
                break;
            case Colour.blue:
                pLayer.goals = yellowGol;
                break;
            case Colour.green:
                pLayer.goals = redGol;
                break;
            case Colour.white:
                pLayer.goals = blackGol;
                break;
            case Colour.yellow:
                pLayer.goals = blueGol;
                break;
        }
    }
    public void CreateCoordinates()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] != null)
                    board[i, j].coordinate = new Vector2(i, j);
            }
        }
    }
}

public class Node
{
    public Piece piece;
    public Colour colr;
    public Vector2 coordinate;
    public List<AdjacentNodes> nodePals = new List<AdjacentNodes>();

    public void AddNode(Node node, Dir dir)
    {
        nodePals.Add(new AdjacentNodes(node, dir));
    }
}

public class Piece
{
    public int player;
    public Node node;
    public float value = float.MinValue;
}

public class AdjacentNodes
{
    Node nod;
    Dir dir;
    public Node node
    {
        get { return this.nod; }
    }
    public Dir Dir
    {
        get { return this.dir; }
    }
    public AdjacentNodes(Node node, Dir dir)
    {
        this.nod = node;
        this.dir = dir;
    }
}





