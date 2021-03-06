using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovment : MonoBehaviour
{
    public bool isActive;
    public GameObject virtualCamera;
    [HideInInspector] public bool isActiveAbility;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float toGroundDistance;
    [SerializeField] private bool itsWhite;
    [Header("?????? ??? ????")]
    [SerializeField] private GameObject realPlayer;
    [SerializeField] private RuntimeAnimatorController spiritAnimator;
    [SerializeField] private float abilityDistance;
    [SerializeField] private Control control;
    [SerializeField] private GameObject messager;
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
    private Animator animator;
    private RuntimeAnimatorController defaultAnimatorController;
    private Text messageText;
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
        animator = GetComponent<Animator>();
        defaultAnimatorController = animator.runtimeAnimatorController;
        messageText = messager.GetComponentInChildren<Text>();        
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
        if ((!isJumpin && rigidbody.velocity.magnitude>0.5)||(isActiveAbility && rigidbody.velocity.magnitude > 0.5))
        {
            animator.SetBool("Run",true);
        }
        else
        {
            animator.SetBool("Run", false);
        }
        if (directionX > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if(directionX<0)
        {
            spriteRenderer.flipX = true;
        }
    }
    public void Jump()
    {
        if (!isJumpin)
        {
            isJumpin = true;
            rigidbody.AddForce(jumpVector, ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
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
        //????????? ???????? ???????
        if (Time.time - startTimer > 1.0f && itsWhite)
        {
            foreach(InteractebleObject iObj in flammableObjects)
            {
                if (Mathf.Abs(Vector2.Distance(transform.position, iObj.transform.position)) <= abilityDistance)
                {
                    iObj.Contact(true);
                    activeFlammableObject = iObj;
                   // Debug.Log(iObj.name);
                    break;
                }
                else 
                {
                    iObj.Contact(false);
                }
                //activeFlammableObject = null;
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
        if (collision.tag == "Death")
        {
            UI.UIData.losePanel.SetActive(true);
            if (itsWhite)
            {
                UI.UIData.loseText.text = "????? ???????...";
            }
            else
            {
                UI.UIData.loseText.text = "???? ?????...";
            }
        }
        if(collision.tag == "Finish")
        {
            control.shadow.gameObject.SetActive(true);
            control.SwitchPerson();
            if (!control.shadow.gameObject.activeSelf || !control.dasy.gameObject.activeSelf)
            {
                UI.UIData.NextScene();
            }
            gameObject.SetActive(false);
        }
        if(collision.tag == "HintCollider")
        {
            MessageTrigger mt = collision.GetComponent<MessageTrigger>();
            if ((itsWhite && mt.itsDayze) || (!itsWhite && !mt.itsDayze))
            {
                MessageText(mt.message, mt.timeAction);
                Destroy(collision.gameObject);
            }
            
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == "Movment")
        {
            playerSpeed = deffaultSpeed * 0.5f;
            //collision.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            animator.SetBool("Push", true);
            if (!Sounds.PlaySound.push.isPlaying)
            {
                Sounds.PlaySound.push.Play();
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
            animator.SetBool("Push", false);
            playerSpeed = deffaultSpeed;
            //collision.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            Sounds.PlaySound.push.Stop();
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
                    animator.runtimeAnimatorController = spiritAnimator;
                    color.a = 0.5f;
                    gameObject.layer = LayerMask.NameToLayer("Shadow");
                    rigidbody.gravityScale = 0;
                    realPlayer.transform.parent = transform.parent;
                    realPlayer.SetActive(true);
                }
                else
                {
                    animator.runtimeAnimatorController = defaultAnimatorController;
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
                if (activeFlammableObject.flame)
                {
                    activeFlammableObject.flame.SetActive(true);
                }
                StartCoroutine(Flame());
                animator.SetTrigger("Cast");
                Sounds.PlaySound.fire.Play();
            }
        }
    }
    private IEnumerator Flame()
    {
        yield return new WaitForSeconds(2);
        activeFlammableObject.StartEvent();
        Destroy(activeFlammableObject.gameObject);
    }
    public void PlayStep()
    {
        Sounds.PlaySound.step.pitch = Random.Range(0.8f, 1.2f);
        Sounds.PlaySound.step.Play();
    }
    public void PlayJump()
    {
        Sounds.PlaySound.jump.Play();
    }
    public void MessageText(string text, int seconds)
    {
        messager.SetActive(true);
        messageText.text = text;
        StartCoroutine(SendHint(seconds));
    }
    IEnumerator SendHint(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        messageText.text = "";
        messager.SetActive(false);
    }
}
