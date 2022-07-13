using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HallOfFameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp_Higher;
    [SerializeField] private TextMeshProUGUI tmp_HigherValue;
    [SerializeField] private TextMeshProUGUI tmp_Scores;
    [SerializeField] private TextMeshProUGUI tmp_ScoresValues;

    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        this.gameManager = GameManager.Instance;
        UpdateScores();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateScores()
    {
        string s_scores = "";
        string s_scoresValues = "";
        if (GameManager.Instance.scoreOfPlayers.Count > 0)
        {
            int lastIndex = this.gameManager.scoreOfPlayers.Count-1;
            this.tmp_Higher.text = this.gameManager.scoreOfPlayers[lastIndex].Name;
            this.tmp_HigherValue.text = this.gameManager.scoreOfPlayers[lastIndex].Score.ToString("0000");
            //---------------------------------------------------------------------------
            int max = this.gameManager.scoreOfPlayers.Count > 5 ?this.gameManager.scoreOfPlayers.Count-6 : 0;
            int min = this.gameManager.scoreOfPlayers.Count > 5 ?this.gameManager.scoreOfPlayers.Count-2 : this.gameManager.scoreOfPlayers.Count-1;
            for (int i = min; i >= max; i--)
            {
                s_scores += this.gameManager.scoreOfPlayers[i].Name + System.Environment.NewLine;
                s_scoresValues += this.gameManager.scoreOfPlayers[i].Score.ToString("0000") + System.Environment.NewLine;
            }
        }
        else
        {
            this.tmp_Higher.text = "";
            this.tmp_HigherValue.text  ="";
            // s_scores = "No scores registered";
        }
        
        this.tmp_Scores.text = s_scores;
        this.tmp_ScoresValues.text = s_scoresValues;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
 