using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CardsDataManager : Singleton<CardsDataManager>
{
    // Start is called before the first frame update
    public string gameDataFileName;
    public AllCards allCards;
    void Start()
    {
        LoadGameData();
    }

    private void LoadGameData()
    {
        string dataAsJson;
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
#if UNITY_EDITOR
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);

#elif UNITY_IOS
        string filePath = Path.Combine (Application.streamingAssetsPath + "/Raw", gameDataFileName);
 
#elif UNITY_ANDROID
        string filePath = Path.Combine ("jar:file://" + Application.streamingAssetsPath + "!assets/", gameDataFileName);
 
#endif


        if (File.Exists(filePath))
        {
            // Read the json from the file into a string
            //string dataAsJson = File.ReadAllText(filePath);


#if UNITY_EDITOR || UNITY_IOS
            dataAsJson = File.ReadAllText(filePath);

#elif UNITY_ANDROID
            WWW reader = new WWW (filePath);
            while (!reader.isDone) {
            }
            dataAsJson = reader.text;
#endif

            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            allCards = JsonUtility.FromJson<AllCards>(dataAsJson);
            // Retrieve the allRoundData property of loadedData
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

 
    public List<CardStaticData> GetRandomDeck(uint tot, int[] levels)
    {
     
        List<CardStaticData> deck = new List<CardStaticData>();

        CardStaticData[] level1Copy = allCards.GetShuffleCopy(allCards.level1);
        CardStaticData[] level2Copy = allCards.GetShuffleCopy(allCards.level2);
        CardStaticData[] level3Copy = allCards.GetShuffleCopy(allCards.level3);
        CardStaticData[] level4Copy = allCards.GetShuffleCopy(allCards.level4);
        CardStaticData[] level5Copy = allCards.GetShuffleCopy(allCards.level5);

        List<CardStaticData[]> allCardsList = new List<CardStaticData[]>
        {
            level1Copy,
            level2Copy,
            level3Copy,
            level4Copy,
            level5Copy
        };

        int aux = 0;
        for (int i = 0; i < tot; i++)
        {
            int id = levels[Random.Range(0, levels.Length)];
            CardStaticData[] tempArray = allCardsList[id - 1];
            aux = i;
            if(aux > tempArray.Length - 1)
            {
                aux = Random.Range(0, tempArray.Length);
            }
            deck.Add(tempArray[aux]);
        }

        return deck;
    }

    public SideType GetOppositeSide(SideType side)
    {
        SideType oppSide = SideType.TopRight;
        switch (side)
        {
            case SideType.BottomLeft:
                oppSide = SideType.TopRight;
                break;
            case SideType.BottomRight:
                oppSide = SideType.TopLeft;
                break;
            case SideType.Left:
                oppSide = SideType.Right;
                break;
            case SideType.Right:
                oppSide = SideType.Left;
                break;
            case SideType.TopLeft:
                oppSide = SideType.BottomRight;
                break;
            case SideType.TopRight:
                oppSide = SideType.BottomLeft;
                break;
            default:
                break;
        }
        return oppSide;
    }
        // Update is called once per frame
        void Update()
    {

    }
}
