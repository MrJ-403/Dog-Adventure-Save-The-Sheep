using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyCollect : MonoBehaviour
{
    private LogicScript logic;
    public int healUp;
    private void Start()
    {
        Destroy(gameObject,10);
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        logic.AddHealth(healUp);
        logic.ShieldUp();
        Destroy(gameObject);
    }
}
