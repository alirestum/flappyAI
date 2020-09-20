using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using GeneticAlgorithm;
using UnityEngine;

public class Piperenderer : MonoBehaviour
{
    #region variables
    private List<Pipe> pipes = new List<Pipe>();
    public GameObject pipePrefab;
    private System.Random random = new System.Random();
    private Piperenderer _instance;
    private const float Spacing = 6;

    public float TravelDistance
    {
        get;
        private set;
    }
    public float Speed
    {
        get;
        set;
    }
    

    #endregion
    private void Awake()
    {
        if (_instance == null)
        {
            this._instance = this.GetComponent<Piperenderer>();
        }

        this.Speed = 0.1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        generateInitialPipes();
    }

    // Update is called once per frame
    void Update()
    {
        TravelDistance += Speed;
        if (Input.GetKeyDown(KeyCode.A))
        {
            Speed += 0.1f;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            resetPiperenderer();
        }
    }

    void moveToTheEnd(Pipe pipe)
    {
        float lastPipeXCoord = (from p in pipes orderby p.transform.position.x descending select p.transform.position.x).First(); 
        Vector3 newPosition = new Vector3(lastPipeXCoord + 6, generateYCoord(), 0.0f);
        pipe.transform.position = newPosition;
    }

    private float generateYCoord()
    {
        return (float)(random.NextDouble() * (-0.11 - -2.7) + -2.7);
    }

    public void resetPiperenderer()
    {
        foreach (Pipe pipe in pipes)
        {
            Destroy(pipe.gameObject);
        }
        pipes.Clear();
        generateInitialPipes();
        Speed = 0.1f;
    }

    private void generateInitialPipes()
    {
        for (int i = 0; i < 6; i++)
        {
            Vector3 pos = new Vector3(Spacing * i, generateYCoord(), 0);
            GameObject newPipe = Instantiate(pipePrefab, pos, Quaternion.identity);
            newPipe.transform.SetParent(_instance.transform);
            newPipe.GetComponent<Pipe>().onBecameInvisible += moveToTheEnd;
            newPipe.GetComponent<Pipe>().PiperendererInstance = this;
            pipes.Add(newPipe.GetComponent<Pipe>());
        }
    }

    public double getDistanceToClosestPipe(Vector3 birdPosition)
    {
        return Vector3.Distance(birdPosition, getClosestPipe().transform.position);
    }

    public Pipe getClosestPipe()
    {
        return (from p in pipes orderby p.transform.position.x select p).First();
    }
}
