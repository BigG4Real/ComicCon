using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] string AButtonLoad;
    [SerializeField] List<GameObject> AButtonObjChange;
    bool AButtonDown;

    [SerializeField] string BButtonLoad;
    [SerializeField] List<GameObject> BButtonObjChange;
    bool BButtonDown;

    [SerializeField] string YButtonLoad;
    [SerializeField] List<GameObject> YButtonObjChange;
    bool YButtonDown;

    [SerializeField] string XButtonLoad;
    [SerializeField] List<GameObject> XButtonObjChange;
    bool XButtonDown;

    [SerializeField] string StartButtonLoad;
    [SerializeField] List<GameObject> StartButtonObjChange;
    bool StartButtonDown;

    [SerializeField] string SelectButtonLoad;
    [SerializeField] List<GameObject> SelectButtonObjChange;
    bool SelectButtonDown;

    void OnAButtonLoad() => Action(ref AButtonDown, AButtonLoad, AButtonObjChange);    
    void OnBButtonLoad()=> Action(ref BButtonDown, BButtonLoad, BButtonObjChange);
    void OnYButtonLoad() =>Action(ref YButtonDown, YButtonLoad, YButtonObjChange);
    void OnXButtonLoad() =>Action(ref XButtonDown, XButtonLoad, XButtonObjChange);

    void OnStart() =>Action(ref StartButtonDown, StartButtonLoad, StartButtonObjChange);
    void OnSelect() =>Action(ref SelectButtonDown, SelectButtonLoad, SelectButtonObjChange);
    
    
    void Action(ref bool buttonDown, string SceneName = " ", List<GameObject> obj = null)
    {
        buttonDown = !buttonDown;
        if (!buttonDown)
        {
            return;
        }
        if(obj.Count > 0)
        {
            for (int i = 0; i < obj.Count; i++)
            {
                obj[i].SetActive(!obj[i].activeInHierarchy);
            }
            buttonDown = false;
            return;
        }
        if(string.IsNullOrEmpty(SceneName)) {return;}
        SceneManager.LoadScene(SceneName);
    }

    void Update()
    {
        if(StartButtonDown && SelectButtonDown)
        {
            SceneManager.LoadScene("DebugMode");
        }
    }

}
