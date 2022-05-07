using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IntEvent : UnityEvent<int>
{
}

public class Timer : MonoBehaviour
{
    #region Singleton
    private static Timer _instance;
    public static Timer Instance { get { return _instance; } }
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
    float gameTime;
    [SerializeField]
    float timeMultiplier;
    [SerializeField]
    float timeMultiplierStep; //.1f
    [SerializeField]
    TMP_Text timeText; //move this to UIController

    public UnityEvent e_AddTime;
    public IntEvent e_ChangeTimeSpeed;

    public float GameTime
    {
        get { return gameTime; }
        set { gameTime = value; }
    }

    private void Start()
    {
        if (e_AddTime == null)
            e_AddTime = new UnityEvent();
        if (e_ChangeTimeSpeed == null)
            e_ChangeTimeSpeed = new IntEvent();

        e_AddTime.AddListener(AddTime);
        e_ChangeTimeSpeed.AddListener(ChangeTimeSpeed);
        GameManager.Instance.e_StartGame.AddListener(ResetAndStartTimer);
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.GameState == GameManager.State.Game)
        {
            ShowCurrentTime();
            DecreaseTime();
        }
    }

    void ResetAndStartTimer()
    {
        print("Timer: ResetAndStartTimer");
        GameTime = 60f;
        timeMultiplier = 1f;
        timeMultiplierStep = .1f;
    }
    void DecreaseTime()
    {
        GameTime -= Time.deltaTime * timeMultiplier;
        //print(GameTime);
        if (GameTime <= 0)
        {
            GameManager.Instance.StopGame("timeout");
        }
    }
    void ShowCurrentTime()
    {
        //print("Timer: ShowCurrentTime");
        timeText.text = "Time: " + Mathf.RoundToInt(GameTime).ToString();
    }
    void AddTime() //value range <2,5>
    {
        print("Timer: AddTime");
        GameTime += Mathf.RoundToInt(Random.Range(2f, 5f));
    }

    void ChangeTimeSpeed(int value)
    {
        print("Timer: ChangeTimeSpeed");
        timeMultiplier += timeMultiplier * timeMultiplierStep * value;
    }
}
