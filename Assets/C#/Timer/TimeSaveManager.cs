using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

public class TimeSaveManager : MonoBehaviour
{
    public string saveFilePath;
    public TimeData timeData;
    
    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "time-records.json");

        LoadData();
    }

    void LoadData()
    {
        if (!File.Exists(saveFilePath))
        {
            timeData = new TimeData();
            return;
        }

        string json = File.ReadAllText(saveFilePath);
        timeData = JsonUtility.FromJson<TimeData>(json);

        if (timeData == null)
        {
            Debug.LogWarning("Ivaild svae file");
            timeData = new TimeData();
        }

        if (timeData.player == null)
        {
            timeData.player = new List<TimeData.Player>();
        }
        Debug.Log("Load successful!\n" + json);
    }

    public void SaveTime(string name, float time)
    {
        if(string.IsNullOrEmpty(name)) return;

        InsertRecord(name, time);

        string jsonSave = JsonUtility.ToJson(timeData, true);

        File.WriteAllText(saveFilePath, jsonSave);
    }

    public void InsertRecord(string name, float time)
    {
        time = (float)Math.Round(time, 2);
        int nameLookUp = GetName(name);
        
        if(nameLookUp == -1)
        {
            TimeData.Player playerRecord = new TimeData.Player();
            playerRecord.Name = name;
            playerRecord.Time = (playerRecord.Time > time) ? time : playerRecord.Time;
            timeData.player.Add(playerRecord);
        }
        else
        {
            timeData.player[nameLookUp].Time = time;
        }
    }

    public int GetName(string name)
    {
        for (int i = 0; i < timeData.player.Count; i++)
        {
            if(timeData.player[i].Name == name)
            {
                return i;
            }
        }
        return -1;
    }
}
