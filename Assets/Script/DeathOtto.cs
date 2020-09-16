using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathOtto : MonoBehaviour
{
    Rigidbody2D deathRb;

    void Start()
    {
        deathRb = transform.GetComponent<Rigidbody2D>();
        deathRb.AddForce(new Vector2(0f, 12f), ForceMode2D.Impulse);
    }
}
