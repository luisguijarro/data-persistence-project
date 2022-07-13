using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class OptionsMenuUI : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] Toggle musicOnToggle;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider soundsVolumeSlider;
    [SerializeField] Toggle hardcoreModeToggle;
    [SerializeField] Slider numOfBrickLinesSlider;
    [SerializeField] TextMeshProUGUI numOfBrickLinesText;
    [SerializeField] Slider velocityBallSlider;
    [SerializeField] TextMeshProUGUI velocityBallText;
    // Start is called before the first frame update
    void Start()
    {
        this.gameManager = GameManager.Instance;
        if (this.gameManager != null)
        {
            this.LoadDefaultValues();
        }
        else
        {
            Debug.Log("this.gameManager is null.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadDefaultValues()
    {
        this.musicOnToggle.isOn = this.gameManager.musicOn;
        this.musicVolumeSlider.value = this.gameManager.MusicVolume;
        this.soundsVolumeSlider.value = this.gameManager.SoundsVolume;
        this.hardcoreModeToggle.isOn = this.gameManager.HardCoreMode;
        this.numOfBrickLinesSlider.value = this.gameManager.NumberOfBricksLines;
        this.numOfBrickLinesText.text = this.gameManager.NumberOfBricksLines.ToString("00");
        this.velocityBallSlider.value = this.gameManager.BallVelocity;
        this.velocityBallText.text = this.gameManager.BallVelocity.ToString("00");
        Debug.Log("Default Values are Loaded.");
    }
        
    #region SetOptions

    public bool musicOn
    {
        set { this.gameManager.musicOn = value; }
        get { return this.gameManager.musicOn; }
    }
    public float MusicVolume
    {
        set { this.gameManager.MusicVolume = value; }
        get { return this.gameManager.MusicVolume; }
    }

    public float SoundsVolume
    {
        set { this.gameManager.SoundsVolume = value; }
        get { return this.gameManager.SoundsVolume; }
    }

    public bool HardCoreMode
    {
        set { this.gameManager.HardCoreMode = value; }
        get { return this.gameManager.HardCoreMode; }
    }

    public float NumberOfBricksLines
    {
        set { this.gameManager.NumberOfBricksLines = value; this.numOfBrickLinesText.text = value.ToString("00"); }
        get { return (float)this.gameManager.NumberOfBricksLines; }
    }

    public float BallVelocity
    {
        set { this.gameManager.BallVelocity = value; this.velocityBallText.text = value.ToString("00"); }
        get { return this.gameManager.BallVelocity; }
    }

    #endregion

    public void ReturToMainMenu()
    {
        SceneManager.LoadScene(0); // Load Main Menu Scene.
    }
}
