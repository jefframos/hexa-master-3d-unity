using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePool : Singleton<GamePool>
{
    readonly Stack<GameObject> tilePool = new Stack<GameObject>();
    public GameObject TilePrefab;

    readonly Stack<GameObject> tileEffectViewPool = new Stack<GameObject>();
    public GameObject TileEffectViewPrefab;

    readonly Stack<GameObject> cardPool = new Stack<GameObject>();
    public GameObject CardPrefab;

    readonly Stack<GameObject> deckPool = new Stack<GameObject>();
    public GameObject DeckPrefab;

    internal GameObject GetDeck()
    {
        return GetObject(DeckPrefab, deckPool);
    }
    public void ReturnDeck(GameObject toReturn)
    {
        ReturnObject(toReturn, deckPool);
    }

    internal GameObject GetCard()
    {
        return GetObject(CardPrefab, cardPool);
    }
    public void ReturnCard(GameObject toReturn)
    {
        ReturnObject(toReturn, cardPool);
    }

    internal GameObject GetTileEffect()
    {
        return GetObject(TileEffectViewPrefab, tileEffectViewPool);
    }
    public void ReturnTileEffect(GameObject toReturn)
    {
        ReturnObject(toReturn, tileEffectViewPool);
    }


    internal GameObject GetTile()
    {        
        return GetObject(TilePrefab, tilePool);
    }
    public void ReturnTile(GameObject toReturn)
    {
        ReturnObject(toReturn, tilePool);
    }



    GameObject GetObject(GameObject prefab, Stack<GameObject> targetPool )
    {
        GameObject spawnedTile;
        // if there is an inactive instance of the prefab ready to return, return that
        if (targetPool.Count > 0)
        {
            // remove the instance from teh collection of inactive instances
            spawnedTile = targetPool.Pop();

        }
        // otherwise, create a new instance
        else
        {
            spawnedTile = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            PooledObject pooledObject = spawnedTile.AddComponent<PooledObject>();
            pooledObject.pool = this;
        }
        return spawnedTile;
    }
    void ReturnObject(GameObject toReturn, Stack<GameObject> targetPool)
    {
        PooledObject pooledObject = toReturn.GetComponent<PooledObject>();

        // if the instance came from this pool, return it to the pool
        if (pooledObject != null && pooledObject.pool == this)
        {
            // make the instance a child of this and disable it
            toReturn.transform.SetParent(transform);
            toReturn.SetActive(false);

            // add the instance to the collection of inactive instances
            targetPool.Push(toReturn);
        }
        // otherwise, just destroy it
        else
        {
            Debug.LogWarning(toReturn.name + " was returned to a pool it wasn't spawned from! Destroying.");
            Destroy(toReturn);
        }
    }

}

public class PooledObject : MonoBehaviour
{
    public GamePool pool;
}
