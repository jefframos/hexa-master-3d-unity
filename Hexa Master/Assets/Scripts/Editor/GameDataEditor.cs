using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class GameDataEditor : EditorWindow
{

    public AllCards allCards;

    private string gameDataProjectFilePath = "/StreamingAssets/cardsFull.json";

    [MenuItem("Window/Game Data Editor")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(GameDataEditor)).Show();
    }

    void OnGUI()
    {
        if (allCards != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("AllCards");
            EditorGUILayout.PropertyField(serializedProperty, true);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save data"))
            {
                SaveGameData();
            }
        }

        if (GUILayout.Button("Load data"))
        {
            LoadGameData();
        }
    }

    private void LoadGameData()
    {
        string filePath = Application.dataPath + gameDataProjectFilePath;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            allCards = JsonUtility.FromJson<AllCards>(dataAsJson);
        }
        else
        {
            allCards = new AllCards();
        }
    }

    private void SaveGameData()
    {

        string dataAsJson = JsonUtility.ToJson(allCards);

        string filePath = Application.dataPath + gameDataProjectFilePath;
        File.WriteAllText(filePath, dataAsJson);

    }
}