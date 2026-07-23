using UnityEngine;


public class DoorTrigger : MonoBehaviour
{
    [SerializeField] Animator an;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("Player entered the door trigger");
        an.Play("Door-Open");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("Player left the door trigger");
        an.Play("Door-Close");
    }
}
