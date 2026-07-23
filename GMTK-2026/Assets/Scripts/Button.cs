using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] GameObject Door;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pressButton()
    {
        gameObject.GetComponent<Collider>().enabled = false;

        Door.GetComponent<Door>().openDoor();
    }
}
