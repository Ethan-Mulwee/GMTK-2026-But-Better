using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Melee : MonoBehaviour
{
    GameObject activeObject;

    [SerializeField] WizardController wizard;
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
        activeObject = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        activeObject = null;
    }

    void OnTriggerStay(Collider other)
    {
        activeObject = other.gameObject;
    }

    public void Attack() {
        if (activeObject != null) {
            // IHitable hitable = activeObject.gameObject.GetComponent<IHitable>();
            // hitable.Hit();
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine() {
        yield return new WaitForSeconds(0.15f);
            IHitable hitable = activeObject.gameObject.GetComponent<IHitable>();
            hitable.Hit();
    }
}
