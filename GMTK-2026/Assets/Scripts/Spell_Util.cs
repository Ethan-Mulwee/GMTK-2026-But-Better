using UnityEngine;

public class Spell_Util
{
    public void checkCollision(GameObject collision)
    {
        if (collision.tag == "Button")
        {
            collision.GetComponent<Button>().pressButton();
        }
    }
}
