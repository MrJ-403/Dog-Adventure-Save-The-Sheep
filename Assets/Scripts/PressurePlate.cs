using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Sprite pressed;
    public Sprite released;
    public bool isPressed = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (isPressed) return;
        isPressed = true;
        GetComponent<SpriteRenderer>().sprite = pressed;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        GetComponent<SpriteRenderer>().sprite = released;
    }
}
