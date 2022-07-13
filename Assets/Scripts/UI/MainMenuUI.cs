using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputNameField;
    [SerializeField] private Button startButton;
    // Start is called before the first frame update
    void Start()
    {
        //this.inputNameField = GameObject.Find("InputNameField (TMP)").GetComponent<TMPro.TMP_InputField>();
        if (GameManager.Instance != null && this.inputNameField != null)
        {
            if (GameManager.Instance.currentName.Length >= 3) { this.inputNameField.text = GameManager.Instance.currentName; }
        }
        else
        {
            if (this.inputNameField == null) { Debug.Log("this.inputNameField is NULL"); }
        }
        //this.startButton = GameObject.Find("StartButton").GetComponent<Button>();
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
        SceneManager.LoadScene(3); // Cargamos escena con el menú de opciones.
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
