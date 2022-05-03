using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGradation : MonoBehaviour
{
    [SerializeField] float timeGradation;
    private SpriteRenderer sprite;
    public static bool isStarted { set { isStart = value; Invert(); }}
    public static bool isInvert;
    private Color color = Color.white;
    float counter = 0;
    private static bool isStart;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        timeGradation = 1/(timeGradation* 50);
        color.a = 0;
        sprite.color = color;
        isInvert = true;
    }
    public void FixedUpdate()
    {
        if (isStart)
        {
            if (counter > 1)
            {
                isStart = false;
                //isInvert = true;
                counter = 1;
            }
            else if (counter < 0)
            {
                isStart = false;
                //isInvert = false;
                counter = 0;
            }
            if (!isInvert && counter+timeGradation<=1)
            { 
                counter+= timeGradation;
                Debug.Log(isInvert);
            }
            else if(isInvert && counter - timeGradation >= 0)
            {
                counter -= timeGradation;               
                Debug.Log(isInvert);
            }
            color.a = counter;
            sprite.color = color;
            Debug.Log(counter);
        }
    }
    private static void Invert()
    {
        isInvert = !isInvert;
    }
}
