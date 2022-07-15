using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ColorMixer {

    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool SharedInstance;
        public List<GameObject> pooledObjects;
        public List<GameObject> objectsToPool;
        public int amountToPool;

        void Awake()
        {
            SharedInstance = this;
        }

        void Start()
        {
            pooledObjects = new List<GameObject>();
            GameObject tmp;
            for(int i = 0; i < amountToPool; i++)
            {
                for(int j = 0; j < objectsToPool.Count; j++) {
                    tmp = Instantiate(objectsToPool[j]);
                    tmp.SetActive(false);
                    pooledObjects[i] = tmp;
                }
            }
        }

        public GameObject GetPooledObject()
        {
            for(int i = 0; i < pooledObjects.Count; i++)
            {
                // if(!pooledObjects[i].activeInHierarchy)
                // {
                    return pooledObjects[i];
                //}
            }
            return null;
        }
    }
}
