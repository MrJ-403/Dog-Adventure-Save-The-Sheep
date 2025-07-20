using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BeeNavigation : MonoBehaviour
{
    private Transform[] flowers;
    private float timer = 0;
    public float timerLimit = 2;
    public float maxHeight = 2;
    public float speed = 4;
    //private bool flyingUp = true;
    public AIDestinationSetter destinationSetter;
    public GameObject bee;
    public AIPath path;
    // Start is called before the first frame update
    void Start()
    {
        GameObject grid = GameObject.FindWithTag("Environment");
        Transform beeTarget = grid.transform.Find("Bee_Targeted");
        if (beeTarget.childCount <= 0)
        {
            return;
        }
        flowers = new Transform[beeTarget.childCount];
        for (int i = 0; i < beeTarget.childCount; i++)
        {
            flowers[i] = beeTarget.GetChild(i).transform;
        }
        //Debug.Log("There is " + flowers.Length + " Flowers..");
        destinationSetter.target = flowers[Random.Range(0, flowers.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Movements>().transform.Find("Shield").gameObject.activeInHierarchy)
        {
            bee.GetComponent<SpriteRenderer>().flipX = path.velocity.x < 0;
            gameObject.GetComponent<AIDestinationSetter>().target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        else
        {
            if (GameObject.FindWithTag("Environment").transform.Find("Bee_Targeted").childCount <= 0) return;
            bee.GetComponent<SpriteRenderer>().flipX = path.velocity.x < 0;

            if (gameObject.GetComponent<AIDestinationSetter>().target == GameObject.FindGameObjectWithTag("Player").transform)
            {
                timer = 0;
                gameObject.GetComponent<AIDestinationSetter>().target = flowers[Random.Range(0, flowers.Length)];
            }

            else if (path.reachedEndOfPath)
            {
                if (timer >= timerLimit)
                {
                    timer = 0;
                    gameObject.GetComponent<AIDestinationSetter>().target = flowers[Random.Range(0, flowers.Length)];
                }
                timer += Time.deltaTime;
            }
        }
    }

    //void FixedUpdate()
    //{
    //    Rigidbody2D rb = bee.GetComponent<Rigidbody2D>();
        
    //    if(flyingUp) rb.velocity = Vector2.up * speed * Time.deltaTime;
    //    else rb.velocity = Vector2.down * speed * Time.deltaTime;
    //    if(bee.transform.position.y >= transform.position.y + maxHeight) flyingUp = false;        
    //    else flyingUp = true;
    //}
}
