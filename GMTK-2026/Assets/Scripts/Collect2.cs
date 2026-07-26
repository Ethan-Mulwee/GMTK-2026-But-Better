using Unity.VisualScripting;
using UnityEngine;

public class Collect2 : MonoBehaviour
{
    public DoorTrigger doorTrigger;

    public WizardController wizard;
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
        wizard.spell2Enabled = true;
        doorTrigger.unlock();
        Destroy(gameObject);
    }
}
