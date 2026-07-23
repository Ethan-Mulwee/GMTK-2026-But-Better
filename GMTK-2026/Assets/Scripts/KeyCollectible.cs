using UnityEngine;

public class KeyCollectible : MonoBehaviour
{
    [SerializeField] GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            player.GetComponent<WizardController>().keyCount++;
            Destroy(gameObject);
        }
    }
}
