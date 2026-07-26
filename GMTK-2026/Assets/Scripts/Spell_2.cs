using UnityEngine;

public class Spell_2 : MonoBehaviour
{
    Spell_Util util = new Spell_Util();

    [SerializeField] float speed;
    [SerializeField] float damage;
    bool firing = true;
    [SerializeField] float explosionTime;
    [SerializeField] private int explosionCastCount = 50;
    [SerializeField] private float explosionRadius = 4.0f;
    [SerializeField] private float explosionSphereRadius = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (firing)
        {
            transform.position += gameObject.transform.forward * speed * Time.deltaTime;
        } else
        {
            explosionTime -= Time.deltaTime;

            if (explosionTime < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        try
        {
            collision.gameObject.GetComponent<EnemyScript>().health -= damage;
        }
        catch { }

        util.checkCollision(collision);

        gameObject.GetComponent<Collider>().enabled = false;

        RaycastHit[] raycastHits = new RaycastHit[explosionCastCount];
        for (int i = 0; i < explosionCastCount; i++) {
            float angle = (i/((float)explosionCastCount))*Mathf.PI*2.0f;
            Ray ray = new Ray(transform.position, new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)));
            Physics.SphereCast(ray, explosionSphereRadius, out raycastHits[i], explosionRadius, LayerMask.GetMask("Enemy"));
        }
        foreach (RaycastHit hit in raycastHits) {
            if (hit.collider != null) {
                hit.collider.gameObject.GetComponent<IHitable>().Hit(transform.position);
            }
        }

        //Create fireball explosion
        firing = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }
}
