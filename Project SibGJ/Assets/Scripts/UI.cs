using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI UIData;
    public GameObject losePanel;
    public Text loseText;
    public GameObject victoryPanel;
    public GameObject menuPanel;
    private void Awake()
    {
        UIData = this;
    }
}
