using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredChainsaw : MonoBehaviour
{
    public GameObject chainsaw;
    public GameObject trigger;
    public GameObject toPos;
    public float speed;
    public bool moveOnce = false;
    private Vector2 startPos;
    private bool forward;

    private void Start()
    {
        startPos = chainsaw.transform.position;
    }

    private void Update()
    {
        if(trigger.GetComponent<ChainsawTrigger>().isTriggered)
        {
            chainsaw.transform.position = Vector2.MoveTowards(chainsaw.transform.position, forward || moveOnce ? toPos.transform.position : startPos, speed*Time.deltaTime);
            if (chainsaw.transform.position == toPos.transform.position) forward = false;
            else if ((Vector2)chainsaw.transform.position == startPos) forward = true;
        }
    }
}
