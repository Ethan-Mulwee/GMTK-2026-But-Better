using UnityEngine;

public class EnemyHurtbox : MonoBehaviour
{
    [SerializeField] float attackCooldown;
    float cooldownTimer = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            dealDamage(other.gameObject, gameObject.transform.parent.gameObject);
        }
    }

    void dealDamage(GameObject player, GameObject enemy)
    {
        if (cooldownTimer <= 0)
        {
            player.GetComponent<WizardController>().health -= enemy.GetComponent<EnemyScript>().damage;
            cooldownTimer = attackCooldown;

            // Play animation?
        }
    }
}
