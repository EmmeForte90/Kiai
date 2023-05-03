using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public int highScore;

    public void SaveGame()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
        FileStream stream = new FileStream(Application.persistentDataPath + "/save.xml", FileMode.Create);
        SaveData data = new SaveData();
        data.highScore = highScore;
        serializer.Serialize(stream, data);
        stream.Close();
    }

    public void LoadGame()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
        FileStream stream = new FileStream(Application.persistentDataPath + "/save.xml", FileMode.Open);
        SaveData data = serializer.Deserialize(stream) as SaveData;
        stream.Close();
        highScore = data.highScore;
    }

    [System.Serializable]
    public class SaveData
    {
        public int highScore;
    }
}