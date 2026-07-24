using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy3 : MonoBehaviour, IHitable
{

    Rigidbody rb;
    [SerializeField] TextMeshPro textMesh;
    public GameObject target;

    [SerializeField] float knockbackForce = 10.0f;
    [SerializeField] float activateDistance = 5.0f;

    bool following = true;
    bool activated = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(target.transform.position, transform.position) < activateDistance && !activated) {
            activated = true;
            textMesh.color = Color.red;
        }

        if (following && activated)
        {
            Vector3 vec = target.transform.position - gameObject.transform.position;
            rb.AddForce(target.transform.position - gameObject.transform.position);
            textMesh.transform.Rotate(0.0f, 0.0f, 1.0f);
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
