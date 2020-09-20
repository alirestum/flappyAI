using GeneticAlgorithm;
using UnityEngine;
using UnityEngine.Serialization;

public class Level : MonoBehaviour
{

    public GameObject pipeRenderer;
    private Population populationInstance;
    [FormerlySerializedAs("BirdPrefab")] public GameObject birdPrefab;

    public int PopulationSize;

    private void Awake()
    {
        pipeRenderer = Instantiate(pipeRenderer, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        pipeRenderer.transform.parent = this.transform;
        populationInstance = new Population(pipeRenderer.GetComponent<Piperenderer>(), PopulationSize, birdPrefab);
        populationInstance.AllDead += AllDead;
        populationInstance.NewGenerationIsReady += NextGeneration;
    }

    private void Start()
    {
        populationInstance.InitializePopulation();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log((100 - _populationInstance.DeadCounter).ToString());
    }

    private void restartGame()
    {
            pipeRenderer.GetComponent<Piperenderer>().resetPiperenderer();
    }

    private void AllDead()
    {
        pipeRenderer.GetComponent<Piperenderer>().Speed = 0;
        populationInstance.DoGeneration();
    }

    private void NextGeneration()
    {
        restartGame();
    }
}