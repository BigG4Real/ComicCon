using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] string AButtonLoad;
    [SerializeField] List<GameObject> AButtonObjChange;
    [SerializeField] List<MonoBehaviour> AButtonScripts;
    bool AButtonDown;

    [SerializeField] string BButtonLoad;
    [SerializeField] List<GameObject> BButtonObjChange;
    [SerializeField] List<MonoBehaviour> BButtonScripts;
    bool BButtonDown;

    [SerializeField] string YButtonLoad;
    [SerializeField] List<GameObject> YButtonObjChange;
    [SerializeField] List<MonoBehaviour> YButtonScripts;
    bool YButtonDown;

    [SerializeField] string XButtonLoad;
    [SerializeField] List<GameObject> XButtonObjChange;
    [SerializeField] List<MonoBehaviour> XButtonScripts;
    bool XButtonDown;

    [SerializeField] string StartButtonLoad;
    [SerializeField] List<GameObject> StartButtonObjChange;
    [SerializeField] List<MonoBehaviour> StartButtonScripts;
    bool StartButtonDown;

    [SerializeField] string SelectButtonLoad;
    [SerializeField] List<GameObject> SelectButtonObjChange;
    [SerializeField] List<MonoBehaviour> SelectButtonScripts;
    bool SelectButtonDown;
    
    void OnAButtonLoad() => Action(ref AButtonDown, AButtonLoad, AButtonObjChange, AButtonScripts);    
    void OnBButtonLoad()=> Action(ref BButtonDown, BButtonLoad, BButtonObjChange, BButtonScripts);
    void OnYButtonLoad() =>Action(ref YButtonDown, YButtonLoad, YButtonObjChange, YButtonScripts);
    void OnXButtonLoad() =>Action(ref XButtonDown, XButtonLoad, XButtonObjChange, XButtonScripts);

    void OnStart() =>Action(ref StartButtonDown, StartButtonLoad, StartButtonObjChange, StartButtonScripts);
    void OnSelect() =>Action(ref SelectButtonDown, SelectButtonLoad, SelectButtonObjChange, SelectButtonScripts);
    
    
    void Action(ref bool buttonDown, string SceneName = " ", List<GameObject> obj = null, List<MonoBehaviour> scripts = null)
    {
        if(!this.enabled) return;
        buttonDown = !buttonDown;
        if (!buttonDown)
        {
            return;
        }
        if(obj.Count > 0 || scripts.Count > 0)
        {
            for (int i = 0; i < obj.Count; i++)
            {
                obj[i].SetActive(!obj[i].activeInHierarchy);
            }
            for (int i = 0; i < scripts.Count; i++)
            {
                scripts[i].enabled = !scripts[i].enabled;
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
