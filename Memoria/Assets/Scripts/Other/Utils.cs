using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

public static class Utils {
    /*Find instances of objects. Mostly used in Globals*/
    #region ObjectFinding
    private static T CheckObjects<T>(T[] collection, string name) where T : class {
        if (collection.Length == 0) Debug.LogError($"No instances of unique type {name} could be found. This will probably break a lot of stuff!");
        if (collection.Length == 1) return collection[0];
        if (collection.Length > 1) Debug.LogError($"Multiple instances of unique type {name} could be found. This will probably break a lot of stuff!");
        return null;
    }

    public static T FindUniqueObject<T>() where T : Object {
        return CheckObjects(GameObject.FindObjectsOfType<T>(), typeof(T).Name);
    }

    public static GameObject FindUniqueGameObjectWithTag(string tag) {
        return CheckObjects(GameObject.FindGameObjectsWithTag(tag), tag);
    }

    public static T FindUniqueObjectWithTag<T>(string tag) where T : Component {
        return FindUniqueGameObjectWithTag(tag)?.GetComponent<T>();
    }
    #endregion

    public static void DelayedAction(float delay, Action action) {
        DOTween.Sequence().AppendInterval(delay).AppendCallback(() => action());
    }

    /*Logs an error when les or more than one instance of the type is found in the scene*/
    public static void EnsureOnlyOneInstanceInScene<T>() where T : Object {
        FindUniqueObject<T>();
    }
}
