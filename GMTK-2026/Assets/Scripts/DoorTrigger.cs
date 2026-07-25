using UnityEngine;


public class DoorTrigger : MonoBehaviour
{
    [SerializeField] Animator an;
    [SerializeField] bool locked = false;

    public void unlock() {
        locked = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (locked) return;
        an.Play("Door-Open");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (locked) return;
        an.Play("Door-Close");
    }
}
