using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public WizardController wizard;
    public Image threeIcon;
    public Image twoIcon;
    public Image oneIcon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        threeIcon.gameObject.SetActive(false);
        twoIcon.gameObject.SetActive(false);
        oneIcon.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (wizard.spell3Enabled) {
            threeIcon.gameObject.SetActive(true);
        }
        threeIcon.color = new Color(27.0f/255.0f, 39.0f/255.0f, 54.0f/255.0f);
        switch (wizard.spell) {
            case SelectedSpell.Three: {
                threeIcon.color = Color.white;
                break;
            }
        }
    }
}
