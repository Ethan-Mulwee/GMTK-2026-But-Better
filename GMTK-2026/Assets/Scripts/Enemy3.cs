using TMPro;
using UnityEngine;

public class Enemy3 : MonoBehaviour, IHitable
{

    Rigidbody rb;
    TextMeshPro textMesh;
    public GameObject target;

    [SerializeField] float knockbackForce = 10.0f;
    [SerializeField] float activateDistance = 5.0f;

    bool following = true;
    bool activated = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        textMesh = gameObject.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(target.transform.position, transform.position) < activateDistance) {
            activated = true;
        }

        if (following && activated)
        {
            rb.AddForce(target.transform.position - gameObject.transform.position);
        }
    }

    public void Hit()
    {
        following = false;

        Vector3 awayFromPlayer = (gameObject.transform.position - target.transform.position).normalized;
        rb.AddForce(awayFromPlayer * knockbackForce, ForceMode.Impulse);
        textMesh.color = Color.green;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject == target && following) {
            Debug.Log("hit player");
            Destroy(gameObject);
        }
        if (!following) {
            Destroy(gameObject);
        }
        // Destroy(gameObject);
    }
}
