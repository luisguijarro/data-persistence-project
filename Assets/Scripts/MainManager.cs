using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    [SerializeField] private GameObject LivesGO;
    [SerializeField] private RawImage[] Hearts;
    [SerializeField] private Texture[] HeartImages;
    [SerializeField] private Brick BrickPrefab;
    private int lineCount = 6;
    internal int totalBricks;
    [SerializeField] private Rigidbody Ball;
    [SerializeField] private GameObject Paddle;

    public Text ScoreText;
    [SerializeField] private GameObject bestScoreText;
    [SerializeField] private GameObject GameOverText;
    [SerializeField] private GameObject VictoryText;
    [SerializeField] private GameObject[] particles;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    [SerializeField] GameObject pauseMenu;
    public bool IsPaused = false;
    private Vector3 forceDir;

    int maxLives = 3;
    private int lives;

    private Vector3 initPaddlePos;
    private Vector3 initBallPos;
    private Quaternion initBallRot;
    private GameManager gameManager;

    
    // Start is called before the first frame update
    void Start()
    {
        this.gameManager = GameManager.Instance;
        if (this.gameManager != null)
        {
            if (this.gameManager.scoreOfPlayers.Count > 0)
            {
                this.bestScoreText.GetComponent<Text>().text = "Best Score: " + 
                this.gameManager.scoreOfPlayers[this.gameManager.scoreOfPlayers.Count-1].Score.ToString("0000");
            }
            else
            {
                this.bestScoreText.GetComponent<Text>().text = "Best Score: 0000";
            }
            this.maxLives = (this.gameManager.HardCoreMode) ? 1 : 3;
            this.LivesGO.SetActive(!this.gameManager.HardCoreMode);
            this.lineCount = (int)this.gameManager.NumberOfBricksLines;
        }

        this.lives = maxLives;
        this.UpdateHearts();
        this.initBallPos = Ball.gameObject.transform.position;
        this.initBallRot = Ball.gameObject.transform.rotation;
        this.initPaddlePos = Paddle.transform.position;

        const float step = 0.6f;
        int perLine = 6; //Mathf.FloorToInt(4.0f / step);
        
        int[] posiblePoints = new [] {1, 2, 5, 7, 11};
        int[] pointCountArray = new int[6*lineCount]; // {1,1,2,2,5,5};
        int cont = 0;
        for (int l = 0; l < lineCount;l++)
        {
            int pointsToAssign = posiblePoints[Mathf.FloorToInt((l)/2f)];
            for (int b = 0; b < 6; b++)
            {
                pointCountArray[cont] = pointsToAssign;
                Debug.Log("Points: " + pointCountArray[cont].ToString("00"));
                cont++;
            }    
        }

        float initPosY = 4.3f - (lineCount * 0.3f);

        for (int i = 0; i < lineCount; i++)
        {
            for (int x = 0; x < perLine; x++)
            {
                Vector3 position = new Vector3(-1.5f + step * x, /*2.5f*/initPosY + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[this.totalBricks];
                brick.onDestroyed.AddListener(AddPoint);
                this.totalBricks++;
            }
        }
        ScoreText.text = $"Score {this.gameManager.currentName}: {m_Points}";
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                this.forceDir = new Vector3(randomDirection, 1, 0);
                this.forceDir.Normalize();

                Ball.gameObject.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
                Ball.gameObject.GetComponent<SphereCollider>().enabled = true;
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                this.ReloadScene();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!this.IsPaused)
            {
                this.PauseGame();
            }
            else
            {
                this.ResumeGame();
            }
        }
        
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    void AddPoint(int point)
    {
        m_Points += (int)(point * (this.gameManager.BallVelocity / 3f));
        ScoreText.text = $"Score {this.gameManager.currentName}: {m_Points}";
        totalBricks--;
        if (totalBricks <= 0)
        {
            ShowVictory();
        }
    }

    private void PauseGame()
    {
        this.IsPaused = true;
        this.forceDir = Ball.velocity.normalized;
        Ball.Sleep();
        this.pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        this.pauseMenu.SetActive(false);
        this.IsPaused = false;
        Ball.velocity = Vector3.zero;
        Ball.WakeUp();
        Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
        //Debug.Log("WakeUp: " + !GameObject.Find("Ball").GetComponent<Rigidbody>().IsSleeping());
    }

    private void Reinit() // Only If Lives > 0
    {
        this.m_Started = false;
        Ball.velocity = Vector3.zero;
        Ball.gameObject.GetComponent<SphereCollider>().enabled = false; // Prevent Launch event on reinit.
        this.Paddle.transform.position = this.initPaddlePos;
        this.Ball.transform.position = this.initBallPos;
        this.Ball.transform.rotation = this.initBallRot;
        Ball.transform.SetParent(this.Paddle.transform);
    }

    public void OneLiveLess()
    {
        this.lives--;
        if (this.lives <= 0)
        {
            this.GameOver();
        }
        else
        {
            this.Reinit(); // Play the next live.
        }
        this.UpdateHearts();
    }

    private void UpdateHearts()
    {
        this.Hearts[2].texture = (this.lives == 3) ? this.HeartImages[1] : this.HeartImages[0];
        this.Hearts[1].texture = (this.lives >= 2) ? this.HeartImages[1] : this.HeartImages[0];
        this.Hearts[0].texture = (this.lives >= 1) ? this.HeartImages[1] : this.HeartImages[0];
    }

    private void ShowVictory()
    {
        Destroy(this.Ball);
        this.VictoryText.SetActive(true);
        this.gameManager.AddScore(this.m_Points);
        foreach (GameObject go in this.particles)
        {
            go.SetActive(true);
        }
    }

    public void GameOver()
    {
        Destroy(this.Ball);
        m_GameOver = true;
        GameOverText.SetActive(true);
        this.gameManager.AddScore(this.m_Points);
    }
}
