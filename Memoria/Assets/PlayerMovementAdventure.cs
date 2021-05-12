using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementAdventure : MonoBehaviour {
    [Header("Movement")]
    [SerializeField] public float rotationSpeed = 5.0f;
    [SerializeField] public float timeToRunMultiplier = 1.0f;
    private float runVariableSmoothed = 0.0f;
    private Vector3 previousMovement = Vector3.zero;
    private Vector3 smoothedRotatedMovement = Vector3.zero;
    private Vector3 plainRotatedMovement = Vector3.zero;
    private Vector3 previousPlainMovement = Vector3.zero;
    private Transform cam;
    private Animator animator;
    private void Start() {
        cam = Camera.main.transform;
        animator = GetComponent<Animator>();
    }

    void Update() {
        float forwardSpeed = 0.0f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool isRunning = false;
        Vector3 movement = previousMovement * 0.85f;
        Vector3 plainMovement = new Vector3(horizontal, 0, vertical);
        float dot = Vector3.Dot(previousPlainMovement, plainMovement);
        if (dot < 0) movement = Vector3.zero;
        previousPlainMovement = plainMovement;
        if (plainMovement != Vector3.zero) {
            movement += plainMovement;
            Quaternion temporaryCameraRotation = cam.rotation;
            cam.eulerAngles = new Vector3(0, cam.eulerAngles.y, 0);
            smoothedRotatedMovement = cam.TransformDirection(movement).normalized;
            plainRotatedMovement = cam.TransformDirection(plainMovement);
            cam.rotation = temporaryCameraRotation;

            if (dot < 0) transform.rotation = Quaternion.LookRotation(smoothedRotatedMovement);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(smoothedRotatedMovement), Time.deltaTime * rotationSpeed);
            forwardSpeed = smoothedRotatedMovement.magnitude;
            if (Input.GetKey(KeyCode.LeftShift)) isRunning = true;
        }
        previousMovement = movement;

        runVariableSmoothed = Mathf.Clamp01(runVariableSmoothed + ((isRunning ? timeToRunMultiplier : -timeToRunMultiplier) * Time.deltaTime));
        animator.SetFloat("Forward", forwardSpeed);
        animator.SetFloat("Running", runVariableSmoothed);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + smoothedRotatedMovement);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + plainRotatedMovement);
    }
}
