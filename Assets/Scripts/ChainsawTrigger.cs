using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainsawTrigger : MonoBehaviour
{
    public bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        isTriggered = true;
    }
}
