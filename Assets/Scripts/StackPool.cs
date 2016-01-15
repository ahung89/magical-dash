using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO: Add logic to start these off as inactive unless explicitly activated, e.g. if a level introduces a new enemy type
// or if a player acquires a new weapon.
public class StackPool : MonoBehaviour {

    public GameObject pooledObject;
    public int preloadAmount = 20;
    public bool allowGrowth = true;

    private Stack<GameObject> pooledObjects;

	void Start () {
        pooledObjects = new Stack<GameObject>();
        for (int i = 0; i < preloadAmount; i++)
        {
            GameObject obj = Instantiate(pooledObject) as GameObject;
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            pooledObjects.Push(obj);
        }
	}

    void OnLevelWasLoaded(int level)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public GameObject Pop()
    {
        if (pooledObjects.Count > 0)
            return pooledObjects.Pop();

        if (allowGrowth)
        {
            GameObject obj = Instantiate(pooledObject) as GameObject;
            obj.transform.SetParent(transform);
            return obj;
        }

        return null;
    }

    public void Push(GameObject obj)
    {
        pooledObjects.Push(obj);
    }
}
