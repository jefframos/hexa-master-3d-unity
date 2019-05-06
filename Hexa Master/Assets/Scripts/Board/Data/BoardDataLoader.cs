using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BoardDataLoader : MonoBehaviour
{
    public BoardTilemapConfig tilemapConfig;
    List<BoardData> boardList = new List<BoardData>();
    // Start is called before the first frame update
    public BoardData LoadBoard(string path)
    {
        JsonLoader jsonLoader = new JsonLoader();
        //        string dataAsJson;

        //        string filePath = Path.Combine(Application.streamingAssetsPath, path);
        //        // Path.Combine combines strings into a file path
        //        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        //#if UNITY_EDITOR
        //        filePath = Path.Combine(Application.streamingAssetsPath, path);

        //#elif UNITY_IOS
        //        string filePath = Path.Combine (Application.dataPath + "/Raw", path);

        //#elif UNITY_ANDROID
        //        filePath = Path.Combine ("jar:file://" +Application.dataPath + "!assets/", path);
        //        //filePath = Application.streamingAssetsPath + gameDataFileName;
        //#endif
        //        if (File.Exists(filePath))
        //        {
        //            // Read the json from the file into a string
        //            //string dataAsJson = File.ReadAllText(filePath);

        //            dataAsJson = File.ReadAllText(filePath);
        //#if UNITY_EDITOR || UNITY_IOS
        //            //dataAsJson = File.ReadAllText(filePath);

        //#elif UNITY_ANDROID
        //            //WWW reader = new WWW (filePath);
        //            //while (!reader.isDone) {
        //            //}
        //            //dataAsJson = reader.text;
        //#endif
        // Pass the json to JsonUtility, and tell it to create a GameData object from it
        string json = jsonLoader.LoadFromStreaming(path);
        BoardData boardData = JsonUtility.FromJson<BoardData>(json);//JsonUtility.FromJson<BoardData>(dataAsJson);
                                                                    // Retrieve the allRoundData property of loadedData

        boardList.Add(boardData);

        boardData.SetConfig(tilemapConfig);

        return boardData;
        //}
        //else
        //{
        //    Debug.LogError("Cannot load game data!");
        //}

        //return null;
    }
}
