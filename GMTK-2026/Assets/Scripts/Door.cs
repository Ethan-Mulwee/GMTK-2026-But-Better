using UnityEngine;

public class Door : MonoBehaviour
{
    public int buttonsPressed = 0;
    [SerializeField] int buttonsNeeded = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openDoor()
    {
        buttonsPressed++;

        if (buttonsPressed >= buttonsNeeded)
        {
            Destroy(gameObject);
        }
    }
}
