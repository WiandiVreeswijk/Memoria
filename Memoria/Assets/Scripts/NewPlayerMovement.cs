using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour {
    private Animator anim;
    void Start() {
        anim = GetComponent<Animator>();
    }

    void Update() {
        Turning();
        Move();
    }

    private void Turning() {
        anim.SetFloat("Turn", Input.GetAxis("Horizontal"));
    }

    private void Move() {
        bool shift = Input.GetKey(KeyCode.LeftShift);
        anim.SetBool("Running", shift);
        float speed = Input.GetAxis("Vertical");
        anim.SetFloat("Forward", shift ? speed / 2 : speed);
    }

}
