using UnityEngine;


public class DoorTrigger : MonoBehaviour
{
    [SerializeField] Animator an;
    [SerializeField] bool locked = false;
    public MeshFilter meshFilter;
    public Mesh unlockedMesh; 
    public Mesh lockedMesh; 

    public void unlock() {
        locked = false;
        meshFilter.mesh = unlockedMesh;
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
