using UnityEngine;

public class Spell_3 : MonoBehaviour
{
    Spell_Util util = new Spell_Util();

    [SerializeField] float speed;
    [SerializeField] float damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += gameObject.transform.forward * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        try
        {
            collision.gameObject.GetComponent<EnemyScript>().health -= damage;
        } catch { }

        util.checkCollision(collision.gameObject);

        Destroy(gameObject);
    }
}
