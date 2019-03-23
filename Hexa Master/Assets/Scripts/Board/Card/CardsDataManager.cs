using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CardsDataManager : MonoBehaviour
{
    // Start is called before the first frame update
    public string gameDataFileName;
    void Start()
    {


    }

    private void LoadGameData()
    {
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);

        if (File.Exists(filePath))
        {
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath);

            Debug.Log(dataAsJson);
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            //GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);

            // Retrieve the allRoundData property of loadedData
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
