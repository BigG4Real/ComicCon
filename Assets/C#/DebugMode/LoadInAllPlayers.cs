using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class LoadInAllPlayers : MonoBehaviour
{
    [SerializeField] RectTransform placeToSpawn;
    [SerializeField] RectTransform fixContent;
    [SerializeField] GameObject playerData;
    [SerializeField] TimeSaveManager TimeSaveManager;

    [System.Serializable]
    class AllPlayers
    {
        public GameObject gameObjectData;
        public PlayerBoxData data;
    }
    [SerializeField] List<AllPlayers> allPlayers = new List<AllPlayers>();
    int currentHoldingPlayer = 0;
    public int howMany = 50;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] TMP_Text saveLocation;
    [SerializeField] bool allowInteraction = true;

    void Start()
    {
        placeToSpawn.position = fixContent.position;
        UpdateText();
    }

    void UpdateText()
    {
        string SaveFolder = TimeSaveManager.ChangeSaveLocationManager.GetDataDiractory();
        if(saveLocation == null) {return;}
        saveLocation.text = $"Save to:\n''{SaveFolder.Replace(@"\", "/")}''\n\nTo chnage save location:\nGo to ''{TimeSaveManager.ChangeSaveLocationManager.saveFilePath.Replace(@"\", "/")}'' and change the json to disare location\nTip: Make a copy of the orginal json save file to the new place";
    } 

    void Update()
    {
        if(allPlayers.Count == 0) {LoadIn();return;}
        if(!allowInteraction) return;
        allPlayers[currentHoldingPlayer].data.select.enabled = true;
        if(currentHoldingPlayer > 0)
        {
            allPlayers[currentHoldingPlayer - 1].data.select.enabled = false;    
        }
        if(currentHoldingPlayer < allPlayers.Count -1)
        {
            allPlayers[currentHoldingPlayer + 1].data.select.enabled = false;            
        }
    }

    void RemoveAllPlayer()
    {
        for (int i = 0; i < allPlayers.Count; i++)
        {
            Destroy(allPlayers[i].gameObjectData);
        }
        allPlayers.Clear();
        Start();
    }

    void OnUpOrDownDebug(InputValue value)
    {
        if(!allowInteraction) return;
        int check = currentHoldingPlayer;
        currentHoldingPlayer = Math.Clamp(currentHoldingPlayer -= (int)value.Get<Vector2>().y, 0, allPlayers.Count - 1);
        if(check != currentHoldingPlayer)
            scrollRect.content.transform.position += (int)value.Get<Vector2>().y * scrollRect.scrollSensitivity * Vector3.down;
    }

    void OnDelet()
    {
        TimeSaveManager.RemoveRecord(TimeSaveManager.GetName(allPlayers[currentHoldingPlayer].data.Name));
        Debug.Log("Deleted succes");
        TimeSaveManager.SaveTime();
        RemoveAllPlayer();
    }

    void LoadIn()
    {
        for(int i = 0; i < howMany; i++)
        {
            if(i >= TimeSaveManager.timeData.player.Count){ break; }
            GameObject displayData = Instantiate(playerData, placeToSpawn);
            displayData.GetComponentInChildren<PlayerBoxData>().Id = i;
            displayData.GetComponentInChildren<PlayerBoxData>().name = TimeSaveManager.timeData.player[i].Name;
            displayData.GetComponentInChildren<PlayerBoxData>().Name = TimeSaveManager.timeData.player[i].Name;
            int displayNum = i;
            displayData.GetComponentInChildren<TMP_Text>().text = $"Top {displayNum + 1}: {TimeSaveManager.timeData.player[i].Name}: {TimeSaveManager.timeData.player[i].Time}s";
            AllPlayers newPlayer = new AllPlayers();
            newPlayer.gameObjectData = displayData;
            newPlayer.data = displayData.GetComponentInChildren<PlayerBoxData>();
            allPlayers.Add(newPlayer);
        }
    }
}
