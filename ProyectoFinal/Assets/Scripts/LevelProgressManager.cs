using System.IO;
using UnityEngine;

public static class LevelProgressManager
{
    //Ruta para ver en el editor
    private static string EditorSavePath =>
        Application.dataPath + "/Resources/LevelProgress/levelProgress.json";

    // Ruta para builds
    private static string BuildSavePath =>
        Application.persistentDataPath + "/levelProgress.json";

    public static void SaveProgress(LevelProgressData data)
    {
        string json = JsonUtility.ToJson(data);
        string savePath = Application.isEditor ? EditorSavePath : BuildSavePath;

        // Crear directorio si no existe (solo en Editor)
        if (Application.isEditor)
        {
            string directory = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        File.WriteAllText(savePath, json);

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh(); // Actualiza el Editor para ver el archivo
#endif
    }

    public static LevelProgressData LoadProgress(int defaultLevelCount)
    {
        string loadPath = Application.isEditor ? EditorSavePath : BuildSavePath;

        if (File.Exists(loadPath))
        {
            string json = File.ReadAllText(loadPath);
            return JsonUtility.FromJson<LevelProgressData>(json);
        }
        return new LevelProgressData(defaultLevelCount);
    }
}