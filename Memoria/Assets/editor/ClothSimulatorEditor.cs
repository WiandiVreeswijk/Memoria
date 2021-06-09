using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ClothSimulator))]
[CanEditMultipleObjects]
public class ClothSimulatorEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ClothSimulator simulator = target as ClothSimulator;
        //if (GUILayout.Button("Reset")) {
        //}

        //if (GUILayout.Button("GenerateSpheres")) {
            //Undo.SetCurrentGroupName("SpheresGenerationClothGroup");
            //Undo.RecordObject(target, "SpheresGenerationMainObject");
            //Cloth cloth = Undo.AddComponent(simulator.gameObject, typeof(Cloth)) as Cloth;
            //float sizeComponent = (simulator.transform.localScale.x) / simulator.size;
            //Vector3 offset = new Vector3(simulator.transform.localScale.x / 2 - sizeComponent / 2, 0, simulator.transform.localScale.z / 2 - sizeComponent / 2);
            //ClothSphereColliderPair[] pairs = new ClothSphereColliderPair[simulator.size * simulator.size];
            //int index = 0;
            //for (int x = 0; x < simulator.size; x++) {
            //    for (int z = 0; z < simulator.size; z++) {
            //        RaycastHit hit;
            //        Ray ray = new Ray(simulator.transform.position - offset + new Vector3(x * sizeComponent, -0.1f, z * sizeComponent), Vector3.down);
            //        //Debug.DrawRay(ray.origin, ray.direction, Color.red, 1);
            //        if (Physics.Raycast(ray, out hit)) {
            //            GameObject sphere = new GameObject(x + " + " + z);
            //            SphereCollider collider = sphere.AddComponent<SphereCollider>();
            //            collider.radius = sizeComponent / 2;
            //            pairs[index] = new ClothSphereColliderPair(collider);
            //            index++;
            //
            //            sphere.transform.position = hit.point;
            //            sphere.transform.parent = simulator.transform;
            //            Undo.RegisterCreatedObjectUndo(sphere, "SpheresGenerationCloth");
            //        }
            //    }
            //}
            //cloth.sphereColliders = pairs;
            //Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
        //}

        //if (GUILayout.Button("Test")) {
        //    simulator.Spawn();
        //}
        //if (GUILayout.Button("Simulate")) {
        //    simulator.Simulate();
        //}
    }
}
