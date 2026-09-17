using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;
 
public class TimeSaveManager : MonoBehaviour
{
    public string saveFilePath;
    public TimeData timeData;

    public ChangeSaveLocationManager ChangeSaveLocationManager;
    
    void Start()
    {
        saveFilePath = ChangeSaveLocationManager.GetDataDiractory();

        LoadData();
    }

    void LoadData()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogError($"{saveFilePath} Don't Exist");
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
        Debug.Log($"Load successful\nPath: {saveFilePath}\nFile:\n{json}");
    }

    public void SaveTime(string name, float time)
    {
        if(string.IsNullOrEmpty(name)) return;

        InsertRecord(name, time);

        SaveTime();
    }

    public void SaveTime()
    {
        timeData.player = timeData.player.OrderBy(td => td.Time).ToList();
        string jsonSave = JsonUtility.ToJson(timeData, true);

        File.WriteAllText(saveFilePath, jsonSave);
    }

    public bool InsertRecord(string name, float time)
    {
        for (int i = 0; i < timeData.InvalidNames.Count; i++)
        {
            if(name == timeData.InvalidNames[i]) return false;
        }
        time = (float)Math.Round(time, 2);
        int nameLookUp = GetName(name);
        
        if(nameLookUp == -1)
        {
            TimeData.Player playerRecord = new TimeData.Player();
            playerRecord.Name = name;
            playerRecord.Time = time;
            timeData.player.Add(playerRecord);
        }
        else
        {
            timeData.player[nameLookUp].Time = time;
        }
        return true;
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

    public void RemoveRecord(int id) 
    {
        timeData.InvalidNames.Add(timeData.player[id].Name);
        timeData.player.RemoveAt(id);
    }
}
