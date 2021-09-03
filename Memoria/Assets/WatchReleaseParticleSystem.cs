using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using Random = UnityEngine.Random;

public class WatchReleaseParticleSystem : CPUParticleSystem
{
    private Utils.Cooldown cooldown;
    
    [NonSerialized]
    public GameObject targetObject;

    public override void OnUpdateParticle(Particle particle) {
        particle.lifeTime += Time.fixedDeltaTime * particle.lifeTime * 5;
        particle.obj.transform.position = Vector3.MoveTowards(particle.obj.transform.position, targetObject.transform.position, particle.lifeTime * Time.fixedDeltaTime);

        if (Vector3.Distance(particle.obj.transform.position, targetObject.transform.position) < 0.001f) {
            particle.active = false;
            particle.obj.SetActive(false);
        }
    }

    void FixedUpdate() {
        UpdateParticles();
        if(targetObject != null)Emit(0.05f);
    }

    public void Emit(float cd) {
        if (cooldown.Ready(cd)) {
            Emit(transform.position, Quaternion.identity, 1.5f);
        }
    }
}
