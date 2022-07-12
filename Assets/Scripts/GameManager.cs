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

    private string currentName;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Instance.LoadScores();
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
        set { this.gameOptions.musicOn = value; SaveOptions(); }
        get { return this.gameOptions.musicOn; }
    }
    public float MusicVolume
    {
        set { this.gameOptions.musicVolume = value; SaveOptions(); }
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

    public int NumberOfBricksLines
    {
        set { this.gameOptions.numberOfBrickLines = value; SaveOptions(); }
        get { return this.gameOptions.numberOfBrickLines; }
    }

    public float BallVelocity
    {
        set { this.gameOptions.ballVelocity = value; SaveOptions(); }
        get { return this.gameOptions.ballVelocity; }
    }

    #endregion

    public void SaveOptions()
    {
        string json = JsonUtility.ToJson(this.gameOptions);
        File.WriteAllText(Application.persistentDataPath + "/saveWallOptions.json", json);
    }

    public void LoadOptions()
    {
        string path = Application.persistentDataPath + "/saveWallOptions.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            this.gameOptions = JsonUtility.FromJson<GameOptions>(json);
        }
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
