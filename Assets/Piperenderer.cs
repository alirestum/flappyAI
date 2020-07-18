using System;
using System.Collections.Generic;
using UnityEngine;

public class Piperenderer : MonoBehaviour
{
    private List<GameObject> pipes = new List<GameObject>();
    public GameObject pipePrefab;
    public float speed = 0.1f;
    private System.Random random = new System.Random();
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
   
    // Start is called before the first frame update
    void Start()
    {

        generateInitialPipes();

    }

    // Update is called once per frame
    void Update()
    {
       
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
            currentPos.x += 5.0f;
            currentPos.y = generateYCoord();
            GameObject newPipe = Instantiate(pipePrefab, currentPos, Quaternion.identity);
            //newPipe.transform.parent = gameObject.transform;
            newPipe.GetComponent<Pipe>().onBecameInvisible += removePipe;
            newPipe.GetComponent<Pipe>().speed = speed;
            pipes.Add(newPipe);
        }
    }

    void removePipe()
    {
        
        Destroy(pipes[0].gameObject);
        pipes.RemoveAt(0);
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
        float spacing = 5;
        for (int i = 0; i < 6; i++)
        {
            Vector3 pos = new Vector3(spacing * i, generateYCoord(), 0);
            GameObject newPipe = Instantiate(pipePrefab, pos, Quaternion.identity);
            newPipe.transform.parent = gameObject.transform;
            newPipe.GetComponent<Pipe>().onBecameInvisible += removePipe;
            newPipe.GetComponent<Pipe>().speed = speed;
            pipes.Add(newPipe);
        }
    }
}
