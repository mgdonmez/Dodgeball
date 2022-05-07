using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
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

    public enum State
    {
        Pause, //show menu
        Game, //show game ui, game is playable
        Stop //gameover screen
    }

    private State gameState; // field
    public State GameState   // property
    {
        get { return gameState; }   // get method
        set { gameState = value; }  // set method
    }

    public UnityEvent e_StartGame;
    public UnityEvent e_PauseGame;
    public UnityEvent e_StopGame;

    [SerializeField]
    GameObject TargetGO;
    [SerializeField]
    int numOfTargets = 4;
    bool newWave = false;
    private void Start()
    {
        GameState = State.Pause;
        if (e_StartGame == null)
            e_StartGame = new UnityEvent();
        if (e_PauseGame == null)
            e_PauseGame = new UnityEvent();
        if (e_StopGame == null)
            e_StopGame = new UnityEvent();

        //targetPool = new ObjectPool<GameObject>(
        //    createFunc: () => new GameObject("Target"),
        //    actionOnGet: (obj) => obj.SetActive(true),
        //    actionOnRelease: (obj) => obj.SetActive(false),
        //    actionOnDestroy: (obj) => Destroy(obj),
        //    defaultCapacity: 5,
        //    maxSize: 10);
    }

    public void StartGame()
    {
        GameState = State.Game;
        print("StartGame: " + GameState);
        numOfTargets = 4;
        e_StartGame.Invoke();
        StartCoroutine(GameLoop());
    }
    public void PauseGame(string reason)
    {

    }
    public void StopGame(string reason)
    {
        print(reason);
        numOfTargets = 0;
        GameState = State.Stop;
        e_StopGame.Invoke();
    }
    public void QuitGame()
    {
        print("QuitGame: Quit");
        Application.Quit();
    }

    private IEnumerator GameLoop()
    {
        while(GameState == State.Game)
        {
            yield return new WaitForSecondsRealtime(1f);
            Wave();
            yield return new WaitUntil(() => newWave);

            if (numOfTargets < 10)
                numOfTargets++;

            yield return null;
        }
        yield return null;
    }
    void Wave()
    {
        newWave = false;
        TargetSpawn();
    }
    void TargetSpawn()
    {
        for (int i = 0; i < numOfTargets ; i++)
        {
            GameObject target = Instantiate(TargetGO, transform);
            if (target != null)
            {
                target.transform.position = new Vector3(Random.Range(-15f, 15f), 1f, Random.Range(-15f, 15f));
                target.transform.rotation = transform.rotation;
                target.SetActive(true);
            }
        }
    }
}
