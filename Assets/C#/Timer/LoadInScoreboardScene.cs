using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadInScoreboardScene : MonoBehaviour
{
    public float Time;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void LoadInScene(float time)
    {
        Time = time;
        SceneManager.LoadScene("Scoreboard");
    }
}
