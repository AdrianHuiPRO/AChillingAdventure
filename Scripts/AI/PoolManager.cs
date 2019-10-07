using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{
    public static class PoolManager
    {
        //changed list to Queue
        private static Dictionary<int, Queue<GameObject>> _ObjectPools = new Dictionary<int, Queue<GameObject>>();                
        //Starting pool size
        private static int _InitialPoolSize = 1;
        //Adding objects to the pool
        private static GameObject AddObjectToPool(GameObject prefab)
        {
            var id = prefab.GetInstanceID();
            var clone = GameObject.Instantiate(prefab);
            _ObjectPools[id].Enqueue(clone);
            clone.gameObject.SetActive(false);
            return clone;
        }

        //Instantiating the pool in the scene
        public static void CreatePool(GameObject prefab, int poolSize)
        {
            //is getting the ID of each instance of the prefab
            var id = prefab.GetInstanceID();
            //creating a new pool to contain game objects
            _ObjectPools[id] = new Queue<GameObject>(poolSize);
            if(_ObjectPools.ContainsKey(id))
            {
                for (int i = 0; i < poolSize; i++)
                {
                    AddObjectToPool(prefab);
                }
            }
        }

        public static GameObject GetObjectFromPool(GameObject prefab)
        {
            var id = prefab.GetInstanceID();
            //Creates pool when called
            if(_ObjectPools.ContainsKey(id) == false)
            {
                CreatePool(prefab, _InitialPoolSize);
            }
            //Detecting if the game needs more of the prefab
            for(int i = 0; i < _ObjectPools[id].Count; i++)
            {
                GameObject reusePrefab = _ObjectPools[id].Dequeue();
                _ObjectPools[id].Enqueue(reusePrefab);
                //detecting whether there's a prefab not active in the hierarchy
                if(reusePrefab.gameObject.activeInHierarchy == false)
                {
                    return reusePrefab;
                }
            }
            //returns the prefab
            return AddObjectToPool(prefab);
        }
    }
}

