using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworksSpawner : MonoBehaviour {
    public GameObject fireworksPrefab;
    private bool active = false;

    private int index = 0;

    Color RandomC(float min = 0) {
        float rr = Random.Range(0, 128);
        float gg = Random.Range(0, 128);
        float bb = Random.Range(0, 128);
        if (rr + gg + bb < min) return RandomC(min);
        return new Color(rr, gg, bb);
    }

    public void SetActive() {
        if (active) return;
        FMODUnity.RuntimeManager.CreateInstance("event:/AMBIENT/Fireworks").start();
        active = true;
    }

    void FixedUpdate() {
        if (!active) return;
        if (index++ % 10 < 1) {
            Vector3 randomPoint = Random.onUnitSphere;
            GameObject obj = Instantiate(fireworksPrefab,
                transform.position + new Vector3((randomPoint.x + randomPoint.y) * transform.localScale.x, -2.0f,
                    (randomPoint.z + randomPoint.y) * transform.localScale.z), Quaternion.Euler(Random.Range(-95, -85), 0, 0));
            float scale = Random.Range(0.1f, 0.25f);
            obj.transform.localScale = new Vector3(scale, scale, scale);
            var psr = obj.GetComponent<ParticleSystemRenderer>();
            Material mat = psr.material;
            mat.SetColor("_EmissionColor", RandomC() * 0.1f);
            psr.material = mat;
            Destroy(obj, 2.5f);
        }
    }
}
