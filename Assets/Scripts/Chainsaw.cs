using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chainsaw : MonoBehaviour
{
    private LogicScript logic;
    public float knockBack;
    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0, 0, 5);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player")){
            logic.KnockBack(collision,knockBack);
            logic.RemoveHealth(10);
        }
    }

    //Declerations specific to OnCollisionStay2D
    private float stayTimer = 0;
    private float stayTimerMax = 0.5f;
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(stayTimer >= stayTimerMax){
            OnCollisionEnter2D(collision);
        }
        else
            stayTimer += Time.deltaTime;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        stayTimer = 0;
    }
}
