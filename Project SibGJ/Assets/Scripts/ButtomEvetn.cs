using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtomEvetn : MonoBehaviour
{
    [SerializeField] private bool itsFirstButton;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Ground")
        {
            if (itsFirstButton)
            {
                PressButton.press1 = true;
            }
            else
            {
                PressButton.press2 = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Ground")
        {
            if (itsFirstButton)
            {
                PressButton.press1 = false;
            }
            else
            {
                PressButton.press2 = false;
                PressButton.press2 = false;
            }
        }
    }
    public void Finish()
    {
        UI.UIData.victoryPanel.SetActive(true);
    }
}
