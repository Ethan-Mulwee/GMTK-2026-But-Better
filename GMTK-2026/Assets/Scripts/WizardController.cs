using System.Collections;
using UnityEditor;
using UnityEngine;

public enum SelectedSpell {
    Melee, Three, Two, One
};

[RequireComponent(typeof(Rigidbody))]
public class WizardController : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] public float maxHealth;
    [SerializeField] public float health;
    [SerializeField] public float maxStamina;
    [SerializeField] public float stamina;
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
    [SerializeField] float jumpStrength = 1.0f;
    [SerializeField] public CameraController cameraController;
    public GameObject swipe;
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
    bool grounded = true;
    public SelectedSpell spell = SelectedSpell.Melee;
    [SerializeField] Melee meleeScript;

    public bool aiming = false;
    public bool attacking = false;

    public void SetSpell(int spellID) {
        switch(spellID) {
            case 1: 
                spell = SelectedSpell.One;
                break;
            case 2: 
                spell = SelectedSpell.Two;
                break;
            case 3: 
                spell = SelectedSpell.Three;
                break;
        }
    }

    // State
    // bool grounded = true;
    bool jumped = false;
    bool dashing = true;
    public bool spell3Enabled = false;
    public bool spell2Enabled = false;
    public bool spell1Enabled = false;

    void OnEnable() {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        GetInput();
        Attack();
        Jump();
        Dash();
        // Spell3();
        // Spell2();
        // Spell1();
        AnimParameters();


        if (Input.GetKeyDown(KeyCode.Alpha3) && spell3Enabled) {
            if (spell == SelectedSpell.Three) {
                spell = SelectedSpell.Melee;
            }
            else {
                spell = SelectedSpell.Three;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && spell2Enabled) {
            if (spell == SelectedSpell.Two) {
                spell = SelectedSpell.Melee;
            }
            else {
                spell = SelectedSpell.Two;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && spell1Enabled) {
            if (spell == SelectedSpell.One) {
                spell = SelectedSpell.Melee;
            }
            else {
                spell = SelectedSpell.One;
            }
        }

        spell1Timer -= Time.deltaTime;
        spell2Timer -= Time.deltaTime;
        spell3Timer -= Time.deltaTime;
    }

    void Jump() {
        if (Input.GetKeyDown(KeyCode.Space) && grounded) {
            rb.AddForce(Vector3.up*jumpStrength, ForceMode.Impulse);
            grounded = false;
            an.SetTrigger("Jump");
            jumped = true;
            // Debug.Log("Set jumped");
        }
    }

    void LateUpdate() {
        VisualTilt();
    }

    void FixedUpdate() {
        FloatForce();
        jumped = false;
        OrientationForce();
        MoveForce();
        // DashForce();
    }

    void VisualTilt() {
        wizardModel.transform.rotation = Quaternion.AngleAxis(new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude*4.0f, transform.right)*transform.rotation;
        // if (!Input.GetKey(KeyCode.LeftShift))
            // wizardHat.transform.rotation = Quaternion.AngleAxis(Mathf.Clamp(new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude*-5.0f, -20.0f, 20.0f), transform.right)*wizardModel.transform.rotation;
    }

    Vector3 dashVector = Vector3.zero;
    float dashTimer = 0.0f;
    float dashCooldownTimer = 0.0f;

    void Dash() {

    }

    void Attack() {
        if (Input.GetMouseButtonDown(0)) {
            switch (spell) {
                case SelectedSpell.Melee: {
                    animator.SetTrigger("Melee");
                    swipe.SetActive(false);
                    swipe.SetActive(true);
                    meleeScript.Attack();
                    break;
                }

                case SelectedSpell.Three: {
                    animator.SetTrigger("Cast1");
                    Spell3();
                    break;
                }

                case SelectedSpell.Two: {
                    animator.SetTrigger("Cast2");
                    Spell2();
                    break;
                }

                case SelectedSpell.One: {
                    animator.SetTrigger("Cast2");
                    Spell1();
                    break;
                }
            }
        }
        if (Input.GetMouseButtonDown(2)) {
            animator.SetTrigger("Melee");
            swipe.SetActive(false);
            swipe.SetActive(true);
            meleeScript.Attack();
        }
    }

    [Header ("Spell 3")]
    [SerializeField] float spell3Cooldown;
    [SerializeField] float spell3Timer = 0.0f;
    [SerializeField] float spell3InitialDelay = 0.5f;
    [SerializeField] float spell3SecondDelay = 0.2f;

    void Spell3() {
        if (spell3Timer <= 0.0f) {
                spell3Timer = spell3Cooldown;
                StartCoroutine(Spell3Routine());
        }
        else {
        }
    }

    public void hurt(int damage) {
        health -= damage;
        an.SetTrigger("Damage");
        cameraController.StartShake(0.2f, 0.03f);
    }

    IEnumerator Spell3Routine() {
        yield return new WaitForSeconds(spell3InitialDelay);
        Instantiate(spell3, gameObject.transform.position, gameObject.transform.rotation);
        stamina -= 15;
        yield return new WaitForSeconds(spell3SecondDelay);
        Instantiate(spell3, gameObject.transform.position, gameObject.transform.rotation);
        stamina -= 15;
    }

    [Header("Spell 2")]
    [SerializeField] float spell2Cooldown;
    public float spell2InitialDelay = 0.2f;
    [SerializeField] float spell2Timer = 0.0f;

    void Spell2()
    {
        if (spell2Timer <= 0.0f)
        {
            StartCoroutine(Spell2Routine());
        } else
        {
            spell2Timer -= Time.deltaTime;
        }
    }

    IEnumerator Spell2Routine() {
        yield return new WaitForSeconds(spell2InitialDelay);
        GameObject spell = Instantiate(spell2, gameObject.transform.position, gameObject.transform.rotation);
        spell.GetComponent<Spell_2>().wizard = this;
        stamina -= 30;
    }

    [Header("Spell 1")]
    [SerializeField] public float spell1Cooldown;
    public float spell1InitialDelay = 0.45f;
    [SerializeField] public float spell1Timer = 0.0f;

    void Spell1()
    {
        if (spell1Timer <= 0.0f)
        {
            StartCoroutine(Spell1Routine());
                spell1Timer = spell1Cooldown;
        } else
        {
            spell1Timer -= Time.deltaTime;
        }
    }

    IEnumerator Spell1Routine() {
        yield return new WaitForSeconds(spell1InitialDelay);
        GameObject spell = Instantiate(spell1, mousePos+ new Vector3(0, 0.4f, 0), gameObject.transform.rotation);
        spell.GetComponent<Spell_1>().wizard = this;
        stamina -= 40;
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
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0,45,0));
        movementInput = matrix.MultiplyPoint3x4(movementInput);

        aiming = Input.GetMouseButton(1);
        attacking = Input.GetMouseButton(0);

        // Mouse input
        RaycastHit hit;
        Physics.Raycast(RayFromCursor(), out hit, Mathf.Infinity, 1 << 9);
        mousePos = hit.point;
        Vector3 lookDir = (mousePos - transform.position).normalized;
        lookDir.y = 0;
        
        if (aiming)
            targetOrientation = Quaternion.LookRotation(lookDir, Vector3.up);
        else if (movementInput.magnitude > 0.1f)
            targetOrientation = Quaternion.LookRotation(movementInput, Vector3.up);
    }

    bool running = false;

    void MoveForce() {
        float maxSpeed = 0.0f;
        float acceleration = 0.0f;
        float maxAcceleration = 0.0f;
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 10.0f && !running) {
            running = true;
            stamina -= Time.deltaTime * 30.0f;
            maxSpeed = runMaxSpeed;
            acceleration = runAcceleration;
            maxAcceleration = runMaxAcceleration;
        } 
        else if (Input.GetKey(KeyCode.LeftShift) && stamina > 1.0f && running) {
            stamina -= Time.deltaTime * 30.0f;
            maxSpeed = runMaxSpeed;
            acceleration = runAcceleration;
            maxAcceleration = runMaxAcceleration;
        } 
        else {
            maxSpeed = walkMaxSpeed;
            acceleration = walkAcceleration;
            maxAcceleration = walkMaxAcceleration;
            stamina += Time.deltaTime*20.0f;
            running = false;
        }
        if (stamina > maxStamina)
            stamina = maxStamina;

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
        if (Physics.Raycast(ray, out hit, rayLength, ~LayerMask.GetMask("Ignore Raycast", "Player"))) {


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
            if (rb.linearVelocity.y <= 0 && !jumped) {
                an.SetTrigger("Land");
                // Debug.Log("Set land");
            }
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
        // if (Input.GetKey(KeyCode.LeftShift)) {
        //     an.SetFloat("Velocity", movementInput.magnitude);
        // }
        // else {
        //     an.SetFloat("Velocity", movementInput.magnitude*0.5f);
        // }
        Vector3 horizontalVelocity = rb.linearVelocity;
        horizontalVelocity.y = 0;
        an.SetFloat("Velocity", horizontalVelocity.magnitude*(1.0f/4.0f));
        // Debug.Log(rb.linearVelocity.magnitude*(1.0f/4.0f));
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (other.gameObject.tag == "Camera Trigger")
        // {
        //     cam_Pivot.GetComponent<CameraController>().target = other.gameObject;
        // }
    }
}
