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
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);

        if (File.Exists(filePath))
        {
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath);

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

        for (int i = 0; i < tot; i++)
        {
            int id = levels[Random.Range(0, levels.Length)];
            switch (id)
            {
                case 1:
                    deck.Add(allCards.level1[Random.Range(0, allCards.level1.Length)]);
                    break;
                case 2:
                    deck.Add(allCards.level2[Random.Range(0, allCards.level2.Length)]);
                    break;
                case 3:
                    deck.Add(allCards.level3[Random.Range(0, allCards.level3.Length)]);
                    break;
                case 4:
                    deck.Add(allCards.level4[Random.Range(0, allCards.level4.Length)]);
                    break;
                case 5:
                    deck.Add(allCards.level5[Random.Range(0, allCards.level5.Length)]);
                    break;
                default:
                    break;
            }
        }

        return deck;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
