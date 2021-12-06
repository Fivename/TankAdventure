using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ObjectPool
{
    private static ObjectPool instance;
    private Dictionary<string,Queue<GameObject>> objectPool= new Dictionary<string,Queue<GameObject>>();
    //父物体pool存放对象池中的对象 防止窗口杂乱
    private GameObject pool;
    public static ObjectPool Instance
    {
        get{
            if(instance == null)
            {
                instance = new ObjectPool();
            }
            return instance;
        }
    }
    public GameObject GetObject(GameObject prefab)
    {
        GameObject _object;
        //检查对象池中是否包含该名字的预制体，再检查池中的 待分配物体数
        if(!objectPool.ContainsKey(prefab.name) || objectPool[prefab.name].Count == 0)
        {
            _object = GameObject.Instantiate(prefab);   
            //如果对象池（字典键值对）不存在该名字的物体或者对象池中没有待分配物体就实例化一个新的物体并使用push函数放入池中
            PushObject(_object);
            //然后判断场景中是否存在对象池的父物体 pool 不存在就创建一个
            if(pool == null)
            {                   
                pool = new GameObject("ObjectPool");
                GameObject.DontDestroyOnLoad(pool);
            }
            GameObject child = GameObject.Find(prefab.name);
            //查找场景中是否存在子对象池的父物体 如不存在则用预制体的名字创建新物体 并设为对象池物体的子物体
            if (!child)
            {
                child = new GameObject(prefab.name);
                child.transform.SetParent(pool.transform);  
            }
            _object.transform.SetParent(child.transform);
        }
        _object = objectPool[prefab.name].Dequeue();
        _object.SetActive(true);
        return _object;
    }
    //得到预制体 在字典队列中加入预制体
    public void PushObject(GameObject prefab)
    {
        string _name = prefab.name.Replace("(Clone)", string.Empty);
        //如果没有叫name的键 就在对象池中加入name的队列
        if (!objectPool.ContainsKey(_name))
        {
            objectPool.Add(_name, new Queue<GameObject>());
        }
        //入列
        objectPool[_name].Enqueue(prefab);
        prefab.SetActive(false);
    }
}
