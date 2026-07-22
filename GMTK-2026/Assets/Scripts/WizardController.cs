using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WizardController : MonoBehaviour
{
    [Header("Position Spring")]
    [SerializeField] float restingHeight = 0.4f;
    [SerializeField] float springStrength = 1.0f;
    [SerializeField] float springDamping = 0.1f;
    [SerializeField] float rayLength = 0.7f;

    [Header("Orientation Spring")]
    [SerializeField] float orientationSpringStrength = 4.0f;
    [SerializeField] float orientationSpringDamping = 0.2f;

    [Header("Movement")]
    [SerializeField] float maxSpeed = 20.0f;
    [SerializeField] float acceleration = 1.0f;
    [SerializeField] float maxAcceleration = 10.0f;
    [SerializeField] float jumpStrength = 1.0f;

    [Header("Object Grab Settings")]
    [SerializeField] private int grabCastCount = 50;
    [SerializeField] private float grabCastLength = 4.0f;
    [SerializeField] private float grabSphereRadius = 0.5f;
    [Range(0.2f, 10.0f)]
    [SerializeField] private float grabStrength = 1.0f;
    [SerializeField] private float grabLift = 1.0f;
    [SerializeField] private float grabDamping = 0.1f;
    [SerializeField] private float grabThrowForce = 5.0f; 
    [SerializeField] private float grabMoveSensitivity = 0.05f;
    [SerializeField] private float grabVerticalSensitivity = 1.5f;


    [Header("Gizmo Drawing")]
    [SerializeField] bool drawGrabCasting = false;
    [SerializeField] bool drawRelativeGrawPosition = false;
    [SerializeField] bool drawHighlightBodySelection = true;
    [SerializeField] float drawHighlightRadius = 0.75f;
    [SerializeField] bool drawSelectionBeizer = true;

    [Header("Assets")]
    [SerializeField] GameObject wizardModel;
    [SerializeField] Animator an;
    // NOTE: temp hate procedual animation testing
    [SerializeField] GameObject wizardHat;
    [SerializeField] GameObject spell3;
    [SerializeField] GameObject spell2;
    [SerializeField] GameObject spell1;

    Rigidbody rb;
    Vector3 movementInput;
    Vector3 lookInput;
    Vector3 velocity;
    Quaternion targetOrientation = Quaternion.identity;

    bool isRidingBroom = false;
    Rigidbody broomBody;
    FixedJoint broomJoint;

    // State
    bool grounded = true;
    bool dashing = true;

    void OnEnable() {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        GetInput();
        Jump();
        Dash();
        Spell3();
        Spell2();
        Spell1();
        AnimParameters();
    }

    void LateUpdate() {
        VisualTilt();
    }

    void FixedUpdate() {
        GrabForce();
        if (isRidingBroom) return;
        FloatForce();
        OrientationForce();
        MoveForce();
        DashForce();
    }

    void VisualTilt() {
        wizardModel.transform.rotation = Quaternion.AngleAxis(new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude*4.0f, transform.right)*transform.rotation;
        wizardHat.transform.rotation = Quaternion.AngleAxis(Mathf.Clamp(new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude*-5.0f, -20.0f, 20.0f), transform.right)*wizardModel.transform.rotation;
    }

    Vector3 dashVector = Vector3.zero;
    float dashTimer = 0.0f;
    void Dash() {
        if (Input.GetKeyDown(KeyCode.T)) {
            dashing = true;
            dashVector = transform.forward;
            dashVector = new Vector3(dashVector.x, 0, dashVector.z);
            dashTimer = 0.3f;
            rb.AddForce(dashVector, ForceMode.Impulse);
        }

        if (dashing) {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0.0f) {
                dashing = false;
            }
        }
    }

    [Header ("Spell 3")]
    [SerializeField] float spell3Cooldown;
    [SerializeField] float spell3Timer = 0.0f;

    void Spell3() {
        if (spell3Timer <= 0.0f)
        {
            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                Instantiate(spell3, gameObject.transform.position, Quaternion.identity);
                spell3Timer = spell3Cooldown;
            }
        } else
        {
            spell3Timer -= Time.deltaTime;
        }
    }

    [Header("Spell 2")]
    [SerializeField] float spell2Cooldown;
    [SerializeField] float spell2Timer = 0.0f;

    void Spell2()
    {
        if (spell2Timer <= 0.0f)
        {
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                Instantiate(spell2, gameObject.transform.position, Quaternion.identity);
                spell2Timer = spell2Cooldown;
            }
        } else
        {
            spell2Timer -= Time.deltaTime;
        }
    }

    [Header("Spell 1")]
    [SerializeField] float spell1Cooldown;
    [SerializeField] float spell1Timer = 0.0f;

    void Spell1()
    {
        if (spell1Timer <= 0.0f)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                Instantiate(spell1, gameObject.transform.position, Quaternion.identity);
                spell1Timer = spell1Cooldown;
            }
        } else
        {
            spell1Timer -= Time.deltaTime;
        }
    }

    void DashForce() {
        if (dashing) {
            rb.AddForce(dashVector*20.0f);
        }
    }

    Rigidbody selectedBody;
    Vector3 relativeGrabPosition;
    bool grabbing;

    void GrabForce() {
        if (grabbing) {
            Vector3 force = (relativeGrabPosition+transform.position+new Vector3(0,grabLift,0)) - selectedBody.position;
            force *= grabStrength;
            Vector3 damping = -selectedBody.linearVelocity*grabDamping;
            force += damping; 
            selectedBody.AddForce(force, ForceMode.Impulse);

            // Move object
            relativeGrabPosition += lookInput*grabMoveSensitivity;
        } else {
            FindBodySelection();
        }
    }

    void FindBodySelection() {
        RaycastHit[] raycastHits = new RaycastHit[grabCastCount];
        for (int i = 0; i < grabCastCount; i++) {
            float angle = (i/((float)grabCastCount))*Mathf.PI*2.0f;
            Ray ray = new Ray(transform.position, new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)));
            Physics.SphereCast(ray, grabSphereRadius, out raycastHits[i], grabCastLength);
        }
        // Find closest body
        Rigidbody closestBody = null;
        for (int i = 0; i < grabCastCount; i++) {
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

    void Jump() {
        if (Input.GetKeyDown(KeyCode.Space) && grounded) {
            rb.AddForce(Vector3.up*jumpStrength, ForceMode.Impulse);
            grounded = false;
        }
    }

    void GetInput() {
        movementInput = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")), 1);
        lookInput = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("LookHorizontal"), 0, Input.GetAxis("LookVertical")), 1);
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0,45,0));
        movementInput = matrix.MultiplyPoint3x4(movementInput);
        if (movementInput != Vector3.zero) {
            targetOrientation = Quaternion.LookRotation(movementInput, Vector3.up);
        }
        lookInput = matrix.MultiplyPoint3x4(lookInput);
    }

    void MoveForce() {
        Vector3 goalVelocity = movementInput * maxSpeed;
        velocity = Vector3.MoveTowards(velocity, goalVelocity, acceleration*Time.deltaTime);

        Vector3 neededAcceleration = (velocity-rb.linearVelocity)/Time.deltaTime;
        neededAcceleration = Vector3.ClampMagnitude(neededAcceleration, maxAcceleration);
        neededAcceleration.y = 0;
        rb.AddForce(neededAcceleration*rb.mass);
    }   

    void FloatForce() {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out hit, rayLength)) {


            Vector3 velocity = rb.linearVelocity;
            Vector3 rayDirection = Vector3.down;

            Vector3 otherVelocity = Vector3.zero;
            Rigidbody hitBody = hit.rigidbody;
            if (hitBody != null) {
                otherVelocity = hitBody.linearVelocity;
            }

            float velocityInRayDirection = Vector3.Dot(rayDirection, velocity);
            float otherVelocityInRayDirection = Vector3.Dot(rayDirection, otherVelocity);

            float relativeVelocity = velocityInRayDirection - otherVelocityInRayDirection;

            float x = hit.distance - restingHeight;

            float springForce = (x*springStrength) - (relativeVelocity*springDamping);

            rb.AddForce(rayDirection*springForce);
            // if ((rayDirection*springForce).magnitude > 30.0f || (rayDirection*springForce).magnitude < 0.0f) {
            //     Debug.Log("Force: " + rayDirection*springForce);
            //     Debug.Log("Velocity along ray:" + velocityInRayDirection);
            //     Debug.Log("Relative velocity:" + relativeVelocity);
            //     Debug.Log("x:" + x);
            // }

            if (hitBody != null) {
                hitBody.AddForceAtPosition(rayDirection*-springForce, hit.point);
            }
            grounded = true;
        }
        else {grounded = false;}
    }


    Quaternion rotationBetween(Quaternion from, Quaternion to) {
        Quaternion multiplyByScalar(Quaternion q, float s) {
            return new Quaternion(q.x*s, q.y*s, q.z*s, q.w*s);
        }
        if (Quaternion.Dot(to,from) < 0)
            return to*(multiplyByScalar(Quaternion.Inverse(from), -1.0f));
        else
            return to*Quaternion.Inverse(from);
    }

    void OrientationForce() {
        Quaternion currentOrientation = transform.rotation;
        Quaternion toGoal = rotationBetween(currentOrientation, targetOrientation);
        
        Vector3 rotationAxis;
        float rotationDegrees;

        toGoal.ToAngleAxis(out rotationDegrees, out rotationAxis);
        rotationAxis.Normalize();

        float rotationRadians = rotationDegrees*Mathf.Deg2Rad;

        rb.AddTorque((rotationAxis*(rotationRadians*orientationSpringStrength))-(rb.angularVelocity*orientationSpringDamping));
    }

    void OnDrawGizmos() {
        for (int i = 0; i < grabCastCount; i++) {
            float angle = (i/((float)grabCastCount))*Mathf.PI*2.0f;
            Vector3 rayPosition = transform.position;
            Vector3 rayDirection = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle))*grabCastLength;
            if (drawGrabCasting) Gizmos.DrawRay(rayPosition, rayDirection);
        }

        // Highlight selection
        if (selectedBody != null) {
            if (drawHighlightBodySelection) Gizmos.DrawWireSphere(selectedBody.position, drawHighlightRadius);
        }
        if (selectedBody != null && grabbing) { 
          if (drawRelativeGrawPosition) Gizmos.DrawSphere(relativeGrabPosition+transform.position+ new Vector3(0,grabLift,0), 0.2f);
        }  
        if (selectedBody != null && drawSelectionBeizer) {
            Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
            Handles.DrawBezier(transform.TransformPoint(new Vector3(-0.17f,-0.10f,0.2f)), selectedBody.transform.position, transform.forward+transform.position, (transform.position+selectedBody.transform.position)*0.5f, Color.white, EditorGUIUtility.whiteTexture, 1.0f);
        }
    }

    void AnimParameters() {
        an.SetFloat("Velocity", movementInput.magnitude);
    }

}
