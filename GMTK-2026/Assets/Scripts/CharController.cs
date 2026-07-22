using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator an;
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed = 360;
    [SerializeField] private float jumpStrength = 1.5f;
    [SerializeField] private float jumpSustainStrength = 0.02f;
    [SerializeField] private float jumpDownForce = 0.02f;

    [Header("Grab Selection Raycast Settings")]
    [SerializeField] private int castCount = 50;
    [SerializeField] private float castLength = 4.0f;
    
    [Header("Grab Force Settings")]
    [Range(0.2f, 10.0f)]
    [SerializeField] private float grabStrength = 1.0f;
    [SerializeField] private float grabLift = 1.0f;
    [SerializeField] private float grabDamping = 0.1f;
    [SerializeField] private float throwForce = 5.0f; 
    [Header("Grab Movement Settings")]
    [SerializeField] private float grabSensitivity = 0.05f;
    private Vector3 movementInput;
    private Vector3 lookInput;
    private bool grounded;
    private bool jumping;
    [Header("Gizmo Drawing")]
    [SerializeField] bool drawGrabCasting = false;
    [SerializeField] bool drawRelativeGrawPosition = false;
    [SerializeField] bool drawHighlightBodySelection = true;

    void Update() {
        GetInput();
        Move();
        Grab();
        AnimParameters();
    }

    void GetInput() {
        movementInput = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")), 1);
        lookInput = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("LookHorizontal"), 0, Input.GetAxis("LookVertical")), 1);
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0,45,0));
        movementInput = matrix.MultiplyPoint3x4(movementInput);
        lookInput = matrix.MultiplyPoint3x4(lookInput);
    }

    void Move() {
        rb.AddForce(movementInput * speed * Time.deltaTime, ForceMode.Impulse);   
        //Rotation
        if (movementInput != Vector3.zero) {
            var rot = Quaternion.LookRotation(movementInput, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,rot,turnSpeed*Time.deltaTime);
        }

        // Jumping
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, 0.51f)) {
            grounded = true;
            jumping = false;
        } else {
            grounded = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded) {
            rb.AddForce(Vector3.up*jumpStrength, ForceMode.Impulse);
            transform.position = transform.position + Vector3.up*0.1f;
            jumping = true;
        }
        if (Input.GetKey(KeyCode.Space) && jumping) {
            rb.AddForce(Vector3.up*jumpSustainStrength, ForceMode.Impulse);
        }
        if (rb.linearVelocity.y < 0 && jumping && !Input.GetKey(KeyCode.Space)) {
            rb.AddForce(Vector3.down*jumpDownForce, ForceMode.Impulse);
        }
    }

    private Rigidbody selectedBody;
    private Vector3 relativeGrabPosition;
    private bool grabbing;
    void Grab() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (grabbing) {
                // give object players velocity on dropping
                selectedBody.AddForce(rb.linearVelocity, ForceMode.Impulse);
                // add throw force to object
                selectedBody.AddForce(movementInput.normalized*throwForce, ForceMode.Impulse);
                Debug.Log(rb.linearVelocity);
                grabbing = false;
                return;
            }
            FindBodySelection();
            if (selectedBody != null)
                grabbing = true;
        }

        if (grabbing) {
            Vector3 force = (relativeGrabPosition+transform.position+new Vector3(0,grabLift,0)) - selectedBody.position;
            force *= grabStrength;
            Vector3 damping = -selectedBody.linearVelocity*grabDamping;
            force += damping; 
            selectedBody.AddForce(force, ForceMode.Impulse);

            // Move object
            relativeGrabPosition += lookInput*grabSensitivity;
        } else {
            FindBodySelection();
        }
    }

    void FindBodySelection() {
        RaycastHit[] raycastHits = new RaycastHit[castCount];
        for (int i = 0; i < castCount; i++) {
            float angle = (i/((float)castCount))*Mathf.PI*2.0f;
            Ray ray = new Ray(transform.position, new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)));
            Physics.Raycast(ray, out raycastHits[i], castLength);
        }
        // Find closest body
        Rigidbody closestBody = null;
        for (int i = 0; i < castCount; i++) {
            Rigidbody body = raycastHits[i].rigidbody;
            if (closestBody == null && body != null) {
                closestBody = body;
                continue;
            }
            if (closestBody != null && body != null) {
                if (Vector3.Distance(transform.position, closestBody.position) > Vector3.Distance(transform.position, body.position)) 
                    closestBody = body;
            }
        }

        selectedBody = closestBody;
        if (selectedBody != null ) {
            relativeGrabPosition = selectedBody.transform.position - transform.position;
        }
    }

    void OnDrawGizmos() {
        for (int i = 0; i < castCount; i++) {
            float angle = (i/((float)castCount))*Mathf.PI*2.0f;
            Vector3 rayPosition = transform.position;
            Vector3 rayDirection = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle))*castLength;
            if (drawGrabCasting) Gizmos.DrawRay(rayPosition, rayDirection);
        }

        // Highlight selection
        if (selectedBody != null) {
            if (drawHighlightBodySelection) Gizmos.DrawWireSphere(selectedBody.position, 1.0f);
        }
        if (selectedBody != null && grabbing) { 
          if (drawRelativeGrawPosition) Gizmos.DrawSphere(relativeGrabPosition+transform.position+ new Vector3(0,grabLift,0), 0.2f);
        }   
    }

    void AnimParameters() {
        an.SetFloat("Velocity", movementInput.magnitude);
    }


}
