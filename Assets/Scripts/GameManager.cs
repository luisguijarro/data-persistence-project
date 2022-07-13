using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<PlayerScore> scoreOfPlayers;
    public GameOptions gameOptions;
    private AudioSource audioSource;

    internal string currentName;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        this.currentName = "";
        this.audioSource = this.gameObject.GetComponent<AudioSource>();
        Instance.LoadScores();
        Instance.LoadOptions();
    }

    public void SetPlayerName(string name)
    {
        this.currentName = name;
    }

    public void AddScore(int score) 
    {
        this.scoreOfPlayers.Add(new PlayerScore(this.currentName, score));
        this.scoreOfPlayers = this.scoreOfPlayers.OrderBy(x => x.Score).ToList();
        this.SaveScores();
    }

    #region SetOptions

    public bool musicOn
    {
        set { this.gameOptions.musicOn = value; this.audioSource.mute = !value; SaveOptions(); }
        get { return this.gameOptions.musicOn; }
    }
    public float MusicVolume
    {
        set { this.gameOptions.musicVolume = value; this.audioSource.volume = value; SaveOptions(); }
        get { return this.gameOptions.musicVolume; }
    }

    public float SoundsVolume
    {
        set { this.gameOptions.soundsVolume = value; SaveOptions(); }
        get { return this.gameOptions.soundsVolume; }
    }

    public bool HardCoreMode
    {
        set { this.gameOptions.hardCoreMode = value; SaveOptions(); }
        get { return this.gameOptions.hardCoreMode; }
    }

    public float NumberOfBricksLines
    {
        set { this.gameOptions.numberOfBrickLines = (int)value; SaveOptions(); }
        get { return (float)this.gameOptions.numberOfBrickLines; }
    }

    public float BallVelocity
    {
        set { this.gameOptions.ballVelocity = value; SaveOptions(); }
        get { return this.gameOptions.ballVelocity; }
    }

    #endregion

    public bool SaveOptions()
    {
        string json = JsonUtility.ToJson(this.gameOptions);
        string path = Application.persistentDataPath + "/saveWallOptions.json";
        File.WriteAllText(path, json);
        return File.Exists(path);
    }

    public void LoadOptions()
    {
        string path = Application.persistentDataPath + "/saveWallOptions.json";
        //File.Delete(path);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            this.gameOptions = JsonUtility.FromJson<GameOptions>(json);
            Debug.Log("Options Loaded");
        }
        else // If dont exist we create Options file by Default.
        {
            this.gameOptions = new GameOptions();
            this.gameOptions.ballVelocity = 3.0f;
            this.gameOptions.hardCoreMode = false;
            this.gameOptions.musicOn = true;
            this.gameOptions.musicVolume = 0.5f;
            this.gameOptions.numberOfBrickLines = 6;
            this.gameOptions.soundsVolume = 0.5f;
            if (this.SaveOptions())
            {
                Debug.Log("Options file has been Created");
            }
            else
            {
                Debug.Log("Options file has been not Created");
            }
        }
        
        this.audioSource.mute = !this.gameOptions.musicOn;
        this.audioSource.volume = this.gameOptions.musicVolume;
    }

    public void SaveScores()
    {
        PlayerScore[] list = this.scoreOfPlayers.ToArray();

        string json = JsonUtility.ToJson(new JsonScores(list));
        Debug.Log("Json String: " + System.Environment.NewLine + "  " + json);
        File.WriteAllText(Application.persistentDataPath + "/saveWallScore.json", json);
        Debug.Log("Scores File Writed");
    }

    public void LoadScores()
    {
        this.scoreOfPlayers = new List<PlayerScore>();
        string path = Application.persistentDataPath + "/saveWallScore.json";
        
        if (File.Exists(path))
        {
            Debug.Log("File Exist!");
            string json = File.ReadAllText(path);
            JsonScores js = JsonUtility.FromJson<JsonScores>(json);
            PlayerScore[] data = js.PlayerScore;

            if (data != null)
            {
                this.scoreOfPlayers.AddRange(data);
                Debug.Log("Objects Readed: " + data.Length);
            }
            Debug.Log("Objects Added: " + this.scoreOfPlayers.Count);            
        }
        else
        {
            Debug.Log("File not exist: " + path);
        }
    }

    private void DeleteScores()
    {
        string path = Application.persistentDataPath + "/saveWallScore.json";
        File.Delete(path); return;
    }

    [System.Serializable]
    public struct JsonScores
    {
        public PlayerScore[] PlayerScore;
        public JsonScores(PlayerScore[] playerScore)
        {
            this.PlayerScore = playerScore;
        }
    }

    [System.Serializable]
    public struct GameOptions
    {
        public bool musicOn;
        public float musicVolume;
        public float soundsVolume;
        public int numberOfBrickLines;
        public float ballVelocity;
        public bool hardCoreMode; // Without lives.
    }
}
