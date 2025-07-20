using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StoneHead : MonoBehaviour
{
    private Animator animator;
    private Transform child;
    public GameObject stone;
    public float stoneSpeed;
    public float shotTimer;
    public float shotRate;
    public float gravityScale = 1;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        child = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        shotTimer += Time.deltaTime;
        if (shotTimer >= shotRate)
        {
            animator.SetTrigger("isShooting");
            Shoot();
            shotTimer = 0;
        }
    }

    private void Shoot()
    {
        GetComponent<AudioSource>().Play();
        var stoneShot = Instantiate(stone, child.position, child.rotation);
        stoneShot.GetComponent<Rigidbody2D>().velocity = transform.right*(-1)*stoneSpeed;
        stoneShot.GetComponent<Rigidbody2D>().gravityScale = gravityScale;
    }

}
