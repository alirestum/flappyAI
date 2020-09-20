using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using UnityEngine;
using NN;
using UnityEditor;

public delegate void onHit(Bird bird);

public delegate void onBecomeInvisible(Bird bird);
public class Bird : MonoBehaviour
{
    #region variables
    public event onHit onHit;
    public event onBecomeInvisible onBecomeInvisible;
    public Rigidbody2D Rigidbody2d
    {
        get;
        private set;
    }
    public NeuralNetwork Brain
    {
        get;
        set;
    }
    private Matrix<double> inputMatrix;
    public Piperenderer PiperendererInstance
    {
        get;
        set;
    }
    public Bird Instance
    {
        get;
        set;
    }
    public bool Dead
    {
        get;
        set;
    }

    public int Fittness
    {
        get;
        private set;
    }
    private int frameCnt = 0;
    #endregion

    public bool DebugNeeded
    {
        get;
        set;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = GetComponent<Bird>();
        }

        Rigidbody2d = GetComponent<Rigidbody2D>();
        Brain = new NeuralNetwork();
        Dead = false;
        inputMatrix = new DenseMatrix(3,3);
    }

    // Update is called once per frame
    void Update()
    {
        if (!Dead)
        {
            /*Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, PiperendererInstance.getClosestPipe().Bottom.GetComponent<SpriteRenderer>().bounds.max);
                Gizmos.DrawLine(transform.position, PiperendererInstance.getClosestPipe().Top.GetComponent<SpriteRenderer>().bounds.min);*/
            Think();
        }
    }

    private void Think()
    {
        inputMatrix[frameCnt % 3, 0] = transform.position.y;
        inputMatrix[frameCnt % 3, 1] = Vector3.Distance(transform.position,PiperendererInstance.getClosestPipe().Top.GetComponent<SpriteRenderer>().bounds.min);
        inputMatrix[frameCnt % 3, 2] = Vector3.Distance(transform.position,PiperendererInstance.getClosestPipe().Bottom.GetComponent<SpriteRenderer>().bounds.max);
        frameCnt++;
        if (frameCnt % 3 == 0)
        {
            if (this.Brain.Think(inputMatrix))
            {
                float currentVelocityY = this.Rigidbody2d.velocity.y;
                jump(currentVelocityY);
            }
        }
        
    }

    void jump(float currentVelocityY)
    {
        if (currentVelocityY < 10)
        {
            this.Rigidbody2d.AddForce(new Vector2(0.0f, 10.0f - currentVelocityY), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!Dead)
        {
            Dead = true;
            Fittness = frameCnt;
            onHit?.Invoke(this);
        }
    }

    private void OnBecameInvisible()
    {
        if (!Dead)
        {
            Dead = true;
            Fittness = frameCnt;
            onBecomeInvisible?.Invoke(this);
        }
    }
}
