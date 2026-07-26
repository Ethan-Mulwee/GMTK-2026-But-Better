using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy2 : MonoBehaviour, IHitable
{

    Rigidbody rb;
    [SerializeField] TextMeshPro textMesh;
    public WizardController target;

    [SerializeField] float knockbackForce = 10.0f;
    [SerializeField] float activateDistance = 5.0f;
    [SerializeField] float speed = 0.5f;
    public GameObject explosionObject;

    bool following = true;
    bool sighted = false;
    public bool activated = false;
    bool exploding = false;
    float explosionTimer = 1.0f;
    float explosionDistance = 1.0f;
    int explosionCastCount = 50;
    float explosionSphereRadius = 0.5f;
    float explosionRadius = 3.0f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(target.transform.position, transform.position) < activateDistance && !sighted && activated) {
            sighted = true;
            textMesh.color = Color.red;
        }

        if (following && sighted && !exploding)
        {
            Vector3 vec = target.transform.position - gameObject.transform.position;
            rb.AddForce((target.transform.position - gameObject.transform.position) * speed);
            textMesh.transform.Rotate(0.0f, 0.0f, 1.0f);
        }

        if (Vector3.Distance(target.transform.position, transform.position) < explosionDistance) {
            Debug.Log("Explosion triggered");
            exploding = true;
            textMesh.gameObject.transform.localScale = new Vector3(1.2f,1.2f,1.2f);
        }
        if (exploding) {
            explosionTimer -= Time.deltaTime;
        }

        if (explosionTimer <= 0) {
            RaycastHit[] raycastHits = new RaycastHit[explosionCastCount];
            for (int i = 0; i < explosionCastCount; i++) {
                float angle = (i/((float)explosionCastCount))*Mathf.PI*2.0f;
                Ray ray = new Ray(transform.position, new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)));
                Physics.SphereCast(ray, explosionSphereRadius, out raycastHits[i], explosionRadius, LayerMask.GetMask("Player"));
            }
            foreach (RaycastHit hit in raycastHits) {
                if (hit.collider != null) {
                    WizardController wizard = hit.collider.gameObject.GetComponent<WizardController>();
                    target.hurt(50);
                    break;
                }
            }
            target.cameraController.StartShake(0.3f, 0.04f);
            Destroy(gameObject);
        }
    }

    public void Hit(Vector3 hitPos)
    {
        following = false;

        Vector3 awayFromPlayer = (gameObject.transform.position - hitPos).normalized;
        rb.AddForce(awayFromPlayer * knockbackForce, ForceMode.Impulse);
        textMesh.color = Color.green;
        StartCoroutine(killTimer());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject == target.gameObject && following) {
            // Debug.Log("hit player");
            // target.hurt(20);
            // Destroy(gameObject);
        }
        if (!following) {
            Destroy(gameObject);
        }
        // Destroy(gameObject);
    }

    // incase it gets out of bounds somehow
    IEnumerator killTimer() {
        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
    }
}
