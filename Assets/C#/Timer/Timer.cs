using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public string Name;
    public float CurrentTime;
    [SerializeField] TimeSaveManager timeSaveManager;
    [SerializeField] TMP_Text StatsText;

    void Update()
    {
        CurrentTime += Time.unscaledDeltaTime;
        StatsText.text = $"Time: {Math.Round(CurrentTime, 2)}s\nTop: {GetTop()} of {timeSaveManager.timeData.player.Count+1}";
    }

    int GetTop()
    {
        int top = timeSaveManager.timeData.player.Count;
        for (int i = 0; i < timeSaveManager.timeData.player.Count; i++)
        {
            if(timeSaveManager.timeData.player[i].Time < CurrentTime)
            {
                top++;
            }
        }
        return top;
    }

    void Save()
    {
        GetComponent<TimeSaveManager>().SaveTime(Name, CurrentTime);        
        enabled = false;
    }
}
