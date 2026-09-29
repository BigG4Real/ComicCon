using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartMap : MonoBehaviour
{
    void OnReload()
    {
        SceneManager.LoadScene("Map");
    }
    void OnReloadStart()
    {
        SceneManager.LoadScene("Start");
    }
}
