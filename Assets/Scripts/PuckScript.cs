using System;
using System.Collections;
using UnityEngine;

public class PuckScript : MonoBehaviour
{
    public ScoreScript scoreScript;
    public float maxSpeed = 20f;
    
    private Rigidbody2D _rb;
    private bool _wasGoal = false;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PlayerGoal"))
        {
            scoreScript.Increment(false);
            _wasGoal = true;
            StartCoroutine(ResetPuck(false));
        }
        else if (col.CompareTag("AiGoal"))
        {
            scoreScript.Increment(true);
            _wasGoal = true;
            StartCoroutine(ResetPuck(true));
        }
    }

    IEnumerator ResetPuck(bool didAiScore)
    {
        yield return new WaitForSeconds(1);
        _rb.velocity = Vector2.zero;
        _wasGoal = false;

        if (didAiScore)
        {
            _rb.position = Vector2.down;
        }
        else
        {
            _rb.position = Vector2.up;
        }
    }

    private void FixedUpdate()
    {
        if (_wasGoal)
        {
            _rb.velocity = Vector2.zero;
        }
        else
        {
            _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, maxSpeed);
        }
    }
}
