using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyScript : MonoBehaviour
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
    [SerializeField ]GameObject target;

    Rigidbody rb;
    bool grounded = true;
    Quaternion targetOrientation = Quaternion.identity;
    Vector3 velocity;

    void OnEnable() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        FloatForce();
        OrientationForce();
        MoveForce();
    }

    void MoveForce() {
        Vector3 movement = (target.transform.position - gameObject.transform.position).normalized;
        Vector3 goalVelocity = movement * maxSpeed;
        velocity = Vector3.MoveTowards(velocity, goalVelocity, acceleration*Time.deltaTime);

        Vector3 neededAcceleration = (velocity-rb.linearVelocity)/Time.deltaTime;
        neededAcceleration = Vector3.ClampMagnitude(neededAcceleration, maxAcceleration);
        neededAcceleration.y = 0;
        rb.AddForce(neededAcceleration*rb.mass);
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
}
