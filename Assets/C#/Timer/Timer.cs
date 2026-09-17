using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float CurrentTime;
    [SerializeField] TimeSaveManager timeSaveManager;
    [SerializeField] TMP_Text StatsText;

    void Start()
    {
        Invoke(nameof(LoadScoreboard), 7);
    }
    
    void Update()
    {
        CurrentTime += Time.deltaTime;
        StatsText.text = $"Time: {Math.Round(CurrentTime, 2)}s\nTop: {timeSaveManager.GetTop(CurrentTime)} of {timeSaveManager.timeData.player.Count+1}";
    }

    void LoadScoreboard()
    {
        GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<LoadInScoreboardScene>().LoadInScene(CurrentTime); 
    }
}
