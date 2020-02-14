using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePoolManager : MonoBehaviour {

    // Stores the gameobjects for the pool
    private Dictionary<int, Queue<ObjectInstance>> objectPool = new Dictionary<int, Queue<ObjectInstance>>();
    public GameObject bulletPrefab;
    public int BULLET_POOL_SIZE = 20;

    private static GamePoolManager instance;

    public static GamePoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GamePoolManager>();
            }
            return instance;
        }
    }

    public void Start()
    {
        CreateBulletPool();
    }

    public void CreatePool(GameObject prefab, int poolSize)
    {
        int poolKey = prefab.GetInstanceID();

        GameObject poolHolder = new GameObject(prefab.name + "pool");
        poolHolder.transform.parent = transform;

        if (!objectPool.ContainsKey(poolKey))
        {
            objectPool.Add(poolKey, new Queue<ObjectInstance>());

            for (int i = 0; i < poolSize; i++)
            {
                ObjectInstance newObject = new ObjectInstance(Instantiate(prefab) as GameObject);
                objectPool[poolKey].Enqueue(newObject);
                newObject.SetParent(poolHolder.transform);
            }
        }
    }

    private void CreateBulletPool()
    {
        int poolKey = bulletPrefab.GetInstanceID();
        GameObject pool = new GameObject(bulletPrefab.name + "pool");
        pool.transform.parent = transform;
        objectPool.Add(poolKey, new Queue<ObjectInstance>());
        for (int i = 0; i < BULLET_POOL_SIZE; i++)
        {
            ObjectInstance obj = new ObjectInstance(Instantiate(bulletPrefab) as GameObject);
            objectPool[poolKey].Enqueue(obj);
            obj.SetParent(pool.transform);
        }
    }

    public ObjectInstance GetBulletToShoot()
    {
        int poolKey = bulletPrefab.GetInstanceID();
        ObjectInstance obj = objectPool[poolKey].Dequeue();
        objectPool[poolKey].Enqueue(obj);
        return obj;
    }

    public void ReuseObject(GameObject prefab, Vector3 position, Vector3 target, Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID();

        if (objectPool.ContainsKey(poolKey))
        {
            ObjectInstance objectToReuse = objectPool[poolKey].Dequeue();
            objectPool[poolKey].Enqueue(objectToReuse);

            objectToReuse.Reuse(position, target, rotation);

        }
    }

    public class ObjectInstance
    {
        GameObject gameObject;
        Transform transform;

        bool hasPoolObjectComponent;
        PoolObject poolObjectScript;

        public ObjectInstance(GameObject objectInstance)
        {
            gameObject = objectInstance;
            transform = gameObject.transform;
            gameObject.SetActive(false);

            if (gameObject.GetComponent<PoolObject>())
            {
                hasPoolObjectComponent = true;
                poolObjectScript = gameObject.GetComponent<PoolObject>();
            }
        }

        public void Reuse(Vector3 position, Vector3 target, Quaternion rotation)
        {
            if (hasPoolObjectComponent)
            {
                poolObjectScript.OnObjectReuse();
            }

            gameObject.SetActive(true);
            if (hasPoolObjectComponent)
                poolObjectScript.SetDirection(target);
            transform.position = position;
            transform.rotation = rotation;
        }

        public void SetParent(Transform parent)
        {
            transform.parent = parent;
        }
    }
}
