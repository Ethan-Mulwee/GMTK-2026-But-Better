using Unity.VisualScripting;
using UnityEngine;

public class Collect3 : MonoBehaviour
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
        wizard.spell3Enabled = true;
        Destroy(gameObject);
    }
}
