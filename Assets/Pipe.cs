using System;
using UnityEngine;


public class Pipe : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 0.1f;
    public event Action onBecameInvisible;

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
