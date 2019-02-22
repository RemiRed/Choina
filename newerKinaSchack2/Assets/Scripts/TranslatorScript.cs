using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslatorScript : MonoBehaviour
{

    GameMaster inte;
    ADumbScript ext;

    // The points with this whole script is that it changes the variables and positions of the pieces internally while the player only sees in externaly in the board.
    void Start()
    {
        inte = GetComponentInParent<GameMaster>();
        ext = GetComponentInParent<ADumbScript>();
    }
    // as explained above this method does the play withing the code. 
    public void ExternalToInternal()
    {
        Piece siPiec = null;
        Node newNod = null;

        for (int i = 0; i < ext.rBoard.GetLength(0); i++)
        {
            for (int j = 0; j < ext.rBoard.GetLength(1); j++)
            {
                if (ext.rBoard[i, j] != null && inte.board.board != null)
                {
                    if (ext.rBoard[i, j].piece != null && inte.board.board[i, j].piece == null)
                    {
                        newNod = inte.board.board[i, j];
                    }
                    if (ext.rBoard[i, j].piece == null && inte.board.board[i, j].piece != null)
                    {
                        siPiec = inte.board.board[i, j].piece;
                        inte.board.board[i, j].piece = null;
                    }
                }

            }
        }
        newNod.piece = siPiec;
        siPiec.node = newNod;
    }
    // From here we do the internal work and show it to the player
    public void InternalToExternal()
    {
        PieceScript siPiec = null;
        NodeScript newNod = null;

        for (int i = 0; i < inte.board.board.GetLength(0); i++)
        {
            for (int j = 0; j < inte.board.board.GetLength(1); j++)
            {
                if (ext.rBoard[i, j] != null && inte.board.board != null)
                {
                    if (ext.rBoard[i, j].piece != null && inte.board.board[i, j].piece == null)
                    {
                        siPiec = ext.rBoard[i, j].piece;
                        ext.rBoard[i, j].piece = null;
                    }
                    if (ext.rBoard[i, j].piece == null && inte.board.board[i, j].piece != null)
                    {
                        newNod = ext.rBoard[i, j];
                    }
                }

            }
        }
        newNod.piece = siPiec;
        siPiec.nodes = newNod;
        siPiec.transform.position = new Vector3(newNod.transform.position.x, 0, newNod.transform.position.z);
    }

    // Save the board data.
    public void SaveDataToBoard()
    {
        B[,] bree = SaveLoad.saveLoad.saveBoard;
        int[] thePieceNum = new int[6];
        for (int i = 0; i < bree.GetLength(0); i++)
        {
            for (int j = 0; j < bree.GetLength(1); j++)
            {
                if (i > ext.rBoard.GetLength(0))
                {
                    print("i.rboard");
                }
                if (j > ext.rBoard.GetLength(1))
                {
                    print("j.rboard");
                }
                if (i > inte.board.board.GetLength(0))
                {
                    print("i.inteboard");
                }
                if (j > inte.board.board.GetLength(1))
                {
                    print("j.inteboard");
                }
                if (i > bree.GetLength(0))
                {
                    print("bree");
                }
                switch (bree[i, j])
                {
                    case B.b:
                        RearrangePieces(Colour.blue, ext.rBoard[i, j], inte.board.board[i, j], thePieceNum[0]);
                        thePieceNum[0]++;
                        break;
                    case B.r:
                        
                        RearrangePieces(Colour.red, ext.rBoard[i, j], inte.board.board[i, j], thePieceNum[1]);
                        thePieceNum[1]++;
                        break;
                    case B.y:
                        RearrangePieces(Colour.yellow, ext.rBoard[i, j], inte.board.board[i, j], thePieceNum[2]);
                        thePieceNum[2]++;
                        break;
                    case B.d:
                        RearrangePieces(Colour.black, ext.rBoard[i, j], inte.board.board[i, j], thePieceNum[3]);
                        thePieceNum[3]++;
                        break;
                    case B.w:
                        RearrangePieces(Colour.white, ext.rBoard[i, j], inte.board.board[i, j], thePieceNum[4]);
                        thePieceNum[4]++;
                        break;
                    case B.g:
                        RearrangePieces(Colour.green, ext.rBoard[i, j], inte.board.board[i, j], thePieceNum[5]);
                        thePieceNum[5]++;
                        break;
                        
                }
            }
        }
    }
     
    void RearrangePieces(Colour aB, NodeScript aNode, Node aNodee, int thisPic)
    {
        foreach (PLayer plwe in inte.pLayers)
        {
            if (plwe.siColr == aB)
            {
                aNodee.piece = plwe.myPieces[thisPic];
                plwe.myPieces[thisPic].node = aNodee;
                break;
            }
        }
        switch (aB)
        {
            case Colour.black:
                aNode.piece = ext.blackPic[thisPic];
                ext.blackPic[thisPic].nodes = aNode;
                ext.blackPic[thisPic].transform.position = new Vector3(aNode.transform.position.x, 0, aNode.transform.position.z);
                break;
            case Colour.yellow:
                aNode.piece = ext.yellowPic[thisPic];
                ext.yellowPic[thisPic].nodes = aNode;
                ext.yellowPic[thisPic].transform.position = new Vector3(aNode.transform.position.x, 0, aNode.transform.position.z);
                break;
            case Colour.green:
                aNode.piece = ext.greenPic[thisPic];
                ext.greenPic[thisPic].nodes = aNode;
                ext.greenPic[thisPic].transform.position = new Vector3(aNode.transform.position.x, 0, aNode.transform.position.z);
                break;
            case Colour.red:
                if (thisPic >= ext.redPic.Count)
                {
                    print("för lång penis");
                }
                aNode.piece = ext.redPic[thisPic];
                ext.redPic[thisPic].nodes = aNode;
                ext.redPic[thisPic].transform.position = new Vector3(aNode.transform.position.x, 0, aNode.transform.position.z);
                break;
            case Colour.blue:
                aNode.piece = ext.bluePic[thisPic];
                ext.bluePic[thisPic].nodes = aNode;
                ext.bluePic[thisPic].transform.position = new Vector3(aNode.transform.position.x, 0, aNode.transform.position.z);
                break;
            case Colour.white:
                aNode.piece = ext.whitePic[thisPic];
                ext.whitePic[thisPic].nodes = aNode;
                ext.whitePic[thisPic].transform.position = new Vector3(aNode.transform.position.x, 0, aNode.transform.position.z);
                break;
            
                
        }
    }
}
