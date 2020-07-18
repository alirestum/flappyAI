using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Pipe : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 0.1f;
    public event Action onBecameInvisible;

    // Start is called before the first frame update
    void Start()
    {
        //rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPos = gameObject.transform.position;
        currentPos += new Vector3(-speed, 0.0f, 0.0f);
        gameObject.transform.position = currentPos;
        //rb.position += new Vector2(-speed, 0.0f);
    }

    private void OnBecameInvisible()
    {
        onBecameInvisible?.Invoke();
    }

}
