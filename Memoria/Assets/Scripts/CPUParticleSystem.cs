using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle {
    public GameObject obj;
    public float lifeTime;
    public bool active = false;
    public object extraData = null;
}

public class CPUParticleSystem : MonoBehaviour {
    public GameObject prefab;
    public int maxParticles = 50;

    private int particleIndex;
    private Particle[] particles;

    void Start() {
        particles = new Particle[maxParticles];
        for (int i = 0; i < maxParticles; i++) particles[i] = new Particle();
        GameObject particleSystemParent = new GameObject(name + "_ParticleSystem");
        foreach (Particle p in particles) {
            p.obj = Instantiate(prefab, particleSystemParent.transform);
            p.obj.SetActive(false);
        }
    }

    public void UpdateParticles() {
        foreach (Particle particle in particles) {
            if (particle.active) {
                OnUpdateParticle(particle);
            }
        }
    }

    public virtual void OnUpdateParticle(Particle particle) { }

    public void Emit(Vector3 position, Quaternion rotation, float lifeTime, float particleSize = 0.05f) {
        Collider[] colliders = Physics.OverlapSphere(position, particleSize);
        if (colliders.Length == 0) {
            particleIndex++;
            if (particleIndex >= maxParticles) particleIndex = 0;

            Particle p = particles[particleIndex];
            p.obj.transform.position = position;
            p.obj.transform.rotation = rotation;
            p.lifeTime = lifeTime;
            p.active = true;
            p.obj.SetActive(true);
        }
    }
}
