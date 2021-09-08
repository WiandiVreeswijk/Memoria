using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oma : MonoBehaviour
{
    void Start()
    {
        Globals.IconManager.AddWorldIcon("oma", transform.position + new Vector3(0, 1, 0));
    }
}
