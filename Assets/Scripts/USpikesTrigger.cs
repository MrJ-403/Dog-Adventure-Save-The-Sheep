using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class USpikesTrigger : MonoBehaviour
{
    public bool trigger = true;
    public LogicScript logic;
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(trigger && collision.CompareTag("Player"))
            logic.RemoveHealth(100);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTriggerEnter2D(collision);
    }
}
