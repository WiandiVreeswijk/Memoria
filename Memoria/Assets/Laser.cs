using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Laser : MonoBehaviour {
    public MeshRenderer lampRenderer;
    public MeshRenderer fogRenderer;
    public Light lamp;

    private Material lampMaterial;
    private Material fogMaterial;

    private Color activeColor;
    private float activeWidth;

    public bool flipped = false;

    private int id = 0;
    private static int globalID = 0;
    // Start is called before the first frame update
    void Start() {
        id = globalID++;
        lampMaterial = lampRenderer.material;
        fogMaterial = fogRenderer.material;

        SetColor(Color.white);

        SetConeWidth(0.0f);

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            DOTween.Kill(id);
            SetConeWidth(0.0f);
            SetColor(Color.magenta);
            SetRotation(new Vector3(0, 180, 0));

            TweenRotation(new Vector3(0, 180, flipped ? 25 : -25), 2.0f, Ease.Flash);
            tweenConeWidth(1.0f, 2.0f, Ease.Flash).OnComplete(() => {
                GoToRandomGoal();
            });
        }
    }


    private void GoToRandomGoal() {
        bool random = Random.Range(0, 10) > 7;
        bool random2 = Random.Range(0, 10) > 8;
        float r = Random.Range(1.0f, 4.0f);
        //float r = random ? Random.Range(0.1f, 0.2f) : Random.Range(1.0f, 4.0f);
        float r2 = Random.Range(1.0f, 4.0f);
        //float r2 = random ? Random.Range(0.1f, 0.2f) : Random.Range(1.0f, 4.0f);
        tweenColor(GetRandomColor() * 1.5f, r, Ease.Linear);
        TweenRotation(GetRandomGoal(), r2, Ease.Linear).OnComplete(GoToRandomGoal);
        if (random2) {
            tweenConeWidth(Random.Range(0.1f, 1.0f), Random.Range(0.3f, 0.5f), Ease.Flash);
        }
    }

    private Color GetRandomColor() {
        switch (Random.Range(0, 5)) {
            case 0: return Color.red;
            case 1: return Color.blue;
            case 2: return GetHTMLColor("#EA75AD");
            case 3: return GetHTMLColor("#B500FF");
            case 4: return GetHTMLColor("#EB00FF");
        }

        return Color.black;
    }

    private Color GetHTMLColor(string col) {
        Color color = Color.black;
        ColorUtility.TryParseHtmlString(col, out color);
        return color;
    }

    private Vector3 GetRandomGoal() {
        return new Vector3(Random.Range(-30, 30), 180f, flipped ? Random.Range(60, 4) : Random.Range(-60, -4));
    }

    private void SetRotation(Vector3 rotation) {
        transform.rotation = Quaternion.Euler(rotation);
    }
    private Tween TweenRotation(Vector3 rotation, float duration, Ease ease) {
        return transform.DORotate(rotation, duration).SetEase(ease).SetId(id);
    }
    private Tween tweenColor(Color color, float duration, Ease ease) {
        return DOTween.To(() => activeColor, x => SetColor(x), color, duration).SetEase(ease).SetId(id);
    }

    public void SetColor(Color color) {
        activeColor = color;
        Color fogColor = new Color(color.r, color.g, color.b, 0.25f);
        lamp.color = color * 2.0f;
        lampMaterial.SetColor("_BaseColor", color);
        lampMaterial.SetColor("_EmissionColor", color);
        fogMaterial.color = fogColor;
    }

    private Tween tweenConeWidth(float width, float duration, Ease ease) {
        return DOTween.To(() => activeWidth, x => SetConeWidth(x), width, duration).SetEase(ease).SetId(id);
    }

    public void SetConeWidth(float width) {
        activeWidth = width;
        lampRenderer.transform.localScale = new Vector3(0.5f * width, 0.2f, 0.5f * width);
        fogRenderer.transform.localScale = new Vector3(0.4f * width, 0.75f, 0.4f * width);
        lamp.innerSpotAngle = 20 * width;
        lamp.spotAngle = lamp.innerSpotAngle + 2;
        fogMaterial.SetFloat("_ConeWidth", 6 * width);
    }
}
