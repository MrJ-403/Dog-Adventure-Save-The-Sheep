using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stinger : MonoBehaviour
{
    private LogicScript logic;
    public LayerMask excludeLayers;
    public float knockBack;
    // Start is called before the first frame update
    private void Start()
    {
        Destroy(gameObject, 3);
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        GetComponent<PolygonCollider2D>().excludeLayers = excludeLayers;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<AudioSource>().Play();
        if (collision.collider.CompareTag("Player"))
        {
            logic.KnockBack(collision, knockBack);
            logic.RemoveHealth(20);
        }
        Destroy(gameObject);
    }
}
