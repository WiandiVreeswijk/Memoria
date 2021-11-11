using System;
using System.Diagnostics;
using UnityEngine;

public class StopwatchWrapper {
    private Stopwatch sw;

    public StopwatchWrapper() {
        sw = new Stopwatch();
        sw.Start();
    }

    public void Print(string name = "") {
        sw.Stop();
        if (name.Length == 0) UnityEngine.Debug.Log($"Took {sw.ElapsedMilliseconds}ms");
        else UnityEngine.Debug.Log($"{name} took {sw.ElapsedMilliseconds}ms");
        sw.Restart();
    }
}
