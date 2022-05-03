using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI UIData;
    public GameObject losePanel;
    public Text loseText;
    public GameObject victoryPanel;
    public GameObject menuPanel;
    [SerializeField] GameObject hint;
    private Text hintText;
    private void Awake()
    {
        UIData = this;
        hintText = hint.GetComponentInChildren<Text>();
    }
    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void NextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index + 1);
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
                menuPanel.SetActive(true);
        }
    }
    public void CloseMenu()
    {
        menuPanel.SetActive(false);
    }
}
