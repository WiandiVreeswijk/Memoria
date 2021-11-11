using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class ProgressionUtils : MonoBehaviour {
    public static IEnumerable<Type> GetEnumerableOfType<T>() where T : class {
        List<Type> objects = new List<Type>();
        foreach (Type type in
            Assembly.GetAssembly(typeof(T)).GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)))) {
            objects.Add(type);
        }
        objects.Sort();
        return objects;
    }
}
