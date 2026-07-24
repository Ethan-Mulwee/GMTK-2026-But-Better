using UnityEngine;

public class Spell_2 : MonoBehaviour
{
    Spell_Util util = new Spell_Util();

    [SerializeField] float speed;
    [SerializeField] float damage;
    [SerializeField] float explosionForce;
    bool firing = true;
    [SerializeField] float explosionTime;

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

        util.checkCollision(collision.gameObject);

        gameObject.GetComponent<Collider>().enabled = false;

        //Create fireball explosion
        firing = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, 5.0f, ~(1 << 7));
        foreach (Collider c in hitColliders)
        {
            try
            {
                c.gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, gameObject.transform.position, 5.0f);

                Debug.Log(c.gameObject);
            }
            catch { }
        }
    }
}
