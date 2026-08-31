using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AddToEneamyTarget : MonoBehaviour
{
    void Start()
    {
        if (AILos.targets.Contains(this.gameObject)) { return; }
        AILos.targets.Add(this.gameObject);
    }
}
