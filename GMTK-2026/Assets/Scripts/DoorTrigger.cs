using UnityEngine;


public class DoorTrigger : MonoBehaviour
{
    [SerializeField] Animator an;
    public bool isOpen = false;
    
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (!other.CompareTag("Player")) return;
    // 
    //     an.Play("Door-Open");
    //     isOpen = true;
    // }

    public void openDoor()
    {
        if (!isOpen)
        {
            an.Play("Door-Open");
            isOpen = true;
        }
    }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (!other.CompareTag("Player")) return;
    // 
    //     an.Play("Door-Close");
    //     isOpen = false;
    // }

    public void closeDoor()
    {
        if (isOpen)
        {
            an.Play("Door-Close");
            isOpen = false;
        }
    }
}
