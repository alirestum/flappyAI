using System;
using UnityEngine;

public delegate void onPipeBecomeInvisible(Pipe pipe);
public class Pipe : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 0.1f;
    public event onPipeBecomeInvisible onBecameInvisible;

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPos = gameObject.transform.position;
        currentPos += new Vector3(-speed, 0.0f, 0.0f);
        gameObject.transform.position = currentPos;
    }

    private void OnBecameInvisible()
    {
        onBecameInvisible?.Invoke(this);
    }

}
