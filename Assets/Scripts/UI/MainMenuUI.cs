using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    private TMP_InputField inputNameField;
    private Button startButton;
    [SerializeField] private GameObject MainPanel;
    [SerializeField] private GameObject OptionsPanel;
    // Start is called before the first frame update
    void Start()
    {
        this.inputNameField = GameObject.Find("InputNameField (TMP)").GetComponent<TMPro.TMP_InputField>();
        this.startButton = GameObject.Find("StartButton").GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        startButton.interactable = (this.inputNameField.text.Length >= 3 && this.inputNameField.text.Length <= 8);
    }


    public void StartNewGame()
    {
        GameManager.Instance.SetPlayerName(this.inputNameField.text);
        Debug.Log("Current Name: " + this.inputNameField.text);
        SceneManager.LoadScene(1);
    }

    public void ShowHallOfFame()
    {
        SceneManager.LoadScene(2);
    }

    public void ShowOptions()
    {
        this.MainPanel.SetActive(false);
        this.OptionsPanel.SetActive(true);
    }

    public void ReturnFromOptions()
    {
        this.OptionsPanel.SetActive(false);
        this.MainPanel.SetActive(true);
    }

    public void Exit()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
}
