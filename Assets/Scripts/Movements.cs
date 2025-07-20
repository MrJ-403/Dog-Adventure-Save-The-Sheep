using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movements : MonoBehaviour
{
    public InputActionReference left;
    public InputActionReference right;
    public InputActionReference space;
    public InputActionReference shift;

    //Cheat Stuff
    public InputActionReference ctrl;
    public InputActionReference f1;
    public GameObject cheatOnTextPrefab;
    private GameObject cheatOnText;
    private bool isMouseHeld = false;
    public bool cheatedOnce = false;

    private bool goLeft = false;
    private bool goRight = false;

    public float walkSpeed = 6;
    public float jumpStrength = 40;
    public float runSpeed = 10;
    public float stopSpeed = 0.5f;
    public bool run = true;
    public bool touchingGround = false;
    public bool touchingRight = false;
    public bool touchingLeft = false;
    public bool isKnocked = false;
    public bool isWonOrLost = false;
    public bool onPlatform = false;
    public LayerMask ground;
    public Transform groundCheck;
    public Rigidbody2D myRB;
    public Animator animator;

    private Vector2 platformSpeed;

    private void OnEnable()
    {
        right.action.started += Right;
        right.action.canceled += RightUp;
        left.action.started += Left;
        left.action.canceled += LeftUp;
        space.action.started += SpaceDown;
        space.action.canceled += SpaceUp;
        shift.action.started += Shift;
        ctrl.action.started += Ctrl;
        ctrl.action.canceled += CtrlUp;
        f1.action.started += F1;
        //leftMouse.action.started += MouseTeleport;
        cheatOnText = Instantiate(cheatOnTextPrefab, GameObject.Find("Main UI").transform);
    }
    private void OnDisable()
    {
        right.action.started -= Right;
        left.action.started -= Left;
        space.action.started -= SpaceDown;
        space.action.performed -= SpaceUp;
        shift.action.started -= Shift;
        ctrl.action.started-= Ctrl;
        ctrl.action.canceled -= CtrlUp;
        f1.action.started -= F1;
        //leftMouse.action.started -= MouseTeleport;
    }

    // Update is called once per frame
    void Update()
    {
        if (!goLeft && !goRight && !isKnocked) myRB.velocity = new(0, myRB.velocity.y);
        touchingGround = Physics2D.OverlapCircle(groundCheck.position, 0.4f, ground);
        if (touchingGround) isKnocked = false;

        if (goRight && !isKnocked && !isWonOrLost)// (Input.GetKey(KeyCode.D) && !isKnocked && !isWonOrLost)
        {
            myRB.velocity = new Vector2(!run ? walkSpeed : runSpeed, myRB.velocity.y);
            GetComponent<SpriteRenderer>().flipX = false;
        }
        if (goLeft && !isKnocked && !isWonOrLost)//(Input.GetKey(KeyCode.A) && !isKnocked && !isWonOrLost)
        {
            myRB.velocity = new Vector2(-1 * (!run ? walkSpeed : runSpeed), myRB.velocity.y);
            GetComponent<SpriteRenderer>().flipX = true;
        }
        animator.SetBool("isJumping", !touchingGround);
        animator.SetBool("isWalking", !run && Math.Abs(myRB.velocity.x) > 0);
        animator.SetBool("isRunning", run && Math.Abs(myRB.velocity.x) > walkSpeed);
        animator.SetBool("isFalling", myRB.velocity.y < -1);
        if (ctrlHeld && cheatMode && Mouse.current.leftButton.isPressed && !isMouseHeld) MouseTeleport();
        if(!Mouse.current.leftButton.isPressed) isMouseHeld = false;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Platform"))
        {
            //onPlatform = true;
            transform.SetParent(collider.transform);
            //onPlatform = true;
            //myRB.velocity -= platformSpeed;
            //platformSpeed = collider.GetComponentInParent<Platform>().speed *2;// * Time.deltaTime;

            //Debug.Log("got a velocity of: " + platformSpeed);
            //myRB.velocity += platformSpeed; 
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Platform"))
        {
            //onPlatform = false;
            transform.SetParent(null);
            //onPlatform = false;
            //myRB.velocity -= platformSpeed;
            //platformSpeed = Vector2.zero;
            //myRB.gravityScale = initialGravityScale;
        }
    }



    //Movments:
    public void MoveLeft()
    {
        animator.SetBool(run ? "isRunning" : "isWalking", true);
        goLeft = !goLeft;
    }
    public void MoveRight()
    {
        animator.SetBool(run ? "isRunning" : "isWalking", true);
        goRight = !goRight;
    }
    public void MJump()
    {
        if (!touchingGround || isWonOrLost) return;
        myRB.velocity = new Vector2(myRB.velocity.x, jumpStrength);
    }
    public void MJumpUp()
    { 
        if (myRB == null || myRB.velocity.y <= 0 || isKnocked) return;
        myRB.velocity = new Vector2(myRB.velocity.x, 0);
    }
    public void MSprint()
    {
        run = !run;
    }

    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
        isKnocked = false;
    }

    private void Left(InputAction.CallbackContext obj)
    {
        //if (isKnocked || isWonOrLost) return;
        goLeft = true;
    }
    private void Right(InputAction.CallbackContext obj)
    {
        //if (isKnocked || isWonOrLost) return;
        goRight = true;
    }
    private void LeftUp(InputAction.CallbackContext obj)
    {
        //if (isKnocked || isWonOrLost) return;
        //if(!isKnocked) myRB.velocity = new Vector2(0,myRB.velocity.y);
        goLeft = false;
    }
    private void RightUp(InputAction.CallbackContext obj)
    {
        //if(!isKnocked) myRB.velocity = new Vector2(0,myRB.velocity.y);
        goRight = false;
    }
    private void SpaceDown(InputAction.CallbackContext obj)
    {
        if (!touchingGround || isWonOrLost) return;
        myRB.velocity = new Vector2(myRB.velocity.x, jumpStrength);
    }
    private void SpaceUp(InputAction.CallbackContext obj)
    {
        if (myRB == null || myRB.velocity.y <= 0 || isKnocked) return;
        myRB.velocity = new Vector2(myRB.velocity.x, 0);
    }
    private void Shift(InputAction.CallbackContext obj)
    {
        run = !run;
    }

    public bool ctrlHeld = false;
    public bool cheatMode = false;
    private void Ctrl(InputAction.CallbackContext obj)
    {
        if (cheatMode)
        {
            GetComponent<TrailRenderer>().emitting = true;
            ctrlHeld = true;
            Time.timeScale = 0f;
            cheatOnText.SetActive(true);
        }
    }

    private void CtrlUp(InputAction.CallbackContext obj)
    {
        if (cheatMode)
        { 
            GetComponent<TrailRenderer>().emitting = false;
            ctrlHeld = false;
            Time.timeScale = 1f;
            cheatOnText.SetActive(false);
        }
    }

    private void F1(InputAction.CallbackContext obj)
    {
        cheatMode = !cheatMode;
    }

    private void MouseTeleport()
    {
        isMouseHeld= true;
        Vector3 pos = Camera.main.ScreenToWorldPoint(Pointer.current.position.value);
        Debug.Log(pos);
        transform.position = new(pos.x, pos.y, 0);
        cheatedOnce = true;
    }
}
