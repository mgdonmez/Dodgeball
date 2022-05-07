using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class Ball : MonoBehaviour
{
    private Rigidbody rb;

    public float ballSpeed;
    public float BallSpeed
    {
        get { return ballSpeed; }
        set { ballSpeed = value; }
    }
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        ballSpeed = 6f;
        PlayerController.Instance.e_Shoot.AddListener(Shoot);
        PlayerController.Instance.e_IncreaseBallSpeed.AddListener(IncreaseBallSpeed);
        PlayerController.Instance.e_ResetBall.AddListener(ResetBall);
        GameManager.Instance.e_StartGame.AddListener(ResetBall);
    }

    void ResetBall()
    {
        StopCoroutine(ShootBall());
        ballSpeed = 6f;
        rb.velocity = Vector3.zero;
        transform.position = -Vector3.one;
    }

    void IncreaseBallSpeed()
    {
        if (BallSpeed < 10)
            BallSpeed += .5f;
        else
            BallSpeed = 10;
    }

    void Shoot()
    {
        StartCoroutine(ShootBall());
    }

    private IEnumerator ShootBall()
    {
        rb.velocity = ballSpeed * transform.forward;
        yield return new WaitForSecondsRealtime(.5f);

        while (Mathf.Abs(PlayerController.Instance.transform.position.x - transform.position.x) > .5f || 
            Mathf.Abs(PlayerController.Instance.transform.position.z - transform.position.z) > .5f)
        {
            if (PlayerController.Instance.hasBall)
                break;
            rb.velocity = ballSpeed * (PlayerController.Instance.transform.position - transform.position).normalized;
            yield return null;
        }
    }
}
