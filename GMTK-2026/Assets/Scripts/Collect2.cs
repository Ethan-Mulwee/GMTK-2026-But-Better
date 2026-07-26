using Unity.VisualScripting;
using UnityEngine;

public class Collect2 : MonoBehaviour
{

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
        Destroy(gameObject);
    }
}
