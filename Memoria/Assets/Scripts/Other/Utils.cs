using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

public static class Utils {
    /*Find instances of objects. Mostly used in Globals*/
    #region ObjectFinding
    private static T CheckObjects<T>(T[] collection, string name, bool errorWhenNoneFound = true) where T : class {
        if (errorWhenNoneFound && collection.Length == 0) Debug.LogError($"No instances of unique type {name} could be found. This will probably break a lot of stuff!");
        if (collection.Length == 1) return collection[0];
        if (collection.Length > 1) Debug.LogError($"Multiple instances of unique type {name} could be found. This will probably break a lot of stuff!");
        return null;
    }

    private static T CheckTransforms<T>(T[] collection, string transformName, string name, bool errorWhenNoneFound = true) where T : class {
        if (errorWhenNoneFound && collection.Length == 0) Debug.LogError($"No children of {transformName} named {name} could be found. This will probably break a lot of stuff!");
        if (collection.Length == 1) return collection[0];
        if (collection.Length > 1) Debug.LogError($"Multiple children of {transformName} named {name} could be found. This will probably break a lot of stuff!");
        return null;
    }

    public delegate T InstantiateCallback<T>();
    public static void FindOrInstantiateUniqueObject<T>(out T obj, InstantiateCallback<T> callback) where T : Object {
        obj = CheckObjects(GameObject.FindObjectsOfType<T>(), typeof(T).Name, false);
        if (obj == null) {
            //Debug.Log($"Object of type {typeof(T).Name} instantiated because it could not be found in scene.");
            obj = callback();
        }
    }

    public static bool FindUniqueObject<T>(out T obj, bool errorWhenNoneFound = true) where T : Object {
        obj = CheckObjects(GameObject.FindObjectsOfType<T>(), typeof(T).Name, errorWhenNoneFound);
        return obj != null;
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

    /*Logs an error when les or more than one instance of the type is found in the scene*/
    public static void EnsureOnlyOneInstanceInScene<T>() where T : Object {
        FindUniqueObject<T>();
    }

    public static Transform FindUniqueChildInTransform(Transform transform, string name) {
        var children = transform.FindChildren(name);
        return CheckTransforms(children, transform.name, name, true);
    }

    #endregion

    #region Math

    /*Maps a float from one range to another*/
    public static float Remap(float val, float minIn, float maxIn, float minOut, float maxOut) {
        return minOut + (val - minIn) * (maxOut - minOut) / (maxIn - minIn);
    }


    /*Distance between two values*/
    public static float Distance(float one, float two) {
        return Mathf.Abs(one - two);
    }
    #endregion

    public static Vector3 EvaluateQuadratic(Vector3 a, Vector3 b, Vector3 c, float t) {
        Vector3 p0 = Vector3.Lerp(a, b, t);
        Vector3 p1 = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(p0, p1, t);
    }

    public static Vector3 EvaluateCubic(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t) {
        Vector3 p0 = EvaluateQuadratic(a, b, c, t);
        Vector3 p1 = EvaluateQuadratic(b, c, d, t);
        return Vector3.Lerp(p0, p1, t);
    }

    public static Vector3 RandomPointOnSphereEdge(float radius) {
        return UnityEngine.Random.insideUnitSphere.normalized * radius;
    }

    public static Vector3 RandomPointOnSphere(float radius) {
        return UnityEngine.Random.insideUnitSphere * radius;
    }

    #region Timing

    public static Sequence DelayedAction(float delay, Action action) {
        return DOTween.Sequence().AppendInterval(delay).AppendCallback(() => action());
    }

    public static Sequence RepeatAction(int amount, float delay, Action action) {
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < amount; i++) {
            sequence.AppendCallback(() => action());
            if (i != amount - 1) sequence.AppendInterval(delay);
        }
        return sequence;
    }

    public struct Cooldown {
        public float time;

        public bool Ready(float time) {
            bool ready = this.time + time <= Time.time;
            if (ready) this.time = Time.time;
            return ready;
        }
    }

    #endregion

    #region Extentions
    /*Remove all matches from a dictionary*/
    public static void RemoveAll<K, V>(this Dictionary<K, V> dict, Func<K, V, bool> match) {
        foreach (var key in dict.Keys.ToArray().Where(key => match(key, dict[key]))) dict.Remove(key);
    }
    #endregion

    public static Transform[] FindChildren(this Transform transform, string name) {
        return transform.GetComponentsInChildren<Transform>().Where(t => t.name == name).ToArray();
    }
}
