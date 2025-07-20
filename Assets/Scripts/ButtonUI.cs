using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public Sprite simple;
    public Sprite hover;
    public Sprite pressed;
    
    public void OnExit()
    {
        gameObject.GetComponent<Image>().sprite = simple;
    }
    public void OnHover()
    {
        gameObject.GetComponent<Image>().sprite = hover;
    }

    public void OnPressed()
    {
        gameObject.GetComponent<Image>().sprite = pressed;
    }
    
    public void OnUp()
    {
        gameObject.GetComponent<Image>().sprite = simple;
    }
}
