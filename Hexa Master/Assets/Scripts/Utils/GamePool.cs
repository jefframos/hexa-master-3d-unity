using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePool : Singleton<GamePool>
{
    readonly Stack<GameObject> tilePool = new Stack<GameObject>();
    public GameObject TilePrefab;

    internal GameObject GetTile()
    {
        GameObject spawnedTile;
        // if there is an inactive instance of the prefab ready to return, return that
        if (tilePool.Count > 0)
        {
            // remove the instance from teh collection of inactive instances
            spawnedTile = tilePool.Pop();

        }
        // otherwise, create a new instance
        else
        {
            spawnedTile = Instantiate(TilePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            PooledObject pooledObject = spawnedTile.AddComponent<PooledObject>();
            pooledObject.pool = this;
        }
        return spawnedTile;
    }
    public void ReturnTile(GameObject toReturn)
    {
        PooledObject pooledObject = toReturn.GetComponent<PooledObject>();

        // if the instance came from this pool, return it to the pool
        if (pooledObject != null && pooledObject.pool == this)
        {
            // make the instance a child of this and disable it
            toReturn.transform.SetParent(transform);
            toReturn.SetActive(false);

            // add the instance to the collection of inactive instances
            tilePool.Push(toReturn);
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
