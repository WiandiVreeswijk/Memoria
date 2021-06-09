using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

//[ExecuteInEditMode]
public class ClothSimulator : MonoBehaviour {
    //[HideInInspector] public bool simulating = false;
    ////public bool simulating;
    //public int sphereGeneratorSize = 3;
    //public int meshSize = 10;
    //public float extraDepth = 0.0f;
    //Mesh mesh;

    //private void Start() {
    //    print(Application.isPlaying);
    //    MeshFilter filter = GetComponent<MeshFilter>();
    //    filter.mesh = new Mesh();
    //    mesh = filter.mesh;
    //    Spawn();
    //}

    //public void Simulate() {
    //    //MeshFilter filter = GetComponent<MeshFilter>();
    //    //filter.mesh = new Mesh();
    //    //mesh = filter.sharedMesh;
    //    //
    //    //simulating = true;
    //}

    //private void Generate() {
    //    int xSize = meshSize;
    //    int ySize = meshSize;
    //    var vertices = new Vector3[(xSize + 1) * (ySize + 1)];
    //    for (int i = 0, y = 0; y <= ySize; y++) {
    //        for (int x = 0; x <= xSize; x++, i++) {
    //            Vector3 pos = nodeMatrix[x, y].transform.position - transform.position;
    //            vertices[i] = pos / transform.localScale.x;
    //        }
    //    }
    //    mesh.vertices = vertices;

    //    int[] triangles = new int[xSize * ySize * 6];
    //    for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++) {
    //        for (int x = 0; x < xSize; x++, ti += 6, vi++) {
    //            triangles[ti] = vi;
    //            triangles[ti + 3] = triangles[ti + 2] = vi + 1;
    //            triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
    //            triangles[ti + 5] = vi + xSize + 2;
    //        }
    //    }
    //    mesh.triangles = triangles;

    //    mesh.RecalculateNormals(60);
    //    mesh.RecalculateTangents();
    //}

    //private void Update() {
    //    //if (simulating && nodeMatrix != null) {
    //    //    for (int x = 0; x < nodeMatrix.GetLength(0); x++) {
    //    //        for (int y = 0; y < nodeMatrix.GetLength(1); y++) {
    //    //            nodeMatrix[x, y].FixedUpdate();
    //    //        }
    //    //    }
    //    //
    //    //    foreach (var spring in springs) {
    //    //        spring.FixedUpdate();
    //    //    }
    //    //    foreach (var spring in diagonalSprings) {
    //    //        spring.FixedUpdate();
    //    //    }
    //    //    Generate();
    //    //}
    //    Generate();
    //}

    //GameObject CreateNode(string name) {
    //    GameObject node = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    //    node.name = name;
    //    node.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
    //    node.AddComponent<ClothNode>().mass = 1.0f;
    //    return node;
    //}

    //public GameObject springPrefab;
    //private ClothNode[,] nodeMatrix;
    //private List<ClothSpring> springs = new List<ClothSpring>();
    //private List<ClothSpring> diagonalSprings = new List<ClothSpring>();

    //public void Spawn() {
    //    Undo.SetCurrentGroupName("SpheresGenerationClothGroup");
    //    //float springStrength = 10.5f;
    //    float springStrength = 1050f;
    //    //float springSize = 1.0f / 1.1f;
    //    float springSize = 1f;
    //    nodeMatrix = new ClothNode[meshSize + 1, meshSize + 1];
    //    int l0 = nodeMatrix.GetLength(0);
    //    int l1 = nodeMatrix.GetLength(1);

    //    GameObject parent = new GameObject("parent");
    //    parent.transform.position = transform.position;
    //    float sizeComponent = (transform.localScale.x) / sphereGeneratorSize;
    //    Vector3 offset = new Vector3(transform.localScale.x / 2 - sizeComponent / 2 + sizeComponent, 0, transform.localScale.z / 2 - sizeComponent / 2 + sizeComponent);
    //    List<CircleCollision> spheres = new List<CircleCollision>();
    //    for (int xx = 0; xx < sphereGeneratorSize + 2; xx++) {
    //        for (int zz = 0; zz < sphereGeneratorSize + 2; zz++) {
    //            RaycastHit hit;
    //            Ray ray = new Ray(
    //                transform.position - offset + new Vector3(xx * sizeComponent, -0.1f, zz * sizeComponent),
    //                Vector3.down);
    //            //Debug.DrawRay(ray.origin, ray.direction, Color.red, 1);
    //            if (Physics.Raycast(ray, out hit)) {
    //                GameObject sphere = new GameObject(xx + " + " + zz);
    //                Undo.RegisterCreatedObjectUndo(sphere, "Sphere");
    //                CircleCollision collider = sphere.AddComponent<CircleCollision>();
    //                collider.radius = sizeComponent / 2 + 0.1f;
    //                sphere.transform.position = hit.point - new Vector3(0, extraDepth, 0);
    //                spheres.Add(collider);
    //            }
    //        }
    //    }

