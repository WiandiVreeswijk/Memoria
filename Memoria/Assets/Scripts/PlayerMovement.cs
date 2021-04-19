using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public Material hoverMaterial;
    private GameObject hoverObject = null;
    private GameObject previousHitObject = null;

    public Animator animatorElena;
    Vector3 oldPos = new Vector3();

    private void Start() {
        oldPos = transform.position;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update() {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f) {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }
    //bool created = false;

    private void FixedUpdate() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(0.5F, 0.5F, 0));
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        GameObject hitObject = null;
        if (Physics.Raycast(ray, out hit)) {
            Transform objectHit = hit.transform;
            if (objectHit.gameObject.tag == "Interactable" && previousHitObject != objectHit.gameObject) {
                hitObject = objectHit.gameObject;
                previousHitObject = hitObject;
                print("object hit");

                //created = true;
                hoverObject = Instantiate(objectHit.gameObject);
                hoverObject.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
                hoverObject.GetComponent<MeshRenderer>().material = hoverMaterial;
                hoverObject.GetComponent<BoxCollider>().enabled = false;
            }
        }

        if (hitObject != previousHitObject) {
            Destroy(hoverObject);
            hoverObject = null;
            print("object destroyed");
        }

        previousHitObject = hitObject;


        Vector3 difference = transform.position - oldPos;
        float mag = difference.magnitude;
        if(mag >= 0.05f)
        {
            animatorElena.SetBool("ElenaMoving", true);
        }
        else if(mag <= 0.01f)
        {
            animatorElena.SetBool("ElenaMoving", false);
        }
        oldPos = transform.position;

        print(mag);
    }
}
