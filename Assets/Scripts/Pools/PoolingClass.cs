using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code;

public class PoolingClass : ScriptableObject
{
    GameObject poolHolder;
    private GameObject prefab;
    List<GameObject> ObjectList;
    private int startAmount;
    private bool hasLimit;
    private int limit;
    private bool isProjectile;

    /// <summary>
    /// Creates a new pooling class with no limit.
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="_prefab"></param>
    /// <param name="_startAmount"></param>
    public PoolingClass(GameObject parent, GameObject _prefab, int _startAmount)
    {
        poolHolder = parent;

        ObjectList = new List<GameObject>();

        this.prefab = _prefab;
        this.startAmount = _startAmount;

        // Don't limit the amount of game objects.
        this.hasLimit = false;

        // Add new game objects to the pool.
        for (int i = 0; i < startAmount; i++)
        {
            ObjectInstantiate();
        }
    }

    /// <summary>
    /// Creates a new pooling class that has an amount limit.
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="_prefab"></param>
    /// <param name="_startAmount"></param>
    /// <param name="_limit"></param>
    public PoolingClass(GameObject parent, GameObject _prefab, int _startAmount, int _limit)
    {
        poolHolder = parent;

        ObjectList = new List<GameObject>();

        this.prefab = _prefab;
        this.startAmount = _startAmount;

        // Prevent the amount of objects from going over a limit.
        this.hasLimit = true;
        this.limit = _limit;

        // Add new game objects to the pool.
        for (int i = 0; i < startAmount; i++)
        {
            ObjectInstantiate();
        }
    }

    /// <summary>
    ///  Instantiate a new object and add it to the poll..
    /// </summary>
    public void ObjectInstantiate()
    {
        // Instantate new game object.
        GameObject gameObj = (GameObject)Instantiate(prefab);
        gameObj.SetActive(false);
        gameObj.transform.parent = poolHolder.transform;

        //Add new game object to the pool.
        ObjectList.Add(gameObj);
    }


    /// <summary>
    /// Searches the object array to find a deactive game object.
    /// Return the first deactive game object inthe arry.
    /// </summary>
    /// <returns></returns>
    public GameObject GetGameObject()
    {
        // Search throuh a game objects pool list.
        for (int i = 0; i < ObjectList.Count; i++)
        {
            // Check game object is not in use.
            if (!ObjectList[i].activeInHierarchy)
            {
                return ObjectList[i];
            }
        }

        // Add a new object to a pool if none is aviable.
        if (hasLimit == false || ObjectList.Count < limit)
        {
            // Instantate new game object.
            GameObject gameObj = (GameObject)Instantiate(prefab);
            gameObj.SetActive(false);
            gameObj.transform.parent = poolHolder.transform;

            // Add new game object to the pool.
            ObjectList.Add(gameObj);
            return gameObj;
        }

        return null;
    }

    /// <summary>
    /// Spawns a deactive game object
    /// </summary>
    /// <param name="_position"></param>
    /// <param name="_rotation"></param>
    public void SpawnObject(Vector3 _position, Quaternion _rotation)
    {
        // Collect game object from the pool.
        GameObject gameObj = GetGameObject();

        // Check for null.
        if (gameObj == null)
            return;

        // Set transorm position and rotation.
        gameObj.transform.position = _position;
        gameObj.transform.rotation = _rotation;
        gameObj.SetActive(true);
    }


    /// <summary>
    /// Spawns a deactive game object
    /// Returns the spawn game object to the what called the methord.
    /// </summary>
    /// <param name="_position"></param>
    /// <param name="_rotation"></param>
    /// <returns></returns>
    public GameObject SpawnObjectReturn(Vector3 _position, Quaternion _rotation)
    {
        // Collect game object from the pool.
        GameObject gameObj = GetGameObject();

        // Check for null.
        if (gameObj == null)
            return null;

        // Set transorm position and rotation.
        gameObj.transform.position = _position;
        gameObj.transform.rotation = _rotation;

        // Return game object.
        return gameObj;
    }

    /// <summary>
    /// Used to fire projectiles
    /// </summary>
    /// <param name="firedBy"></param>
    /// <param name="prefab"></param>
    /// <param name="_position"></param>
    /// <param name="_rotation"></param>
    public void FireProjectile(int firedBy, GameObject prefab, Vector3 _position, Quaternion _rotation)
    {
        GameObject gameObj = SpawnObjectReturn(_position, _rotation);
        BaseProjectle baseProjectle = gameObj.GetComponent<BaseProjectle>();

        if (baseProjectle != null)
        {
            baseProjectle.OnProjectleSpawn(firedBy);
        }

        gameObj.SetActive(true);
    }

    /// <summary>
    /// Count the number of active objects within the pool.
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public int GetAmountOfActive()
    {
        int amount = 0;

        for (int i = 0; i < ObjectList.Count; i++)
        {
            if (ObjectList[i].activeInHierarchy)
                amount++;
        }

        return amount;
    }

    /*/// <summary>
    /// Count the number of non-active objects within the pool.
    /// </summary>
    /// <returns></returns>
    public int GetAmountOfNonActive()
    {
        int amount = 0;

        for (int i = 0; i < ObjectList.Count; i++)
        {
            if (!ObjectList[i].activeInHierarchy)
                amount++;
        }

        return amount;
    }**/
}
