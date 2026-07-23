using UnityEngine;

public class Door : MonoBehaviour
{
    public int buttonsPressed = 0;
    [SerializeField] int buttonsNeeded = 1;
    [SerializeField] bool interactable = false;
    [SerializeField] GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactable && buttonsNeeded == 0)
        {
            openDoor();
        }
    }

    public void openDoor()
    {
        if (buttonsNeeded >= 1)
        {
            buttonsPressed++;

            if (buttonsPressed >= buttonsNeeded)
            {
                Destroy(gameObject);
            }
        } else if (player.GetComponent<WizardController>().keyCount > 0)
        {
            player.GetComponent<WizardController>().keyCount--;
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            interactable = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            interactable = false;
        }
    }
}
