using UnityEngine;


public class DoorTrigger : MonoBehaviour
{
    [SerializeField] Animator an;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        an.Play("Door-Open");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        an.Play("Door-Close");
    }
}
