using System.Collections;
using TMPro;
using UnityEngine;

public class Enemy1 : MonoBehaviour, IHitable
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
    float attackTimer = 0.0f;
    float attackCooldown = 2.0f;
    public GameObject strike;



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
        
        if (attackTimer <= 0.0f && sighted) {
            attackTimer = attackCooldown + Random.Range(0.0f, 0.5f);
            Vector2 random = Random.insideUnitCircle;
            Instantiate(strike, target.transform.position + new Vector3(random.x, 0, random.y), Quaternion.identity);
        }

        if (Vector3.Distance(target.transform.position, transform.position) < 3.0f)
        {
            Vector3 vec = target.transform.position - gameObject.transform.position;
            rb.AddForce((target.transform.position - gameObject.transform.position) * -speed*0.4f);
            // textMesh.transform.Rotate(0.0f, 0.0f, 1.0f);
        }

        attackTimer -= Time.deltaTime;
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
