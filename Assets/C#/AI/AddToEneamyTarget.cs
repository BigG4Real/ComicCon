using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AddToEneamyTarget : MonoBehaviour
{
    void Start()
    {
        if (AILos.targets.Contains(this.gameObject)) { AILos.targets.Remove(this.gameObject); }
        AILos.targets.Add(this.gameObject);
        for (int i = 0; i < AILos.targets.Count; i++)
        {
            if(AILos.targets[i] == null)
            {
                AILos.targets.RemoveAt(i);
                i--;
            } 
        }
    }
}
