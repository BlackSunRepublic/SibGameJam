using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    public bool isActive;
    [HideInInspector] public bool isActiveAbility;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float toGroundDistance;
    [SerializeField] private bool itsWhite;
    private Rigidbody2D rigidbody;
    private bool isJumpin = false;
    private Vector2 jumpVector;
    private Control control;
    private float deffaultSpeed;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        control = GetComponent<Control>();
        jumpVector = new Vector2(0, jumpPower);
        Debug.DrawRay(transform.position, Vector2.down * 5, Color.red);
        deffaultSpeed = playerSpeed;
    }
    public void Move(float direction)
    {
        rigidbody.velocity = new Vector2(direction * playerSpeed, rigidbody.velocity.y);
    }
    public void Jump()
    {
        if (!isJumpin)
        {
            isJumpin = true;
            rigidbody.AddForce(jumpVector, ForceMode2D.Impulse);
        }
    }
    private void Update()
    {        
        RaycastHit2D hit  = Physics2D.Raycast(rigidbody.position, Vector2.down, toGroundDistance,LayerMask.GetMask("Ground"));
        Debug.DrawRay(rigidbody.position, Vector2.down*toGroundDistance,Color.red);
        if (hit.collider)
        {
            isJumpin = false;
        }
        else
        {
            isJumpin = true;
        }
    }
    public void Interaction()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Interaction")
        {
            control.isInteract = true;
            collision.GetComponent<InteractebleObject>().Contact(true);
        }
        if (collision.transform.tag == "Movment")
        {
            playerSpeed = deffaultSpeed * 0.5f;
            collision.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Interaction")
        {
            control.isInteract = false;
            collision.GetComponent<InteractebleObject>().Contact(false);
        }
        if (collision.transform.tag == "Movment")
        {
            playerSpeed = deffaultSpeed;
            collision.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }
    public void Ability()
    {
        if (itsWhite)
        {

        }
    }
}
