using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    private Rigidbody rb;
    public float movementSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameManager.Instance.e_StartGame.AddListener(ResetTarget);
        Move();
    }
    void Move()
    {
        StartCoroutine(MoveTarget());
    }

    private IEnumerator MoveTarget()
    {
        while (true)
        {
            rb.velocity = movementSpeed * (new Vector3(Random.Range(-5f, 5f), 1f, Random.Range(-5f, 5f)));
            yield return new WaitForSecondsRealtime(Random.Range(2f, 5f));
        }
    }

    void ResetTarget() //object pooling is a better approach
    {
        Destroy(this);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            print("HIT!");
            ScoreCounter.Instance.e_ChangeScore.Invoke(5);
            gameObject.SetActive(false);
        }
    }
}
