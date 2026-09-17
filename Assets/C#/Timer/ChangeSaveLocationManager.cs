using UnityEngine;
using System.IO;

public class ChangeSaveLocationManager : MonoBehaviour
{
    public string saveFilePath;
    public ChangeSaveLocation ChangeSaveLocation;

    [SerializeField] TimeSaveManager TimeSaveManager;

    void Start()
    {
        GetDataDiractory();
        SaveNewDiractory();
    }

    public string GetDataDiractory()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "save-location.json");
        if (!File.Exists(saveFilePath))
        {
            ChangeSaveLocation = new ChangeSaveLocation();
            ChangeSaveLocation.SaveFolder = Path.Combine(Application.persistentDataPath, "time-records.json");
            return ChangeSaveLocation.SaveFolder;
        }

        string json = File.ReadAllText(saveFilePath);
        ChangeSaveLocation = JsonUtility.FromJson<ChangeSaveLocation>(json);

        if (ChangeSaveLocation == null)
        {
            Debug.LogWarning("Ivaild save file");
            ChangeSaveLocation = new ChangeSaveLocation();
        }

        if (ChangeSaveLocation.SaveFolder == null)
        {
            ChangeSaveLocation.SaveFolder = Path.Combine(Application.persistentDataPath, "time-records.json");
        }
        Debug.Log($"Load successful\nPath: {Application.persistentDataPath}/time-records.json\nFile:\n{json}");
        return ChangeSaveLocation.SaveFolder;
    }

    public void SaveNewDiractory()
    {
        string jsonSave = JsonUtility.ToJson(ChangeSaveLocation, true);

        File.WriteAllText(saveFilePath, jsonSave);
    }
}
