using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using UnityEngine;
using NN;

public delegate void onHit(Bird bird);

public delegate void onBecomeInvisible(Bird bird);
public class Bird : MonoBehaviour
{
    public Rigidbody2D _rigidbody2d
    {
        get;
        private set;
    }
    public event onHit onHit;
    public event onBecomeInvisible onBecomeInvisible;

    public NeuralNetwork Brain
    {
        get;
        set;
    }
    private int _frames;
    private Matrix<double> inputMatrix;

    public float TravelledDistance
    {
        get;
        set;
    }
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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = GetComponent<Bird>();
        }
        _rigidbody2d = GetComponent<Rigidbody2D>();
        Brain = new NeuralNetwork();
        Dead = false;
        inputMatrix = new DenseMatrix(3,3);
        this._frames = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Dead)
        {
            Think();
        }
    }

    private void Think()
    {
        inputMatrix[_frames, 0] = this._rigidbody2d.velocity.y;
        inputMatrix[_frames, 1] = this.transform.position.y;
        inputMatrix[_frames, 2] = PiperendererInstance.getDistanceToClosestPipe(this.transform.position);
        _frames++;
        if (_frames == 3)
        {
            _frames = 0;
            if (this.Brain.Think(inputMatrix))
            {
                float currentVelocityY = this._rigidbody2d.velocity.y;
                jump(currentVelocityY);
            }
        }
        
    }

    void jump(float currentVelocityY)
    {
        if (currentVelocityY < 10)
        {
            this._rigidbody2d.AddForce(new Vector2(0.0f, 10.0f - currentVelocityY), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!Dead)
        {
            Dead = true;
            onHit?.Invoke(this);
        }
    }

    public void resetBird()
    {

        Dead = false;
        Instance.Brain = new NeuralNetwork();
        transform.position = new Vector3(-7.5f, 1.0f, 0.0f);
        transform.rotation = Quaternion.identity;
        _rigidbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rigidbody2d.constraints = RigidbodyConstraints2D.FreezePositionX;
    }

    private void OnBecameInvisible()
    {
        if (!Dead)
        {
            Dead = true;
            onBecomeInvisible?.Invoke(this);
        }
    }
}
