using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class BroomController : MonoBehaviour
{
    [Header("Position Spring")]
    [SerializeField] float springRestingHeight = 0.4f;
    [SerializeField] float springStrength = 1.0f;
    [SerializeField] float springDamping = 0.1f;
    [SerializeField] float rayLength = 0.7f;
    
    [Header("Orientation Spring")]
    [SerializeField] float orientationSpringStrength = 4.0f;
    [SerializeField] float orientationSpringDamping = 0.2f;
    [SerializeField] LayerMask playerLayer;

    Quaternion targetOrientation = Quaternion.identity;


    Rigidbody rb;
    // State
    bool grounded;

    void OnEnable() {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        Movement();
    }

    void Movement() {
        // Getting up axis excluding roll while including pitch and yaw
        Matrix4x4 m = gameObject.transform.localToWorldMatrix;

        Vector3 x = m.GetColumn(0);
        Vector3 y = m.GetColumn(1);
        Vector3 z = m.GetColumn(2);

        Vector3 projectedX = Vector3.ProjectOnPlane(x, Vector3.up);
        projectedX.Normalize();

        Debug.DrawLine(transform.position, transform.position+(projectedX*4), Color.red);
        Debug.DrawLine(transform.position, transform.position+(z*4), Color.blue);
        Vector3 createdY = Vector3.Cross(z,projectedX);
        createdY.Normalize();
        if (Vector3.Dot(createdY,Vector3.up) < 0) {
            createdY *= -1;
        }
        Debug.DrawLine(transform.position, transform.position+(createdY*4), Color.green);

        Matrix4x4 correctedMatrix = new Matrix4x4();
        correctedMatrix.m00 = projectedX.x;
        correctedMatrix.m10 = projectedX.y;
        correctedMatrix.m20 = projectedX.z;

        correctedMatrix.m01 = createdY.x;
        correctedMatrix.m11 = createdY.y;
        correctedMatrix.m21 = createdY.z;

        correctedMatrix.m02 = z.x;
        correctedMatrix.m12 = z.y;
        correctedMatrix.m22 = z.z;

        correctedMatrix.m33 = 1.0f;

        targetOrientation = correctedMatrix.rotation;

        if (Input.GetKey(KeyCode.G)) {
            rb.AddTorque(createdY * Time.deltaTime * -20.0f);
        }
        if (Input.GetKey(KeyCode.J)) {
            rb.AddTorque(createdY * Time.deltaTime * 20.0f);
        }

        // roll
        if (Input.GetKey(KeyCode.U)) {
            rb.AddTorque(transform.forward * Time.deltaTime * -20.0f);
        }
        if (Input.GetKey(KeyCode.T)) {

            rb.AddTorque(transform.forward * Time.deltaTime * 20.0f);

        }

        if (Input.GetKey(KeyCode.Y)) {
            rb.AddForce(gameObject.transform.forward * Time.deltaTime * 100.0f);
        }

        if (Input.GetKey(KeyCode.H)) {
            rb.AddForce(gameObject.transform.forward * Time.deltaTime * -100.0f);
        }
    }


    void FixedUpdate() {
        FloatForce();
        OrientationForce();
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

    void FloatForce() {
        SpringAtPosition(transform.TransformPoint(new Vector3(0.0f,0.0f,0.5f)) - new Vector3(0,0.05f,0));
        SpringAtPosition(transform.TransformPoint(new Vector3(0.0f,0.0f,-0.5f)) - new Vector3(0,0.05f,0));
    }


    void SpringAtPosition(Vector3 position) {
        RaycastHit hit;

        Vector3 rayDirection = Vector3.down; /* transform.TransformDirection(new Vector3(0,-1,0)); */
        Ray ray = new Ray(position, rayDirection);
        Debug.DrawRay(ray.origin, ray.direction*rayLength);
        
        if (Physics.Raycast(ray, out hit, rayLength, ~playerLayer)) {
            // TODO: remove this really shitty workaround to ignore self intersection of raycasts
            if (hit.collider == GetComponent<Collider>()) {
                Debug.Log("self collision");
                return;
            }
            Vector3 springDirection = Vector3.up; /* transform.TransformDirection(new Vector3(0,1,0)); */

            Vector3 velocity = rb.GetPointVelocity(position);

            float offset = springRestingHeight - hit.distance;

            float velocityAlongSpring = Vector3.Dot(springDirection, velocity);

            float force = (offset*springStrength) - (velocityAlongSpring*springDamping);

            rb.AddForceAtPosition(springDirection*force, position);
        }
    }
}