    //    //nested loop to create the fabric
    //    float sizeComponentMesh = (transform.localScale.x) / meshSize;
    //    Vector3 offsetMesh = new Vector3(transform.localScale.x / 2, 0, transform.localScale.z / 2);
    //    for (int x = 0; x < l0; x++) {
    //        for (int y = 0; y < l1; y++) {
    //            //create a new instance of a node at hte specific location name it and add the wall and circle to it
    //            GameObject tempObj = CreateNode("Node" + (x) + " " + (y));
    //            tempObj.transform.position = transform.position - offsetMesh +
    //                                         new Vector3(x * sizeComponentMesh, 0, y * sizeComponentMesh);
    //            Undo.RegisterCreatedObjectUndo(tempObj, "nodeObj");
    //            tempObj.transform.parent = parent.transform;
    //            nodeMatrix[x, y] = tempObj.GetComponent<ClothNode>();
    //            nodeMatrix[x, y].spheres = spheres;
    //        }
    //    }
    //    //loop through the matrix and attach a spring to each pair of nodes
    //    for (int i = 0; i < nodeMatrix.GetLength(0); i++) {
    //        for (int j = 0; j < nodeMatrix.GetLength(1); j++) {
    //            //make the top row anchors for cloth
    //            if (i == 0) {
    //                //nodeMatrix[i, j].GetComponent<NodeScrypt>().isAnchor = true;
    //            }
    //            //attach a spring to the row abov it 
    //            else {
    //                GameObject tempObj = Instantiate(springPrefab);
    //                Undo.RegisterCreatedObjectUndo(tempObj, "spring");
    //                ClothSpring spring = tempObj.GetComponent<ClothSpring>();
    //                spring.node1 = nodeMatrix[i, j];
    //                spring.node2 = nodeMatrix[i - 1, j];
    //                tempObj.name = "spring: " + i + " " + j + " - " + (i - 1) + " " + (j);
    //                spring.startPlay(springSize, springStrength);
    //                tempObj.transform.parent = parent.transform;
    //                springs.Add(spring);
    //            }
    //            //attach a spring to every colum but the first
    //            if (j != 0) {
    //                GameObject tempObj = Instantiate(springPrefab);
    //                Undo.RegisterCreatedObjectUndo(tempObj, "spring");
    //                ClothSpring spring = tempObj.GetComponent<ClothSpring>();
    //                spring.node1 = nodeMatrix[i, j];
    //                spring.node2 = nodeMatrix[i, j - 1];
    //                tempObj.name = "spring: " + i + " " + j + " - " + i + " " + (j - 1);
    //                spring.startPlay(springSize, springStrength);
    //                tempObj.transform.parent = parent.transform;
    //                springs.Add(spring);
    //                //attach a quad to the bottom right node & attach the two diaogonal springs
    //                if (i != 0) {
    //                    GameObject tempObj2 = Instantiate(springPrefab);
    //                    Undo.RegisterCreatedObjectUndo(tempObj2, "spring");
    //                    ClothSpring spring2 = tempObj2.GetComponent<ClothSpring>();
    //                    spring2.node1 = nodeMatrix[i, j];
    //                    spring2.node2 = nodeMatrix[i - 1, j - 1];
    //                    tempObj2.name = "spring: " + i + " " + j + " - " + i + " " + (j - 1);
    //                    spring2.startPlay(springSize, springStrength);
    //                    tempObj2.transform.parent = parent.transform;
    //                    diagonalSprings.Add(spring2);
    //                    springs.Add(spring2);
    //                }
    //                if (i != nodeMatrix.GetLength(0) - 1) {
    //                    GameObject tempObj2 = Instantiate(springPrefab);
    //                    Undo.RegisterCreatedObjectUndo(tempObj2, "spring");
    //                    ClothSpring spring2 = tempObj2.GetComponent<ClothSpring>();
    //                    spring2.node1 = nodeMatrix[i, j];
    //                    spring2.node2 = nodeMatrix[i + 1, j - 1];
    //                    tempObj2.name = "spring: " + i + " " + j + " - " + i + " " + (j - 1);
    //                    tempObj2.GetComponent<ClothSpring>().startPlay(springSize, springStrength);
    //                    tempObj2.transform.parent = parent.transform;
    //                    springs.Add(spring2);
    //                    diagonalSprings.Add(spring2);
    //                }
    //            }
    //        }
    //    }

    //    parent.transform.rotation = transform.rotation;
    //    transform.rotation = Quaternion.identity;
    //    //begin the sipmulation for each node
    //    for (int i = 0; i < nodeMatrix.GetLength(0); i++) {
    //        for (int j = 0; j < nodeMatrix.GetLength(1); j++) {
    //            nodeMatrix[i, j].GetComponent<ClothNode>().beginSim();
    //        }
    //    }
    //    Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
    //}
}
