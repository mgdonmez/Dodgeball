using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Singleton
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }
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
    Canvas menuCanvas;
    [SerializeField]
    Canvas gameCanvas;
    //[SerializeField]
    //Canvas stopCanvas;

    private void Start()
    {
        GameManager.Instance.e_StartGame.AddListener(StartGame);
        GameManager.Instance.e_PauseGame.AddListener(PauseGame);
        GameManager.Instance.e_StopGame.AddListener(StopGame);
    }

    void StartGame()
    {
        menuCanvas.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(true);
    }
    void PauseGame()
    {
        menuCanvas.gameObject.SetActive(true);
        gameCanvas.gameObject.SetActive(false);
    }
    void StopGame()
    {
        menuCanvas.gameObject.SetActive(true);
        gameCanvas.gameObject.SetActive(false);
    }
}
