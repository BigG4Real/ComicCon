//Häjlp från https://uhiyama-lab.com/en/notes/unity/unity-save-load-json-serialization-guide/

using System.Collections.Generic;

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
}
