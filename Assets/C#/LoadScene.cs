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

    void OnAButtonLoad() => Action(AButtonLoad, AButtonDown, AButtonObjChange);    
    void OnBButtonLoad()=> Action(BButtonLoad, BButtonDown, BButtonObjChange);
    void OnYButtonLoad() =>Action(YButtonLoad, YButtonDown, YButtonObjChange);
    void OnXButtonLoad() =>Action(XButtonLoad, XButtonDown, XButtonObjChange);

    void OnStart() =>Action(StartButtonLoad, StartButtonDown, StartButtonObjChange);
    void OnSelect() =>Action(SelectButtonLoad, SelectButtonDown, SelectButtonObjChange);
    
    
    void Action(string SceneName, bool buttonDown, List<GameObject> obj = null)
    {
        buttonDown = !buttonDown;
        if(obj.Count > 0)
        {
            for (int i = 0; i < obj.Count; i++)
            {
                obj[i].SetActive(!obj[i].activeInHierarchy);
            }
            return;
        }
        if(SceneName == null) {return;}
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
