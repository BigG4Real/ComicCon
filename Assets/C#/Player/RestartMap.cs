using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartMap : MonoBehaviour
{
    void OnReload()
    {
        SceneManager.LoadScene("Map");
        Debug.Log("Slop");
    }
    void OnReloadStart()
    {
        SceneManager.LoadScene("Start");
        Debug.Log("Slop");
    }
}
