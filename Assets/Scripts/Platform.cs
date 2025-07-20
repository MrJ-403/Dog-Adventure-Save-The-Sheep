using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject platform;
    public GameObject trigger;
    public GameObject toPos;
    public Rigidbody2D rb; //Not simulated by defualt
    public float speed;
    public bool isFallingPlatform = false; // will the platform fall when the player touches it?
    public bool isTriggered = false;
    public bool moveOnce = false;
    public bool isReversed = false;
    public bool lerp = true; // controls wether the platform uses Lerp() or MoveTowards()
    private Vector2 startPos;
    private float totalLerpTime;
    private float lerpTime = 0f;

    //Give 0.1f of a second for the player to jump off the platform
    private float jumpTimer = 0f;
    private float jumpThreshold = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        startPos = platform.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        totalLerpTime = Vector2.Distance(startPos, toPos.transform.position) / speed;
        lerpTime += Time.deltaTime;
        if (moveOnce && isReversed) return;
        if (!isTriggered && trigger.GetComponent<BoxCollider2D>().IsTouching(GameObject.FindGameObjectWithTag("Player").GetComponent<CircleCollider2D>()))
        {
            isTriggered = true;
        }
        if (isTriggered)
        {
            if (isFallingPlatform)
            {
                if (jumpTimer < jumpThreshold) 
                { 
                    jumpTimer += Time.deltaTime;
                    return; //This is a temporary solution as it skips all the code after it
                }
                
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.simulated = true;
                rb.gravityScale = 1f; //to make sure the gravityScale is 1f (not really needed)
                if(platform.transform.position.y <= toPos.transform.position.y)
                {
                    if (GameObject.FindGameObjectWithTag("Player").transform.parent == transform)
                        GameObject.FindGameObjectWithTag("Player").transform.SetParent(null);
                    Destroy(gameObject);
                }
            }
            else if (lerp)
                platform.transform.position = Vector2.Lerp(platform.transform.position, isReversed ? startPos : toPos.transform.position, Mathf.SmoothStep(0.01f, 1, lerpTime / totalLerpTime));
            else
                platform.transform.position = Vector2.MoveTowards(platform.transform.position, isReversed ? startPos : toPos.transform.position, speed*Time.deltaTime);

        }
        //Switching targets Algo
        if (!moveOnce && Vector2.Distance(platform.transform.position, toPos.transform.position) < 0.01)
        {
            isReversed = true;
            lerpTime = 0f;
            
        }
        if (Vector2.Distance(platform.transform.position, startPos) < 0.01)
        {
            isReversed = false;
            lerpTime = 0f;
        }
    }

}
