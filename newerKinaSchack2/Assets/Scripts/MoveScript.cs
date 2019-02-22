using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveScript : MonoBehaviour
{

    ADumbScript ad;
    GameMaster gm;
    TranslatorScript tr;
    [SerializeField]
    PieceScript SelectedPiece;
    int turn;
    public int numberOfPlayers;
    GameObject guile;
    GameObject guile2;

    List<NodeScript> positions = new List<NodeScript>();
    // assigns everything on start. 
    void Start()
    {
        ad = GetComponentInParent<ADumbScript>();
        gm = GetComponentInParent<GameMaster>();
        tr = GetComponentInParent<TranslatorScript>();
        guile = GameObject.FindGameObjectWithTag("MenuGui");
        guile2 = GameObject.FindGameObjectWithTag("GameGui");
        guile2.SetActive(false);
    }
    //when pressing down New game, it creates a new instance of a game with the settings chosen.
    public void ButtonNew()
    {
        ad.RealStart();
        gm.RealStart();
        guile.SetActive(false);
        guile2.SetActive(true);
    }
    // When pressed on Load game the saved game loads up and all the data bytes is loaded and brings the player to its last saved points. 
    public void ButtonLoad()
    {
        guile.SetActive(false);
        guile2.SetActive(true);
        SaveLoad.saveLoad.Load();
        numberOfPlayers = SaveLoad.saveLoad.saveNumberOfPlayers;
        gm.depth = SaveLoad.saveLoad.saveDepth;
        //turn = SaveLoad.saveLoad.turne;
        ad.RealStart();
        gm.RealStart();
        tr.SaveDataToBoard();
    }
    // "Difficulty" slider.

    public void DeptSlder(float depth)
    {
        gm.depth = (int)depth;
    }

    // Slider deciding the ammount of players. 
    public void PlayerSlider(float players)
    {
        switch ((int)players)
        {
            case 0:
                numberOfPlayers = 2;
                break;
            case 1:
                numberOfPlayers = 3;
                break;
            case 2:
                numberOfPlayers = 4;
                break;
            case 3:
                numberOfPlayers = 6;
                break;
        }
    }
    // Saving button. 
    public void ButtonSave()
    {
        SaveLoad.saveLoad.saveBoard = ad.CreateSaveData();
        SaveLoad.saveLoad.saveDepth = gm.depth;
        SaveLoad.saveLoad.saveNumberOfPlayers = numberOfPlayers;
        //SaveLoad.saveLoad.turne = turn;
        SaveLoad.saveLoad.Save();
    }

    // Update is called once per frame
    void Update()
    {


        Move();
    }
    // Wincondition 
    void TheGame()
    {
        int winCon = gm.Win();
        if (winCon == 0)
        {
            if (turn != 0)
            {
                if (turn < numberOfPlayers)
                {
                    gm.ComPlayerTurn(turn + 1);
                    tr.InternalToExternal();
                    turn++;
                    TheGame();
                }
                else if (turn == numberOfPlayers || turn > numberOfPlayers)
                {
                    turn = 0;
                }
            }
        }
        else
        {
            print("player " + winCon + " winns!");
        }

    }
    // Move method. it uses a raycast to know what piece the player is targeting and then it highlights the possible position the player can move to, it also calculates the "jumped over"
    // positions. Did not figure out how to uncheck the piece tho. So if you choose one piece, you have to commit. This is hardcore Chinese checkers. 
    void Move()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (turn == 0)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100.0F))
                {
                    if (SelectedPiece == null && hit.transform.gameObject.CompareTag("APiece"))
                    {
                        if (hit.transform.gameObject.GetComponent<PieceScript>().playa == 1)
                        {
                            SelectedPiece = hit.transform.gameObject.GetComponent<PieceScript>();
                            FindNodes(SelectedPiece.nodes, true);
                            foreach (NodeScript nodesss in positions)
                                nodesss.GetComponent<Renderer>().material.color = Color.black;
                        }
                    }
                    else if (SelectedPiece != null && hit.transform.gameObject.CompareTag("ANode"))
                    {
                        foreach (NodeScript nodess in positions)
                        {
                            if (nodess == hit.transform.gameObject.GetComponent<NodeScript>())
                            {
                                foreach (NodeScript nodis in positions)
                                    nodis.GetComponent<Renderer>().material.color = Color.white;
                                NewPosition(SelectedPiece, nodess);
                                SelectedPiece = null;
                                turn++;
                                positions.Clear();
                                tr.ExternalToInternal();
                                TheGame();
                                break;
                            }
                        }
                    }
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (SelectedPiece != null)
            {
                foreach (NodeScript nodesss in positions)
                    nodesss.GetComponent<Renderer>().material.color = Color.white;
                SelectedPiece = null;
            }
        }
    }
    // Speaks for it self, This method is the method that finds all the adjecent neighbours. 
    void FindNodes(NodeScript theNode, bool first)
    {
        foreach (AdjecentNeighbours node in theNode.addNode)
        {
            if (node.nodes.piece == null && first == true)
                positions.Add(node.nodes);
            else if (node.nodes.piece != null)
            {
                foreach (AdjecentNeighbours node2 in node.nodes.addNode)
                {
                    if (node2.Dire == node.Dire)
                    {
                        if (node2.nodes.piece == null)
                        {
                            if (!positions.Contains(node2.nodes))
                            {
                                positions.Add(node2.nodes);
                                FindNodes(node2.nodes, false);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
    // It takes on the new position. 
    void NewPosition(PieceScript siPiece, NodeScript siNode)
    {
        siPiece.transform.position = new Vector3(siNode.transform.position.x, 0, siNode.transform.position.z);
        siPiece.nodes.piece = null;
        siNode.piece = siPiece;
        siPiece.nodes = siNode;
    }
}
