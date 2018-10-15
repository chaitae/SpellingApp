using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionManager : MonoBehaviour {
    static GameActionManager _instance;
    public static GameActionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("GameActionManager");
                go.AddComponent<GameActionManager>();
                _instance = new GameActionManager();
            }
            return _instance;
        }
    }
    public void OnEnterString(string word)
    {

    }
}
