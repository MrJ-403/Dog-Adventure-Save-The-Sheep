using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class collect : MonoBehaviour
{
    public AudioSource audioSource;
    public LogicScript logicScript;
    private bool isCollected = false;

    public void Start()
    {
        logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") || isCollected) return;
        isCollected = true;
        logicScript.AddScore(10);
        audioSource.Play();
        Destroy(gameObject, audioSource.clip.length);
    }

    
}
