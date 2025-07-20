using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beehive : MonoBehaviour
{
    private float timer = 0;
    public float dropRate;
    public GameObject honeyDrop;

    // Update is called once per frame
    void Update()
    {
        if (timer >= dropRate)
        {
            timer = 0;
            Instantiate(honeyDrop, transform.position, transform.rotation);
        }
        else
            timer += Time.deltaTime;
    }
}
