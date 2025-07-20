using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    private LogicScript logic;
    public int damage = 20;
    public float knockBack;
    public LayerMask excludeLayers;
    // Start is called before the first frame update
    void Start()
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
            logic.RemoveHealth(damage);
        }
        Destroy(gameObject);
    }

}
