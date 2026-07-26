using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy2 : MonoBehaviour, IHitable
{

    Rigidbody rb;
    [SerializeField] TextMeshPro textMesh;
    public WizardController target;

    [SerializeField] float knockbackForce = 10.0f;
    [SerializeField] float activateDistance = 5.0f;
    [SerializeField] float speed = 0.5f;
    public GameObject explosionObject;
    public ParticleSystem particles;

    bool following = true;
    bool sighted = false;
    public bool activated = false;
    bool exploding = false;
    bool exploded = false;
    float explosionTimer = 1.0f;
    float explosionDistance = 1.0f;
    int explosionCastCount = 50;
    float explosionSphereRadius = 0.2f;
    float explosionRadius = 1.0f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (exploded) return;

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
                    if (wizard != null) {
                        target.hurt(33);
                        break;
                    }
                }
            }

                RaycastHit[] raycastHits2= new RaycastHit[explosionCastCount];
            for (int i = 0; i < explosionCastCount; i++) {
                float angle = (i/((float)explosionCastCount))*Mathf.PI*2.0f;
                Ray ray = new Ray(transform.position, new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)));
                Physics.Raycast(ray, out raycastHits2[i], explosionRadius);
            }

            foreach (RaycastHit hit in raycastHits2) {
                if (hit.rigidbody != null) {
                    hit.rigidbody.AddForce((hit.rigidbody.position - transform.position).normalized * 0.3f, ForceMode.Impulse);
                }
            }
            target.cameraController.StartShake(0.3f, 0.04f);
            // Destroy(gameObject);
            textMesh.gameObject.SetActive(false);
            particles.gameObject.SetActive(true);
            particles.Play();
            exploded = true;
            StartCoroutine(killTimer());
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
        if (collision.collider.gameObject.tag == "Spell") {
            Destroy(gameObject);
        }
        if (!following) {
            Destroy(gameObject);
        }
        // Destroy(gameObject);
    }

    // incase it gets out of bounds somehow
    IEnumerator killTimer() {
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }
}
