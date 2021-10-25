using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Icon : MonoBehaviour {
    public GameObject iconObject;
    public GameObject iconZObject;
    public TextMesh distanceText;
    public TextMesh distanceTextZ;
    private List<KeyValuePair<Material, float>> materials = new List<KeyValuePair<Material, float>>(6);
    public float size = 1.0f;
    public float minScale = 1.0f;
    public float maxScale = 1.0f;

    private const float MIN_FADE_DISTANCE = 4f;
    private const float MAX_FADE_DISTANCE = 10f;

    public void Start() {
        Globals.UIManager.IndicatorManager.CreateIndicator(gameObject);
        gameObject.SetActive(false);
    }

    public void SetImage(Texture texture) {
        Material mat = iconObject.GetComponent<MeshRenderer>().material;
        Material matZ = iconZObject.GetComponent<MeshRenderer>().material;
        mat.SetTexture("_BaseMap", texture);
        matZ.SetTexture("_BaseMap", texture);

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers) {
            materials.Add(new KeyValuePair<Material, float>(renderer.material, renderer.material.color.a));
        }
    }

    public void SetText(string text) {
        distanceText.text = text;
        distanceTextZ.text = text;
    }

    public void SetPosition(Vector3 position) {
        transform.position = position;
    }

    void LateUpdate() {
        float distance = Vector3.Distance(Globals.Camera.transform.position, transform.position);
        float playerDistance = Vector3.Distance(Globals.Player.transform.position, transform.position);
        float remapedPlayerDistance = Utils.Remap(Mathf.Clamp(playerDistance, MIN_FADE_DISTANCE, MAX_FADE_DISTANCE), MIN_FADE_DISTANCE, MAX_FADE_DISTANCE, 0f, 1f);
        SetText(Mathf.Round(distance) + "M");
        float scale = (Globals.Camera.transform.position - transform.position).magnitude * size;
        scale = Mathf.Clamp(scale, minScale, maxScale);
        transform.localScale = new Vector3(scale, scale, scale);

        foreach (KeyValuePair<Material, float> mat in materials) {
            Color color = mat.Key.color;
            color.a = remapedPlayerDistance * mat.Value;
            mat.Key.color = color;
        }
    }

    public void SetEnabled(bool toggle) {
        gameObject.SetActive(toggle);
    }
}