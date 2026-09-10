using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpillEssenceOnHit : MonoBehaviour
{
    [SerializeField] GameObject essenceSpillObject;
    [SerializeField] HealthScript health;
    [SerializeField] float moveOut;

    List<GameObject> essence = new List<GameObject>();
    List<Vector3> offset = new List<Vector3>();
    
    
    float lastHealth;
    void Start()
    {
        lastHealth = health.Health;
    }
    void Update()
    {
        for (int i = 0; i < essence.Count; i++)
        {
            essence[i].transform.position = transform.position + offset[i];
        }

        if (lastHealth != health.Health)
        {
            RemoveEssence(SpillEssence(), 1.5f);
            lastHealth = health.Health;
        }
    }

    IEnumerator RemoveEssence(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        for (int i = 0; i < essence.Count; i++)
        {
            if (essence[i] == obj)
            {
                essence.Remove(obj);
                offset.RemoveAt(i);
            }

        }
    }

    GameObject SpillEssence()
    {
        GameObject obj = Instantiate(essenceSpillObject, transform.position, transform.rotation, transform);
        obj.GetComponent<particallEssenceGive>().moveForawrd = moveOut;
        obj.GetComponent<particallEssenceGive>().StartEssence();
        offset.Add(obj.transform.localPosition);
        obj.transform.parent = null;
        essence.Add(obj);
        return obj;
    }
}
