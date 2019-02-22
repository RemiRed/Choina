using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSave : MonoBehaviour {

    public static GameSave saveLoad;


    void Awake()
    {
        if (saveLoad == null)
        {
            saveLoad = this;
        }   
        else if (saveLoad != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    public bool IsSaveFile()
    {
        return Directory.Exists(Application.persistentDataPath + "/game_save");
    }
    
    public void SaveGame()
    {
        if (!IsSaveFile())
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/game_save");
        }



    }

}
