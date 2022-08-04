using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    public class PoolManager : MonoBehaviour
    {
        #region Singlton Deisgn Patten
        static PoolManager _instance;
        public static PoolManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<PoolManager>();
                }

                return _instance;
            }
        }
        #endregion

        // Dictionarry of pooling class
        public Dictionary<int, PoolingClass> poolDictionary = new Dictionary<int, PoolingClass>();

        #region Pool Creation
        /// <summary>
        /// Create a new pool of a game objects.
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="startAmount"></param>
        /// <param name="hasLimit"></param>
        public void CreatePool(GameObject prefab, int startAmount)
        {
            // Set the pool key to the prefabs ID;
            int poolKey = prefab.GetInstanceID();

            // Check pool has not already been created.
            if (!poolDictionary.ContainsKey(poolKey))
            {
                // Create a holder.
                GameObject poolHolder = new GameObject(prefab.name + "_poolHolder");
                poolHolder.transform.parent = this.transform;

                poolDictionary.Add(poolKey, new PoolingClass(poolHolder, prefab, startAmount));
            }
        }

        /// <summary>
        /// Create a pool of game objects that cannot expand passed a limit
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="startAmount"></param>
        /// <param name="limit"></param>
        public void CreatePool(GameObject prefab, int startAmount, int limit)
        {
            int poolKey = prefab.GetInstanceID();

            GameObject poolHolder = new GameObject(prefab.name + "_PoolHolder");
            poolHolder.transform.parent = this.transform;

            if (!poolDictionary.ContainsKey(poolKey))
            {
                poolDictionary.Add(poolKey, new PoolingClass(poolHolder, prefab, startAmount));
            }
        }
        #endregion

        #region Object Spawn
        /// <summary>
        /// This methord is used to simply spawn a game object
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="postion"></param>
        /// <param name="rotation"></param>
        public void SpawnObject(GameObject prefab, Vector3 postion, Quaternion rotation)
        {
            int poolKey = prefab.GetInstanceID();

            if (poolDictionary.ContainsKey(poolKey))
            {
                PoolingClass poolingClass = poolDictionary[poolKey];
                poolingClass.SpawnObject(postion, rotation);
            }
        }

        /// <summary>
        /// The methord is used to spawn a game object and return.
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="postion"></param>
        /// <param name="rotation"></param>
        public GameObject SpawnObjectReturn(GameObject prefab, Vector3 postion, Quaternion rotation)
        {
            int poolKey = prefab.GetInstanceID();

            if (poolDictionary.ContainsKey(poolKey))
            {
                PoolingClass poolingClass = poolDictionary[poolKey];
                return poolingClass.SpawnObjectReturn(postion, rotation);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// This methord is used to fire a projectile.
        /// </summary>
        /// <param name="firedBy"></param>
        /// <param name="prefab"></param>
        /// <param name="postion"></param>
        /// <param name="rotation"></param>
        public void FireProjectile(int firedBy, GameObject prefab, Vector3 postion, Quaternion rotation)
        {
            int poolKey = prefab.GetInstanceID();

            if (poolDictionary.ContainsKey(poolKey))
            {
                PoolingClass poolingClass = poolDictionary[poolKey];
                poolingClass.FireProjectile(firedBy, prefab, postion, rotation);
            }
        }
        #endregion
    }
}