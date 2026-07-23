using UnityEngine;

public class Spell_Util
{
    public void checkCollision(Collision collision)
    {
        if (collision.gameObject.tag == "Button")
        {
            collision.gameObject.GetComponent<Button>().pressButton();
        }
    }
}
