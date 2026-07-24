using UnityEditor;
using UnityEngine;

public enum SelectedSpell {
    Melee, Three, Two, One
};

[RequireComponent(typeof(Rigidbody))]
public class WizardController : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] float maxHealth;
    [SerializeField] public float health;
    [SerializeField] float dashCooldown;
    public int keyCount = 0;
    
    [Header("Position Spring")]
    [SerializeField] float restingHeight = 0.4f;
    [SerializeField] float springStrength = 1.0f;
    [SerializeField] float springDamping = 0.1f;
    [SerializeField] float rayLength = 0.7f;

    [Header("Orientation Spring")]
    [SerializeField] float orientationSpringStrength = 4.0f;
    [SerializeField] float orientationSpringDamping = 0.2f;

    [Header("Movement")]
    [SerializeField] float walkMaxSpeed = 3.0f;
    [SerializeField] float walkAcceleration = 10.0f;
    [SerializeField] float walkMaxAcceleration = 10.0f;

    [SerializeField] float runMaxSpeed = 6.0f;
    [SerializeField] float runAcceleration = 2.0f;
    [SerializeField] float runMaxAcceleration = 10.0f;
    // [SerializeField] float jumpStrength = 1.0f;

    // [Header("Object Grab Settings")]
    // [SerializeField] private int grabCastCount = 50;
    // [SerializeField] private float grabCastLength = 4.0f;
    // [SerializeField] private float grabSphereRadius = 0.5f;
    // [Range(0.2f, 10.0f)]
    // [SerializeField] private float grabStrength = 1.0f;
    // [SerializeField] private float grabLift = 1.0f;
    // [SerializeField] private float grabDamping = 0.1f;
    // [SerializeField] private float grabThrowForce = 5.0f; 
    // [SerializeField] private float grabMoveSensitivity = 0.05f;
    // [SerializeField] private float grabVerticalSensitivity = 1.5f;


    // [Header("Gizmo Drawing")]
    // [SerializeField] bool drawGrabCasting = false;
    // [SerializeField] bool drawRelativeGrawPosition = false;
    // [SerializeField] bool drawHighlightBodySelection = true;
    // [SerializeField] float drawHighlightRadius = 0.75f;
    // [SerializeField] bool drawSelectionBeizer = true;

    [Header("Assets")]
    [SerializeField] GameObject wizardModel;
    [SerializeField] Animator an;
    // NOTE: temp hate procedual animation testing
    [SerializeField] GameObject wizardHat;
    [SerializeField] GameObject cam_Pivot;
    [SerializeField] Animator animator;
    [SerializeField] GameObject spell3;
    [SerializeField] GameObject spell2;
    [SerializeField] GameObject spell1;

    Rigidbody rb;
    Vector3 movementInput;
    Vector3 velocity;
    Quaternion targetOrientation = Quaternion.identity;
    Vector3 mousePos;
    public SelectedSpell spell = SelectedSpell.Melee;
    [SerializeField] Melee meleeScript;

    public bool aiming = false;
    public bool attacking = false;

    // State
    // bool grounded = true;
    bool dashing = true;

    void OnEnable() {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        GetInput();
        Attack();
        // Jump();
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
        FloatForce();
        OrientationForce();
        MoveForce();
        // DashForce();
    }

    void VisualTilt() {
        wizardModel.transform.rotation = Quaternion.AngleAxis(new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude*4.0f, transform.right)*transform.rotation;
        if (!Input.GetKey(KeyCode.LeftShift))
            wizardHat.transform.rotation = Quaternion.AngleAxis(Mathf.Clamp(new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude*-5.0f, -20.0f, 20.0f), transform.right)*wizardModel.transform.rotation;
    }

    Vector3 dashVector = Vector3.zero;
    float dashTimer = 0.0f;
    float dashCooldownTimer = 0.0f;

    void Dash() {
        // if (dashCooldownTimer <= 0)
        // {
        //     if (Input.GetKeyDown(KeyCode.LeftShift))
        //     {
        //         dashing = true;
        //         // dashVector = gameObject.GetComponent<Rigidbody>().linearVelocity.normalized;
        //         // dashVector = new Vector3(dashVector.x, 0, dashVector.z);
        //         dashVector = movementInput;
        //         Ray ray = new Ray(transform.position, movementInput);
        //         RaycastHit hit;
        //         // if (Physics.Raycast(ray, out hit, 1.0f, ~(1 << 7))) {
        //         //     Debug.Log(hit.collider.gameObject.name);
        //         //     transform.position = hit.point;
        //         // } else {
        //         //     transform.position = transform.position + movementInput.normalized * 1.0f;
        //         // }
        //         dashTimer = 0.3f;
        //         rb.AddForce(dashVector*10.0f, ForceMode.Impulse);
        //         dashCooldownTimer = dashCooldown;
        //     }
        // } else
        // {
        //     dashCooldownTimer -= Time.deltaTime;

        // }

        // if (dashing) {
        //     dashTimer -= Time.deltaTime;
        //     if (dashTimer <= 0.0f) {
        //         rb.AddForce(-dashVector*10.0f, ForceMode.Impulse);
        //         dashing = false;
        //     }
        // }
    }

    void Attack() {
        if (Input.GetMouseButtonDown(0) && aiming) {
            animator.SetTrigger("Melee");
            meleeScript.Attack();
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
                Instantiate(spell3, gameObject.transform.position, gameObject.transform.rotation);
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
                Instantiate(spell2, gameObject.transform.position, gameObject.transform.rotation);
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
                Instantiate(spell1, new Vector3(mousePos.x, 1, mousePos.z), Quaternion.identity);
                spell1Timer = spell1Cooldown;
            }
        } else
        {
            spell1Timer -= Time.deltaTime;
        }
    }

    void DashForce() {
        // if (dashing) {
        //     rb.AddForce(dashVector*20.0f);
        // }
    }

    Rigidbody selectedBody;
    Vector3 relativeGrabPosition;
    bool grabbing;

    // void Jump() {
    //     if (Input.GetKeyDown(KeyCode.Space) && grounded) {
    //         rb.AddForce(Vector3.up*jumpStrength, ForceMode.Impulse);
    //         grounded = false;
    //     }
    // }
    Ray RayFromCursor()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height, 0f));
        return ray;
    }

    void GetInput() {
        movementInput = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")), 1);
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0,90,0));
        movementInput = matrix.MultiplyPoint3x4(movementInput);

        aiming = Input.GetMouseButton(1);
        attacking = Input.GetMouseButton(0);

        // Mouse input
        RaycastHit hit;
        Physics.Raycast(RayFromCursor(), out hit, Mathf.Infinity, 1 << 7);
        mousePos = hit.point;
        Vector3 lookDir = (mousePos - transform.position).normalized;
        lookDir.y = 0;
        
        if (aiming)
            targetOrientation = Quaternion.LookRotation(lookDir, Vector3.up);
        else if (movementInput.magnitude > 0.1f)
            targetOrientation = Quaternion.LookRotation(movementInput, Vector3.up);
    }

    void MoveForce() {
        float maxSpeed = 0.0f;
        float acceleration = 0.0f;
        float maxAcceleration = 0.0f;
        if (Input.GetKey(KeyCode.LeftShift)) {
            maxSpeed = runMaxSpeed;
            acceleration = runAcceleration;
            maxAcceleration = runMaxAcceleration;
        } else {
            maxSpeed = walkMaxSpeed;
            acceleration = walkAcceleration;
            maxAcceleration = walkMaxAcceleration;
        }

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
            // grounded = true;
        }
        // else {grounded = false;}
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

    // void OnDrawGizmos() {
    //     for (int i = 0; i < grabCastCount; i++) {
    //         float angle = (i/((float)grabCastCount))*Mathf.PI*2.0f;
    //         Vector3 rayPosition = transform.position;
    //         Vector3 rayDirection = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle))*grabCastLength;
    //         if (drawGrabCasting) Gizmos.DrawRay(rayPosition, rayDirection);
    //     }

        // Highlight selection
    //     if (selectedBody != null) {
    //         if (drawHighlightBodySelection) Gizmos.DrawWireSphere(selectedBody.position, drawHighlightRadius);
    //     }
    //     if (selectedBody != null && grabbing) { 
    //       if (drawRelativeGrawPosition) Gizmos.DrawSphere(relativeGrabPosition+transform.position+ new Vector3(0,grabLift,0), 0.2f);
    //     }  
    //     if (selectedBody != null && drawSelectionBeizer) {
    //         Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
    //         Handles.DrawBezier(transform.TransformPoint(new Vector3(-0.17f,-0.10f,0.2f)), selectedBody.transform.position, transform.forward+transform.position, (transform.position+selectedBody.transform.position)*0.5f, Color.white, EditorGUIUtility.whiteTexture, 1.0f);
    //     }
    // }

    void AnimParameters() {
        if (Input.GetKey(KeyCode.LeftShift)) {
            an.SetFloat("Velocity", movementInput.magnitude);
        }
        else {
            an.SetFloat("Velocity", movementInput.magnitude*0.5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Camera Trigger")
        {
            cam_Pivot.GetComponent<CameraController>().target = other.gameObject;
        }
    }
}
