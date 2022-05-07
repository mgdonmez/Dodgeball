using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    #region Singleton
    private static PlayerController _instance;
    public static PlayerController Instance { get { return _instance; } }
    
    #endregion

    private Rigidbody rb;
    
    public float movementSpeed;
    public float rotationSpeed;

    [SerializeField]
    GameObject ball;
    //[SerializeField]
    //Transform startPos;
    public bool hasBall;

    public UnityEvent e_IncreaseBallSpeed;
    public UnityEvent e_Shoot;
    public UnityEvent e_ResetBall;
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

        if (e_Shoot == null)
            e_Shoot = new UnityEvent();
        if (e_ResetBall == null)
            e_ResetBall = new UnityEvent();
        if (e_IncreaseBallSpeed == null)
            e_IncreaseBallSpeed = new UnityEvent();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hasBall = true;
        
        GameManager.Instance.e_StartGame.AddListener(ResetPlayer);
    }

    void FixedUpdate()
    {
        var ForwardDirection = transform.forward;
        //var RightDirection = Vector3.right;

        // Move Forwards
        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = ForwardDirection * movementSpeed;
        }
        // Move Back
        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity = -ForwardDirection * movementSpeed;
        }
        // Move Right
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(.0f, rotationSpeed, .0f, Space.World);
        }
        // Move Left
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(.0f, -rotationSpeed, .0f, Space.World);
        }
        if(Input.GetKey(KeyCode.Space) && hasBall)
        {
            Shoot();
        }
    }

    void ResetPlayer()
    {
        hasBall = true;
        //transform.position = startPos.position;
    }
    void Shoot()
    {
        hasBall = false;
        ball.transform.position = transform.position + transform.forward * .5f;
        ball.transform.rotation = transform.rotation;
        e_Shoot.Invoke();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider == ball.GetComponent<Collider>())
        {
            print("HAS BALL");
            hasBall = true;
            e_ResetBall.Invoke();
        }
    }
}
