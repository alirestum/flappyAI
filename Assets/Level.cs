using System;
using GeneticAlgorithm;
using UnityEngine;

public class Level : MonoBehaviour
{

    public GameObject pipeRenderer;
    private Population _populationInstance;
    public GameObject BirdPrefab;
    private bool _paused;

    public int PopulationSize;

    private void Awake()
    {
        _paused = false;
        pipeRenderer = Instantiate(pipeRenderer, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        pipeRenderer.transform.parent = this.transform;
        _populationInstance = new Population(pipeRenderer.GetComponent<Piperenderer>(), PopulationSize, BirdPrefab);
        _populationInstance.AllDead += AllDead;
    }

    private void Start()
    {
        _populationInstance.InitializePopulation();
    }

    // Update is called once per frame
    void Update()
    {
        restartGame();
        pauseGame();
    }

    private void restartGame()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            pipeRenderer.GetComponent<Piperenderer>().Speed = 0.1f;
            pipeRenderer.GetComponent<Piperenderer>().resetPiperenderer();
        }
    }

    private void pauseGame()
    {
        if (Input.GetKeyDown(KeyCode.P) && !_paused)
        {
            pipeRenderer.GetComponent<Piperenderer>().Speed = 0;
            _paused = true;
        } else if (Input.GetKeyDown(KeyCode.P) && _paused)
        {
            pipeRenderer.GetComponent<Piperenderer>().Speed = 0.1f;
            _paused = false;
        }
    }
    
    private void AllDead()
    {
        pipeRenderer.GetComponent<Piperenderer>().Speed = 0;
        _populationInstance.crossover();
    }
}
