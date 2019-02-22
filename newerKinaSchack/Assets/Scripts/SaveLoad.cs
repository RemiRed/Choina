using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

//The save and load script

public class SaveLoad : MonoBehaviour
{


    
    public static SaveLoad saveLoad;
    public B[,] saveBoard;
    public int saveDepth;
    public int saveNumberOfPlayers;
    //public int turne;


    void Awake()
    {
        if (saveLoad == null)
        {
            DontDestroyOnLoad(gameObject);
            saveLoad = this;
        }
        else if (saveLoad != null)
        {
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        // Binaryformatter is used to serialize an object and deserialize it, it creates a byte data. Used to usally save data. 
        BinaryFormatter bf = new BinaryFormatter();

        // Provides a stream of files to be created
        FileStream file = File.Create(Application.persistentDataPath + "/gameData.data");

        TheData data = new TheData();
        data.saveBoard = saveBoard;
        data.saveDepth = saveDepth;
        data.saveNumberOfPlayers = saveNumberOfPlayers;
        //data.turne = turne;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {       // If there already is a file then the player can load it. 
        print("Load");
        if (File.Exists(Application.persistentDataPath + "/gameData.data"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameData.data", FileMode.Open);
            TheData data = (TheData)bf.Deserialize(file);
            
            saveBoard = data.saveBoard;
            FindObjectOfType<ADumbScript>().Board2 = saveBoard;
            saveDepth = data.saveDepth;
            saveNumberOfPlayers = data.saveNumberOfPlayers;
            //turne = data.turne;
            file.Close();
        }
    }
}

[Serializable]
class TheData
{
    public B[,] saveBoard;
    public int saveDepth;
    public int saveNumberOfPlayers;
    //public int turne;
}
