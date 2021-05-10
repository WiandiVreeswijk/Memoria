using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    #region variables
    public Transform cam;
    private CharacterController controller;

    [Header("Hover over object")]
    public Material hoverMaterial;
    private GameObject hoverObject = null;
    private GameObject previousHitObject = null;

    [Header("Animation")]
    Vector3 oldPos = new Vector3();
    private Animator animatorElena;


    private float horizontal, vertical;
    [Header("Movement Variables")]
    [Range(0, 6f)] public float speed = 3f;
    [Range(0, 1f)] public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    float gravity;

    private Vector3 rightFootPosition, leftFootPosition, leftFootIkPosition, rightFootIkPosition;
    private Quaternion leftFootIkRotation, rightFootIkRotation;
    private float lastPelvisPositionY, lastRightFootPositionY, lastLeftFootPositionY;
    [Header("Feet Grounder")]
    public bool enableFeetIk = true;
    [Range(0, 2)] [SerializeField] private float heightFromGroundRaycast = 1.14f;
    [Range(0, 2)] [SerializeField] private float raycastDownDistance = 1.5f;
    [SerializeField] private LayerMask environmentLayer;
    [SerializeField] private float pelvisOffset = 0f;
    [Range(0, 1)] [SerializeField] private float pelvisUpAndDownSpeed = 0.28f;
    [Range(0, 1)] [SerializeField] private float feetToIkPositionSpeed = 0.5f;

    public string leftFootAnimVariableName = "LeftFootCurve";
    public string rightFootAnimVariableName = "RightFootCurve";

    public bool useProIkFeature = false;
    public bool showSolverDebug = true;

    #endregion

    private void Start() {
        animatorElena = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        oldPos = transform.position;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update() {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }
    //bool created = false;

    private void FixedUpdate() {
        Movement();
        Interaction();
        Animation();
        FixedUpdateFunction();
    }
    #region Movement
    private void Movement() {
        gravity -= 9.8f * Time.fixedDeltaTime;
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        if (direction.magnitude >= 0.1f) {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Vector3 moveDirection = new Vector3(moveDir.x, gravity, moveDir.z);
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);


            if (controller.isGrounded) {
                gravity = 0f;
            }
        }
    }
    #endregion

    #region Interaction
    private void Interaction() {
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
    }

    #endregion

    #region Animation
    private void Animation() {
        Vector3 difference = transform.position - oldPos;
        float mag = difference.magnitude;

        if (mag >= 0.01f) {
            animatorElena.SetBool("ElenaMoving", true);
        } else if (mag <= 0.005f) {
            animatorElena.SetBool("ElenaMoving", false);
        }
        oldPos = transform.position;
    }
    #endregion

    #region FeetGrounding

    private void FixedUpdateFunction() {
        if (enableFeetIk == false) { return; }
        if (animatorElena == null) { return; }

        AdjustFeetTarget(ref rightFootPosition, HumanBodyBones.RightToes);
        AdjustFeetTarget(ref leftFootPosition, HumanBodyBones.LeftToes);

        FeetPositionSolver(rightFootPosition, ref rightFootIkPosition, ref rightFootIkRotation);
        FeetPositionSolver(leftFootPosition, ref leftFootIkPosition, ref leftFootIkRotation);
    }

    private void OnAnimatorIK(int layerIndex) {
        if (enableFeetIk == false) { return; }
        if (animatorElena == null) { return; }
        MovePelvisHeight();

        //right foot ik position and rotation -- utilise the pro features in here
        animatorElena.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);

        if (useProIkFeature) {
            animatorElena.SetIKRotationWeight(AvatarIKGoal.RightFoot, animatorElena.GetFloat(rightFootAnimVariableName));
        }
        MoveFeetToIkPoint(AvatarIKGoal.RightFoot, rightFootIkPosition, rightFootIkRotation, ref lastRightFootPositionY);

        //left foot ik position and rotation -- utilise the pro features in here
        animatorElena.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);

        if (useProIkFeature) {
            animatorElena.SetIKRotationWeight(AvatarIKGoal.LeftFoot, animatorElena.GetFloat(leftFootAnimVariableName));
        }
        MoveFeetToIkPoint(AvatarIKGoal.LeftFoot, leftFootIkPosition, leftFootIkRotation, ref lastLeftFootPositionY);
    }
    #endregion

    #region FeetGroundingMethods
    void MoveFeetToIkPoint(AvatarIKGoal foot, Vector3 positionIkHolder, Quaternion rotationIkHolder, ref float lastFootPositionY) {
        Vector3 targetIkPosition = animatorElena.GetIKPosition(foot);

        if (positionIkHolder != Vector3.zero) {
            targetIkPosition = transform.InverseTransformPoint(targetIkPosition);
            positionIkHolder = transform.InverseTransformPoint(positionIkHolder);

            float yVariable = Mathf.Lerp(lastFootPositionY, positionIkHolder.y, feetToIkPositionSpeed);
            targetIkPosition.y += yVariable;

            lastFootPositionY = yVariable;

            targetIkPosition = transform.TransformPoint(targetIkPosition);
            animatorElena.SetIKRotation(foot, rotationIkHolder);
        }
        //Vector3 temp = targetIkPosition + new Vector3(0, offset, 0);
        animatorElena.SetIKPosition(foot, targetIkPosition);
    }
    private void MovePelvisHeight() {
        if (rightFootIkPosition == Vector3.zero || leftFootIkPosition == Vector3.zero || lastPelvisPositionY == 0) {
            lastPelvisPositionY = animatorElena.bodyPosition.y;
            return;
        }

        float lOffsetPosition = leftFootIkPosition.y - transform.position.y;
        float rOffsetPosition = rightFootIkPosition.y - transform.position.y;

        float totalOffset = (lOffsetPosition < rOffsetPosition) ? lOffsetPosition : rOffsetPosition;

        Vector3 newPelvisPosition = animatorElena.bodyPosition + Vector3.up * totalOffset;

        newPelvisPosition.y = Mathf.Lerp(lastPelvisPositionY, newPelvisPosition.y, pelvisUpAndDownSpeed);

        animatorElena.bodyPosition = newPelvisPosition;

        lastPelvisPositionY = animatorElena.bodyPosition.y;
    }
    private void FeetPositionSolver(Vector3 fromSkyPosition, ref Vector3 feetIkPositions, ref Quaternion feetIkRotations) {
        RaycastHit feetOutHit;

        if (showSolverDebug)
            Debug.DrawLine(fromSkyPosition, fromSkyPosition + Vector3.down * (raycastDownDistance + heightFromGroundRaycast), Color.yellow);

        if (Physics.Raycast(fromSkyPosition, Vector3.down, out feetOutHit, raycastDownDistance + heightFromGroundRaycast, environmentLayer)) {
            //finding our feet ik positions from the sky position
            feetIkPositions = fromSkyPosition;
            feetIkPositions.y = feetOutHit.point.y + pelvisOffset;
            feetIkRotations = Quaternion.FromToRotation(Vector3.up, feetOutHit.normal) * transform.rotation;
            return;
        }

        feetIkPositions = Vector3.zero;
    }

    private void AdjustFeetTarget(ref Vector3 feetPositions, HumanBodyBones foot) {
        //if (animatorElena.GetBoneTransform(foot) == null) print(foot + " is null");
        //feetPositions = animatorElena.GetBoneTransform(foot).position;
        feetPositions = new Vector3(0, 0, 0);
        feetPositions.y = transform.position.y + heightFromGroundRaycast;
    }
    #endregion

    private void OnCollisionEnter(Collision collision) {
        print(collision.gameObject.name);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(rightFootIkPosition, 0.05f);
        Gizmos.DrawSphere(leftFootIkPosition, 0.05f);
        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(lastPelvisPositionY, 0.05f);
    }
}
