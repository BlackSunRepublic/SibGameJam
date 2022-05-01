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
    [Header("Только для тени")]
    [SerializeField] private GameObject realPlayer;
    [SerializeField] private float abilityDistance;
    private Rigidbody2D rigidbody;
    private bool isJumpin = false;
    private Vector2 jumpVector;
    private Control control;
    private float deffaultSpeed;
    private SpriteRenderer spriteRenderer;
    private float startTimer;
    private List<InteractebleObject> flammableObjects = new List<InteractebleObject>();
    private InteractebleObject activeFlammableObject;
    private InteractebleObject activeInterObject;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        control = GetComponent<Control>();
        jumpVector = new Vector2(0, jumpPower);
        Debug.DrawRay(transform.position, Vector2.down * 5, Color.red);
        deffaultSpeed = playerSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>();
        startTimer = Time.time;
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Flammable");
        if (itsWhite)
        {
            foreach (GameObject gameObj in objects)
            {
                flammableObjects.Add(gameObj.GetComponent<InteractebleObject>());
            }
        }
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
        if (isActiveAbility && !itsWhite)
        {
            if((transform.position.x - realPlayer.transform.position.x) > abilityDistance)
            {
                transform.position = new Vector2(realPlayer.transform.position.x + abilityDistance, transform.position.y);
            }
            if ((transform.position.x - realPlayer.transform.position.x) < - abilityDistance)
            {
                transform.position = new Vector2(realPlayer.transform.position.x - abilityDistance, transform.position.y);
            }
        }
        if (Time.time - startTimer > 1.0f && itsWhite)
        {
            foreach(InteractebleObject iObj in flammableObjects)
            {
                if (Mathf.Abs(Vector2.Distance(transform.position, iObj.transform.position)) <= abilityDistance)
                {
                    iObj.Contact(true);
                    activeFlammableObject = iObj;
                    break;
                }
                else 
                {
                    iObj.Contact(false);
                }
                activeFlammableObject = null;
            }
            startTimer = Time.time;
        }
    }
    public void Interaction()
    {
        if (activeInterObject)
        {
            activeInterObject.StartEvent();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Interaction")
        {
            control.isInteract = true;
            activeInterObject = collision.GetComponent<InteractebleObject>();
            activeInterObject.Contact(true);
        }
        if (collision.transform.tag == "Movment")
        {
            playerSpeed = deffaultSpeed * 0.5f;
            collision.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
        if (collision.tag == "Death")
        {
            UI.UIData.losePanel.SetActive(true);
            if (itsWhite)
            {
                UI.UIData.loseText.text = "Дейзи погибла...";
            }
            else
            {
                UI.UIData.loseText.text = "Тень погиб...";
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Interaction")
        {
            control.isInteract = false;
            collision.GetComponent<InteractebleObject>().Contact(false);
            activeInterObject = null;
        }
        if (collision.transform.tag == "Movment")
        {
            playerSpeed = deffaultSpeed;
            collision.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }
    public void Ability()
    {
        if (!itsWhite&&!isJumpin)
        {
            isActiveAbility = !isActiveAbility;
            Color color = spriteRenderer.color;
            if (isActiveAbility)
            {
                color.a = 0.5f;
                gameObject.layer = LayerMask.NameToLayer("Shadow");
                realPlayer.transform.parent = transform.parent;
                realPlayer.SetActive(true);
            }
            else
            {
                color.a = 1;
                gameObject.layer = LayerMask.NameToLayer("Default");
                transform.position = realPlayer.transform.position;
                realPlayer.transform.parent = transform;
                realPlayer.SetActive(false);
            }
            spriteRenderer.color = color;
        }
        else
        {
            if (activeFlammableObject)
            {
                flammableObjects.Remove(activeFlammableObject);
                Destroy(activeFlammableObject.gameObject, 2);
            }
        }
    }
}
