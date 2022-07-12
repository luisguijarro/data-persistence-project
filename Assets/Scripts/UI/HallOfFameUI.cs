using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HallOfFameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp_Scores;
    // Start is called before the first frame update
    void Start()
    {
        UpdateScores();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateScores()
    {
        string s_scores = "";
        if (GameManager.Instance.scoreOfPlayers.Count > 0)
        {
            int maxmin = GameManager.Instance.scoreOfPlayers.Count > 5 ? GameManager.Instance.scoreOfPlayers.Count-5 : 0;
            for (int i = GameManager.Instance.scoreOfPlayers.Count-1; i>=maxmin; i--)
            {
                s_scores += GameManager.Instance.scoreOfPlayers[i].Name + " " + GameManager.Instance.scoreOfPlayers[i].Score + System.Environment.NewLine;
            }
        }
        else
        {
            s_scores = "No scores registered";
        }
        
        this.tmp_Scores.text = s_scores;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
 