using UnityEngine;

public class Explosion : MonoBehaviour
{
    Spell_Util util = new Spell_Util();
    
    [SerializeField] float damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        try
        {
            other.gameObject.GetComponent<EnemyScript>().health -= damage;
        } catch { }

        util.checkCollision(other.gameObject);

        // float mass = gameObject.GetComponent<Rigidbody>().mass;
        // Vector3 direction = gameObject.transform.position - collision.gameObject.transform.position;
        // collision.gameObject.GetComponent<Rigidbody>().linearVelocity = mass * direction * 100;

        Debug.Log("Enter: " + other.gameObject);
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Stay: " + collision.gameObject);
    }
}
