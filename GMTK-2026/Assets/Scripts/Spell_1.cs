using UnityEngine;

public class Spell_1 : MonoBehaviour
{
    Spell_Util util = new Spell_Util();

    [SerializeField] float destroyTimer = 2.5f;
    [SerializeField] float damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyTimer > 0.0f)
        {
            destroyTimer -= Time.deltaTime;
        } else
        {
            Destroy(gameObject);
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
    }
}
