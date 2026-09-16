//Häjlp från https://uhiyama-lab.com/en/notes/unity/unity-save-load-json-serialization-guide/

using System.Collections.Generic;
using UnityEngine.InputSystem.Controls;

[System.Serializable]
public class TimeData
{
    [System.Serializable]
    public class Player
    {
        public string Name;
        public float Time;
    }
    public List<Player> player = new List<Player>();
    public List<string> InvalidNames = new List<string>();
}
