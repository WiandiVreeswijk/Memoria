using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootPlacement : MonoBehaviour {
    Animator anim;

    [Range(-1f, 1f)] public float distanceToGround;
    [Range(-1f, 1f)] public float feetYOffset;
    [Range(0f, 1f)] public float maxFootHeight = 0.5f;
    [Range(0f, 1f)] public float minFootHeight = 0.5f;
    public bool debugDraw;
    public LayerMask layerMask;

    void Start() {
        anim = GetComponent<Animator>();
    }

    void Update() {

    }

    private Vector3 leftFootPosition;
    private Vector3 rightFootPosition;
    private void OnAnimatorIK(int layerIndex) {
        if (anim) {
            leftFootPosition = DoIKFoot(AvatarIKGoal.LeftFoot, "IKLeftFootWeight");
            rightFootPosition = DoIKFoot(AvatarIKGoal.RightFoot, "IKRightFootWeight");
        }
    }

    private Vector3 DoIKFoot(AvatarIKGoal foot, string weight) {
        anim.SetIKPositionWeight(foot, 1 - anim.GetFloat(weight));
        anim.SetIKRotationWeight(foot, 1 - anim.GetFloat(weight));
        //anim.SetIKPositionWeight(foot, 1);
        //anim.SetIKRotationWeight(foot, 1);

        RaycastHit hit;
        Vector3 originalIKPos = anim.GetIKPosition(foot);
        if(debugDraw) Debug.DrawLine(originalIKPos + (Vector3.up * maxFootHeight), originalIKPos + (Vector3.down * minFootHeight), Color.yellow);
        Ray ray = new Ray(originalIKPos + (Vector3.up * maxFootHeight), Vector3.down);
        if (Physics.Raycast(ray, out hit, distanceToGround + 1f, layerMask)) {
            Vector3 footPosition = hit.point;
            footPosition.y += distanceToGround;
            footPosition.y += feetYOffset;
            anim.SetIKPosition(foot, footPosition);
            anim.SetIKRotation(foot, Quaternion.LookRotation(transform.forward, hit.normal));
            return footPosition;
        }
        return anim.GetIKPosition(foot);
    }

    private void OnDrawGizmos() {
        if(!debugDraw)return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(leftFootPosition, 0.1f);
        Gizmos.DrawWireSphere(rightFootPosition, 0.1f);
    }
}
