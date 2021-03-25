using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
    /*Find instances of unique scripts. Used in Globals*/
    public static T FindUniqueScript<T>() where T : Object {
        var objects = GameObject.FindObjectsOfType<T>();
        if (objects.Length == 0) throw new System.Exception($"No instances of unique script {typeof(T).Name} found");
        if (objects.Length > 1) throw new System.Exception($"Too many instances of unique script {typeof(T).Name} found");
        return objects[0];
    }
}
