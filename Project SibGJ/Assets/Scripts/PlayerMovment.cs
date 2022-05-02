using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    public bool isActive;
    public GameObject virtualCamera;
    [HideInInspector] public bool isActiveAbility;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float toGroundDistance;
    [SerializeField] private bool itsWhite;
    [Header("Только для тени")]
    [SerializeField] private GameObject realPlayer;
    [SerializeField] private float abilityDistance;
    [SerializeField] private Control control;
    private Rigidbody2D rigidbody;
    private bool isJumpin = false;
    private Vector2 jumpVector;    
    private float deffaultSpeed;
    private SpriteRenderer spriteRenderer;
    private float startTimer;
    private List<InteractebleObject> flammableObjects = new List<InteractebleObject>();
    private InteractebleObject activeFlammableObject;
    private InteractebleObject activeInterObject;
    private Transform defaulParent;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
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
        defaulParent = transform.parent;
    }
    public void Move(float directionX, float directionY)
    {
        directionX *= playerSpeed;
        directionY *= playerSpeed;
        if (!isActiveAbility)
        {
            directionY = rigidbody.velocity.y;
        }
        rigidbody.velocity = new Vector2(directionX, directionY);
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
            transform.parent = hit.transform.parent; 
        }
        else
        {
            isJumpin = true;
            transform.parent = defaulParent;
        }

        if (isActiveAbility && !itsWhite)
        {
            Vector2 offset = transform.position - realPlayer.transform.position;
            Vector2 rPosition = realPlayer.transform.position;
            transform.position = rPosition + Vector2.ClampMagnitude(offset, abilityDistance);
        }
        //Подсветка горящего объекта
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
        if(collision.tag == "Finish")
        {
            control.shadow.gameObject.SetActive(true);
            control.SwitchPerson();
            gameObject.SetActive(false);
        }
        if(collision.tag == "HintCollider")
        {
            if (itsWhite)
            {
                UI.UIData.MessageText("У Дейзи есть способность поджигать предметы на расстоянии, нажми на F что бы пожечь веревку удерживающую платформу ", 5);
            }
            else
            {
                UI.UIData.MessageText("Способность тени - выпускать дух, который может летать и проходить сквозь перпятствия", 4);
            }
            Destroy(collision.gameObject);
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
        if (!itsWhite)
        {
            {
                if(!isJumpin && !isActiveAbility) 
                {
                    isActiveAbility = true;
                }
                else
                {
                    isActiveAbility = false;
                }
                Color color = spriteRenderer.color;
                if (isActiveAbility)
                {
                    color.a = 0.5f;
                    gameObject.layer = LayerMask.NameToLayer("Shadow");
                    rigidbody.gravityScale = 0;
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
                    rigidbody.gravityScale = 1;
                }
                spriteRenderer.color = color;
            }
        }
        else
        {
            if (activeFlammableObject)
            {
                flammableObjects.Remove(activeFlammableObject);
                StartCoroutine(Flame());
            }
        }
    }
    private IEnumerator Flame()
    {
        yield return new WaitForSeconds(2);
        activeFlammableObject.StartEvent();
        Destroy(activeFlammableObject.gameObject);
    }
}
