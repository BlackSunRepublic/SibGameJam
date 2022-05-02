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
    public void MessageText(string text, int seconds)
    {
        hint.SetActive(true);
        hintText.text = text;
        StartCoroutine(SendHint(seconds));
    } 
    IEnumerator SendHint(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        hintText.text = "";
        hint.SetActive(false);
    } 
}
