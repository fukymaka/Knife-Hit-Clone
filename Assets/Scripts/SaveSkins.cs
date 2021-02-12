using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveSkins : MonoBehaviour
{        
    public static string currentSkin = "Default Skin";    
    public static Dictionary<string, bool> skins = new Dictionary<string, bool>();
    public static int appleCoins = 10;


    public static void WriteSkins(string nameSkin, bool isPurchased)
    {
        if (!skins.ContainsKey(nameSkin))
        {
            skins.Add(nameSkin, isPurchased);
        }        
    }


    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath
          + "/MySaveData.dat");
        SaveData data = new SaveData();

        data.savedCurrentSkin = currentSkin;
        data.savedSkins = new Dictionary<string, bool>(skins);
        data.savedAppleCoins = appleCoins;

        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game data saved!");
    }

    public static void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath
          + "/MySaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
              File.Open(Application.persistentDataPath
              + "/MySaveData.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            
            currentSkin = data.savedCurrentSkin;
            skins = new Dictionary<string, bool>(data.savedSkins);
            appleCoins = data.savedAppleCoins;

            Debug.Log("Game data loaded!");
        }
        else
            Debug.LogError("There is no save data!");
    }

    public static void ResetData()
    {
        if (File.Exists(Application.persistentDataPath
          + "/MySaveData.dat"))
        {
            File.Delete(Application.persistentDataPath
              + "/MySaveData.dat");


            currentSkin = "Default Skin";
            skins = new Dictionary<string, bool>();
            appleCoins = 10;


            Debug.Log("Data reset complete!");
        }
        else
            Debug.LogError("No save data to delete.");
    }
}

[Serializable]
class SaveData
{
    public string savedCurrentSkin = "Default Skin";
    public Dictionary<string, bool> savedSkins = new Dictionary<string, bool>();
    public int savedAppleCoins = 0;
}
