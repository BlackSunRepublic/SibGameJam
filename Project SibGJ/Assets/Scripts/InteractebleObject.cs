using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractebleObject : MonoBehaviour
{
    [SerializeField] Material outlineMaterial;
    [SerializeField] Material defaultMaterial;
    [SerializeField] GameObject hint;
    [SerializeField] Animator eventAnimation;
    [SerializeField] private Animator animator;
    [SerializeField] private bool itsButton;
    public GameObject flame;
    private SpriteRenderer spriteRenderer;
      
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        PressButton.press1 = false;
        PressButton.press2 = false;
    }
    public void Contact(bool isContact)
    {
        if (isContact)
        {
            spriteRenderer.material = outlineMaterial;
            if (hint)
            {
                hint.SetActive(true);
            }
        }
        else
        {
            spriteRenderer.material = defaultMaterial;
            if (hint)
            {
                hint.SetActive(false);
            }
        }
    }
    public void StartEvent()
    {
        if (eventAnimation)
        {
            eventAnimation.enabled = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (itsButton && collision.gameObject.tag != "Ground")
        {
            animator.SetBool("Start",true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        animator.SetBool("Start", false);
    }
    private void isStart()
    {
        if (PressButton.press1 && PressButton.press2)
        {
            StartEvent();
            PressButton.press1 = false;
            PressButton.press2 = false;
        }
    }
}
