using UnityEngine;
using System.IO;
using ETK;

public class SaveLoadManagerSkills : MonoBehaviour
{
    private string saveFilePath;
    private static SaveLoadManagerSkills _instance;
    public static SaveLoadManagerSkills Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SaveLoadManagerSkills();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "skillsSaveData.json");
    }

    public void SaveSkills(SkillsSaveData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(saveFilePath, json);
    }

    public SkillsSaveData LoadSkills()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            return JsonUtility.FromJson<SkillsSaveData>(json);
        }
        return new SkillsSaveData();
    }
}
