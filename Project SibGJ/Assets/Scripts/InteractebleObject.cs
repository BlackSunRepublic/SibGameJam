using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractebleObject : MonoBehaviour
{
    [SerializeField] Material outlineMaterial;
    [SerializeField] Material defaultMaterial;
    [SerializeField] GameObject hint;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Contact(bool isContact)
    {
        if (isContact)
        {
            spriteRenderer.material = outlineMaterial;
            hint.SetActive(true);
        }
        else
        {
            spriteRenderer.material = defaultMaterial;
            hint.SetActive(false);
        }
    }
}
