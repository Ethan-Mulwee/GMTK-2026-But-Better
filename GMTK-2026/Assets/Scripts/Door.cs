using UnityEngine;

public class Door : MonoBehaviour
{
    [HideInInspector] public int buttonsPressed = 0;
    [SerializeField] int buttonsNeeded = 1;
    bool interactable = false;
    [SerializeField] GameObject player;
    [SerializeField] GameObject openingDoor;

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
                replaceDoor();
            }
        } else if (player.GetComponent<WizardController>().keyCount > 0)
        {
            player.GetComponent<WizardController>().keyCount--;
            replaceDoor();
        }
    }

    void replaceDoor()
    {
        Vector3 spawnPos = gameObject.transform.position + new Vector3(0, 0.7f, 0);
        Instantiate(openingDoor, spawnPos, gameObject.transform.rotation);
        Destroy(gameObject);
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
