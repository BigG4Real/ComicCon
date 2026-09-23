using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class screenKeyboard : MonoBehaviour
{
    [SerializeField] Transform spawnPos;
    [SerializeField] List<GameObject> AllKeys = new List<GameObject>();
    [SerializeField] GameObject KeyPrefab;
    [SerializeField] List<char> value = new List<char>();

    [SerializeField] int selectedKey;

    [SerializeField] string Name;
    [SerializeField] TMP_Text nameDisplay;
    bool noName;

    [SerializeField] GameObject InputName;
    [SerializeField] GameObject ScoreBoard;

    [SerializeField] TMP_Text scoreBoardDisplay;
    [SerializeField] TimeSaveManager TimeSaveManager;
    bool OnlyOneMove = true;
    Vector2 lastMove;
    void Start()
    {
        for (int i = 0; i < value.Count; i++)
        {
            GameObject spawned = Instantiate(KeyPrefab, spawnPos);
            spawned.GetComponentInChildren<TMP_Text>().text = value[i].ToString();
            spawned.GetComponentInChildren<keyData>().key = value[i];
            AllKeys.Add(spawned);
        }
        nameDisplay.text = null;
        noName = true;
    }

    void Update() {
        if (!noName)
        {
            nameDisplay.text = Name;
        }
        if(string.IsNullOrEmpty(nameDisplay.text))
        {
            noName = true;
            nameDisplay.text = "Input your name here!";
            nameDisplay.color = Color.gray7;
            nameDisplay.fontStyle = FontStyles.Italic;
        }
        for (int i = 0; i < AllKeys.Count; i++)
        {
            AllKeys[i].GetComponent<keyData>().select.enabled = false;
        }
        AllKeys[selectedKey].GetComponent<keyData>().select.enabled = true;
    }
    
    void OnKeyboardScreenMove(InputValue inputValue)
    {
        if(!OnlyOneMove) {OnlyOneMove = true; return;}
        Vector2 dir = inputValue.Get<Vector2>();
        dir = LegitMove(dir);
        if(lastMove == dir) return;
        lastMove = dir;
        Debug.Log(dir);
        if(dir.y != 0 && OnlyOneMove)
        {
            selectedKey += (int)(value.Count/4 * dir.y * -1);

            if(selectedKey > AllKeys.Count-1)
            {
                selectedKey -=value.Count;
            }
            else if(selectedKey < 0)
            {
                selectedKey +=value.Count;
            }
        }
        else if(OnlyOneMove)
            selectedKey += (int)dir.x;
        selectedKey = Math.Clamp(selectedKey, 0, AllKeys.Count-1);
    }

    Vector2 LegitMove(Vector2 dir)
    {
        float deadZone = 0.2f;
        
        if(dir.x >= -deadZone && dir.x <= deadZone)
        {
            dir.x = 0;
        }
        else if(dir.x >= deadZone){dir.x = 1;}
        else if(dir.x <= -deadZone){dir.x = -1;}

        if(dir.y >= -deadZone && dir.y <= deadZone)
        {
            dir.y = 0;
        }
        else if(dir.y >= deadZone){dir.y = 1;}
        else if(dir.y <= -deadZone){dir.y = -1;}
        return dir;
    }

    void OnSelect()
    {
        if(Name.Length > 23){return;}
        if(noName)
        {
            noName = false;
            nameDisplay.color = Color.white;
            nameDisplay.fontStyle = FontStyles.Bold;
            nameDisplay.fontStyle = FontStyles.SmallCaps;
        }
        Name = Name.Insert(Name.Length, value[selectedKey].ToString()); 
    }

    void OnSpace()
    {
        if(noName) return;
        Name = Name.Insert(Name.Length, " "); 
    }

    void OnRemove()
    {
        try
        {
            Name = Name.Remove(Name.Length-1);
        }
        catch{} 
    }
    void OnDone()
    {
        if(TimeSaveManager.InsertRecord(Name, GameObject.FindWithTag("Scoreboard").GetComponent<LoadInScoreboardScene>().Time))
        {
            TimeSaveManager.SaveTime(Name, GameObject.FindWithTag("Scoreboard").GetComponent<LoadInScoreboardScene>().Time);
            scoreBoardDisplay.text = $"Top {TimeSaveManager.GetName(Name)+1}: {Name}: {Math.Round(GameObject.FindWithTag("Scoreboard").GetComponent<LoadInScoreboardScene>().Time, 2)}s";
            InputName.SetActive(false);
            ScoreBoard.SetActive(true);
            Destroy(GameObject.FindWithTag("Scoreboard"));
        }
    }

}
