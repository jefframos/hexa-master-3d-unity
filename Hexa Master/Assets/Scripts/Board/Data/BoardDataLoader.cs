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
        JsonLoader jsonLoader = JsonLoader.Instance;
        
        string json = jsonLoader.LoadFromStreaming(path);

        BoardData boardData;
        if (json != null)
        {
            boardData = JsonUtility.FromJson<BoardData>(json);

        }
        else
        {
            boardData = new BoardData();
            boardData.CreateMock();
        }

       

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
