using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoadManager
{
    public static void Save(GameData gameData)
    {

        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.sav"); //you can call it anything you want
        bf.Serialize(file, gameData);
        file.Close();
    }

    public static GameData Load()
    {
        GameData data = new GameData();
        if (File.Exists(Application.persistentDataPath + "/savedGames.sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.sav", FileMode.Open);
            data = bf.Deserialize(file) as GameData;
            file.Close();
            return data;
        }
        return data;
    }

}
[Serializable]
public class SpellingList
{
    public string name = "";
    public int index;
    public List<string> list;
    public SpellingList(List<string> tempList,string tempName,int tempIndex)
    {
        name = tempName;
        list = tempList;
        index = tempIndex;
    }
}
[Serializable]
public class GameData
{
    public int wonGames;
    public List<string> currentWordList = new List<string>();
    public List<SpellingList> masterList = new List<SpellingList>();
    public int avgScoreThisWeek;
    public int listIndex = 0;
    public void AddWins()
    {
       wonGames++;
        
    }
    public GameData()
    {

        masterList.Add(new SpellingList(currentWordList, "Default",0));
        currentWordList = masterList[listIndex].list;
    }
    public GameData(int index,List<SpellingList> lists)
    {
        listIndex = index;
        masterList = lists;
        currentWordList = masterList[listIndex].list;
    }
    public void UpdateMasterList(List<SpellingList> lists)
    {
        masterList = lists;
    }
    public void UpdateList(List<string> list)
    {
        currentWordList = list;
    }
    public void UpdateCurrentIndex(int i)
    {
        listIndex = i;
        currentWordList = masterList[i].list;
    }
}