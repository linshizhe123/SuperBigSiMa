using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlayer : MonoBehaviour
{
    public Rigidbody2D rb;
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        Time.timeScale = 0.7f;
        rb.AddForce(new Vector2(0, 16f), ForceMode2D.Impulse);
        GameManager.PlayerDied();
    }
}
