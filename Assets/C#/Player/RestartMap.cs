using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartMap : MonoBehaviour
{
    void OnReload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void OnReloadStart()
    {
        SceneManager.LoadScene("Start");
    }
}
