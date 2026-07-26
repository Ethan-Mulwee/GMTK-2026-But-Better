using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public WizardController wizard;
    public Image threeIcon;
    public Image twoIcon;
    public Image oneIcon;
    public RectTransform Health;
    public RectTransform Stamina;
    public RectTransform oneCooldown;
    public RectTransform twoCooldown;
    public RectTransform threeCooldown;
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
        if (wizard.spell2Enabled) {
            twoIcon.gameObject.SetActive(true);
        }
        if (wizard.spell1Enabled) {
            oneIcon.gameObject.SetActive(true);
        }
        threeIcon.color = new Color(27.0f/255.0f, 39.0f/255.0f, 54.0f/255.0f);
        twoIcon.color = new Color(27.0f/255.0f, 39.0f/255.0f, 54.0f/255.0f);
        oneIcon.color = new Color(27.0f/255.0f, 39.0f/255.0f, 54.0f/255.0f);
        switch (wizard.spell) {
            case SelectedSpell.Three: {
                threeIcon.color = Color.white;
                break;
            }
            case SelectedSpell.Two: {
                twoIcon.color = Color.white;
                break;
            }
            case SelectedSpell.One: {
                oneIcon.color = Color.white;
                break;
            }
        }
        
        Health.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, wizard.health*(500.0f/wizard.maxHealth));
        Stamina.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, wizard.stamina*(500.0f/wizard.maxStamina));
        oneCooldown.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (wizard.spell1Timer/wizard.spell1Cooldown)*30.0f);
        twoCooldown.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (wizard.spell2Timer/wizard.spell2Cooldown)*30.0f);
        threeCooldown.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (wizard.spell3Timer/wizard.spell3Cooldown)*30.0f);
    }
}
