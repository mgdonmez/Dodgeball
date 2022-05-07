using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScoreCounter : MonoBehaviour
{
    #region Singleton
    private static ScoreCounter _instance;
    public static ScoreCounter Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    [SerializeField]
    int score;
    bool highScore = false;
    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    [SerializeField]
    TMP_Text scoreText; //move this to UIController

    public UnityEvent e_AddTime;
    public IntEvent e_ChangeScore;
    private void Start()
    {
        //DEBUG ONLY:
        PlayerPrefs.DeleteKey("HighScore");

        if (e_ChangeScore == null)
            e_ChangeScore = new IntEvent();

        e_ChangeScore.AddListener(ChangeScore);
        GameManager.Instance.e_StartGame.AddListener(ResetScore);
    }
    private void FixedUpdate()
    {
        ShowCurrentScore();
    }
    void ResetScore()
    {
        print("Timer: ResetScore");
        Score = 0;
        highScore = false;
        if (!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }
    }
    void ShowCurrentScore()
    {
        print("Timer: ShowCurrentScore");
        if(!highScore)
            scoreText.text = "Score: " + Score.ToString();
        else
            scoreText.text = "New High Score: " + Score.ToString();
    }
    void ChangeScore(int value)
    {
        score += value;
        if(score > PlayerPrefs.GetInt("HighScore"))
        {
            highScore = true;
            PlayerPrefs.SetInt("HighScore", score);
        }
    }
}
