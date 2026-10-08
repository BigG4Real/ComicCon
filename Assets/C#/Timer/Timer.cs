using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float CurrentTime;
    [SerializeField] TimeSaveManager timeSaveManager;
    [SerializeField] TMP_Text StatsText;
    [SerializeField] GameObject MinusTimeObj;
    
    void Update()
    {
        CurrentTime += Time.deltaTime;
        StatsText.text = $"Time: {Math.Round(CurrentTime, 2)}s\nTop: {timeSaveManager.GetTop(CurrentTime)} of {timeSaveManager.timeData.player.Count+1}";
    }

    public void LoadScoreboard()
    {
        GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<LoadInScoreboardScene>().LoadInScene(CurrentTime); 
    }

    public void TimeRemove(float time)
    {
        if(time <= 0) return;
        GameObject obj = Instantiate(MinusTimeObj, transform.GetChild(0));
        obj.GetComponent<TMP_Text>().text = $"-{time}s";
        CurrentTime -= time;
        Destroy(obj, 5);
    }
}
