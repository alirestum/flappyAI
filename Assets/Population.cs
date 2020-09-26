using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using UnityEngine;

namespace GeneticAlgorithm
{
    public class Population
    {

        #region Properties and member attributes
        private int PopulationSize
        {
            get;
            set;
        }

        public Dictionary<GameObject, Bird> PopulationMembers
        {
            get;
        }

        public GameObject Bird
        {
            get;
            set;
        }

        public int DeadCounter
        {
            get;
            set;
        }

        private Piperenderer piperendererInstance;
        public event Action AllDead;

        public event Action NewGenerationIsReady;

        #endregion

        #region Constructor
        public Population(Piperenderer piperendererInstance, int populationSize, GameObject birdPrefab)
        {
            PopulationMembers = new Dictionary<GameObject, Bird>(PopulationSize);
            this.piperendererInstance = piperendererInstance;
            PopulationSize = populationSize;
            Bird = birdPrefab;
        }
        #endregion

        #region Init
        public void InitializePopulation()
        {
            for (int i = 0; i < PopulationSize; i++)
            {
                GameObject bird = GameObject.Instantiate(Bird, new Vector3(-7.5f, 1.0f, 0.0f), Quaternion.identity);
                Bird birdInstance = bird.GetComponent<Bird>();
                birdInstance.onBecomeInvisible += BirdDeath;
                birdInstance.onHit += BirdDeath;
                birdInstance.PiperendererInstance = piperendererInstance;
                PopulationMembers.Add(bird, bird.GetComponent<Bird>());
            }
        }

        private void BirdDeath(Bird bird)
        {
            bird.Rigidbody2d.constraints = RigidbodyConstraints2D.FreezeAll;
            GameObject.Destroy(bird.gameObject);
            bird.Dead = true;
            DeadCounter++;
            if (DeadCounter == PopulationSize)
            {
                AllDead?.Invoke();
            }
        }
        #endregion

        #region NextGeneration
        private List<Bird> selectParents()
        {
            List<Bird> parents = new List<Bird>(3);

            for (int i = 0; i < 3; i++)
            {
                Bird parent = (from bird in PopulationMembers
                    orderby bird.Value.Fittness
                        descending
                    select bird.Value).Skip(i).First();
                
                parents.Add(parent);
            }
            return parents;
        }

        private void crossover()
        {
            List<Bird> parents = selectParents();
            System.Random random = new System.Random();
            PopulationMembers.Clear();

            for (int i = 0; i < PopulationSize; i++)
            {
                GameObject newChild = GameObject.Instantiate(Bird, new Vector3(-7.5f, 1.0f, 0.0f), Quaternion.identity);
                Bird newChildInstance = newChild.GetComponent<Bird>();
                newChildInstance.onBecomeInvisible += BirdDeath;
                newChildInstance.onHit += BirdDeath;
                newChildInstance.PiperendererInstance = piperendererInstance;

                Matrix<double> crossedHiddenLayer1 = parents[0].Brain.HiddenLayers[0].Weights +
                                                     random.NextDouble() * (parents[1].Brain.HiddenLayers[0].Weights -
                                                      parents[0].Brain.HiddenLayers[0].Weights);
                
                Matrix<double> crossedHiddenLayer2 = parents[0].Brain.HiddenLayers[1].Weights +
                                                     random.NextDouble() * (parents[1].Brain.HiddenLayers[1].Weights -
                                                                            parents[0].Brain.HiddenLayers[1].Weights);

                Matrix<double> crossedOutputLayer = parents[0].Brain.OutputLayer.Weights +
                                                    random.NextDouble() * (parents[1].Brain.OutputLayer.Weights -
                                                                           parents[0].Brain.OutputLayer.Weights);

                newChildInstance.Brain.HiddenLayers[0].Weights = crossedHiddenLayer1;
                newChildInstance.Brain.HiddenLayers[1].Weights = crossedHiddenLayer2;
                newChildInstance.Brain.OutputLayer.Weights = crossedOutputLayer; 
                PopulationMembers.Add(newChild, newChildInstance);
            }
        }

        private void mutation()
        {
            System.Random random = new System.Random();
            foreach (Bird bird in PopulationMembers.Values)
            {
                bird.Brain.HiddenLayers[0].Weights.Multiply(random.NextDouble());
                bird.Brain.HiddenLayers[1].Weights.Multiply(random.NextDouble());
                bird.Brain.OutputLayer.Weights.Multiply(random.NextDouble());
            }
        }

        public void DoGeneration()
        {
            piperendererInstance.resetPiperenderer();
            crossover();
            mutation();
            DeadCounter = 0;
            NewGenerationIsReady?.Invoke();
        }
        #endregion
    }
}