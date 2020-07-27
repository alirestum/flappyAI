using System;
using System.Collections.Generic;
using System.Net.Mail;
using GeneticAlgorithm;
using UnityEngine;

public class Piperenderer : MonoBehaviour
{
    private List<GameObject> pipes = new List<GameObject>();
    public GameObject pipePrefab;
    private float speed = 0.1f;
    private System.Random random = new System.Random();
    private Piperenderer _instance;

    public float TravelDistance
    {
        get;
        private set;
    }
    public float Speed
    {
        get { return speed; }
        set
        {
            speed = value;
            foreach (var pipe in pipes)
            {
                pipe.GetComponent<Pipe>().speed = value;
            }
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            this._instance = this.GetComponent<Piperenderer>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        generateInitialPipes();
    }

    // Update is called once per frame
    void Update()
    {
        TravelDistance += speed;
        if (Input.GetKeyDown(KeyCode.A))
        {
            Speed += 0.1f;
        }
    }

    void addnewPipe()
    {
        if (pipes.Count < 6)
        {
            Vector3 currentPos = pipes[pipes.Count-1].gameObject.transform.position;
            currentPos.x += 6.0f;
            currentPos.y = generateYCoord();
            GameObject newPipe = Instantiate(pipePrefab, currentPos, Quaternion.identity);
            newPipe.transform.SetParent(_instance.transform);
            newPipe.GetComponent<Pipe>().onBecameInvisible += removePipe;
            newPipe.GetComponent<Pipe>().speed = speed;
            pipes.Add(newPipe);
        }
    }
    void removePipe(Pipe pipe)
    {
        EventExecutors.RemovePipe(pipe, pipes);
        addnewPipe();
    }

    private float generateYCoord()
    {
        return (float)(random.NextDouble() * (-0.11 - -3.7) + -3.7);
    }

    public void resetPiperenderer()
    {
        for (int i = 0; i < pipes.Count; i++)
        {
            Destroy(pipes[i].gameObject);
            pipes.RemoveAt(i);
        }
        generateInitialPipes();
    }

    private void generateInitialPipes()
    {
        float spacing = 6;
        for (int i = 0; i < 6; i++)
        {
            Vector3 pos = new Vector3(spacing * i, generateYCoord(), 0);
            GameObject newPipe = Instantiate(pipePrefab, pos, Quaternion.identity);
            newPipe.transform.SetParent(_instance.transform);
            newPipe.GetComponent<Pipe>().onBecameInvisible += removePipe;
            newPipe.GetComponent<Pipe>().speed = speed;
            pipes.Add(newPipe);
        }
    }

    public double getDistanceToClosestPipe(Vector3 BirdPosition)
    {
        foreach (GameObject pipe in pipes)
        {
            if (pipe.transform.position.x > BirdPosition.x)
            {
                return Vector3.Distance(BirdPosition, pipe.transform.position);
            }
        }

        return 0;
    }
}
