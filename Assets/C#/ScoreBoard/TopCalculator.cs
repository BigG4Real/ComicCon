using TMPro;
using UnityEngine;

public class TopCalculator : MonoBehaviour
{
    [SerializeField] TimeSaveManager timeSaveManager;
    [SerializeField] TMP_Text topText;
    [SerializeField] TMP_Text scoreboardName;
    void Start()
    {
        topText.text = $"You are Top {timeSaveManager.GetTop(GameObject.FindWithTag("Scoreboard").GetComponent<LoadInScoreboardScene>().Time)} of {timeSaveManager.timeData.player.Count} people";
    }
}
