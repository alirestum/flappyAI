using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NN;

public delegate void onHit(Collision2D collision);
public class Bird : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    public event onHit onHit;
    private NeuralNetwork brain;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float currentVelocityY = this.rigidbody2d.velocity.y;
        jump(currentVelocityY);
       
    }

    void jump(float currentVelocityY)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        { 
            if (currentVelocityY < 10)
            {
                this.rigidbody2d.AddForce(new Vector2(0.0f, 10.0f - currentVelocityY), ForceMode2D.Impulse);
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        onHit?.Invoke(collision);
    }

    public void resetBird()
    {
        rigidbody2d.constraints = RigidbodyConstraints2D.None;
        this.transform.position = new Vector3(-7.5f, 1.0f, 0.0f);
    }
}
