using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class WatchParticleSystem : CPUParticleSystem {
    private Utils.Cooldown cooldown;

    public override void OnUpdateParticle(Particle particle) {
        particle.lifeTime += Time.fixedDeltaTime * particle.lifeTime * 5;
        particle.obj.transform.position = Vector3.MoveTowards(particle.obj.transform.position, transform.position, particle.lifeTime * Time.fixedDeltaTime);

        if (Vector3.Distance(particle.obj.transform.position, transform.position) < 0.001f) {
            particle.active = false;
            particle.obj.SetActive(false);
        }
    }

    void FixedUpdate() {
        UpdateParticles();
    }

    public void Emit(float cd) {
        if (cooldown.Ready(cd)) {
            Emit(transform.position + Utils.RandomPointOnSphere(Random.Range(0.2f, 0.4f)), Quaternion.identity, 0.5f);
        }
    }
}
