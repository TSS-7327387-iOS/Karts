using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ObjectPoolManager : MonoBehaviour
{

    public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();
    private GameObject _objectPoolEmptyHolder;
    private static GameObject _particleSystemsempty;
    private static GameObject _gameObjectsEmpty;
   
    public enum PoolType
    {
        ParticleSystem,
        GameObject,
        none
    }
    public static PoolType PoolingType;
    private void Awake()
    {
        
    }
    private void SetUpEmpties()
    {
        _objectPoolEmptyHolder = new GameObject("Pooled Objects");

        _particleSystemsempty = new GameObject("Particle Effects");
        _particleSystemsempty.transform.SetParent(_objectPoolEmptyHolder.transform);

        _gameObjectsEmpty = new GameObject("GaneObjects");
        _gameObjectsEmpty.transform.SetParent(_objectPoolEmptyHolder.transform);
    }
    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation,PoolType poolType=PoolType.none)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);

       

        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }


        //check if there are any inactive objects in the pool
        GameObject spawnableObj = pool.InactiveGameObjects.FirstOrDefault();

      

        if (spawnableObj == null)
        {
            //find the parent of Empty Object
            GameObject parentObject = SetParentObject(poolType);

            //if there are no inactive objects ,create a new one
            spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation);

            if (parentObject != null)
            {
                spawnableObj.transform.SetParent(parentObject.transform); 
            }
        }
        else
        {
            //if there is an inactive object ,receive it
            spawnableObj.transform.position = spawnPosition;
            spawnableObj.transform.rotation = spawnRotation;
            pool.InactiveGameObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }
        return spawnableObj;
    }
    public static GameObject SpawnObject(GameObject objectToSpawn,Transform parentTransform)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);



        if (pool == null)
        {
            pool = new PooledObjectInfo() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }


        //check if there are any inactive objects in the pool
        GameObject spawnableObj = pool.InactiveGameObjects.FirstOrDefault();



        if (spawnableObj == null)
        {
          

            //if there are no inactive objects ,create a new one
            spawnableObj = Instantiate(objectToSpawn, parentTransform);

            
        }
        else
        {
            //if there is an inactive object ,receive it
            
            pool.InactiveGameObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }
        return spawnableObj;
    }


    public static void ReturnObjectToPool(GameObject obj)
    {
        string goName = obj.name.Substring(0, obj.name.Length - 7);//by taking off 7,we are removing the (Clone) from the name of the passed in obj
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookupString == goName);
        if (pool == null)
        {
            Debug.LogWarning("trying to release an object that is not pooled "+obj.name);
        }
        else
        {
            obj.SetActive(false);
           


            pool.InactiveGameObjects.Add(obj);
        }
    
    }




    private static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.ParticleSystem:
                return _particleSystemsempty;
            case PoolType.GameObject:
                return _gameObjectsEmpty;
            case PoolType.none:
                return null;
            default: return null;


        }
    }
}
public class PooledObjectInfo
{
    public string LookupString;
    public List<GameObject> InactiveGameObjects = new List<GameObject>();
}
