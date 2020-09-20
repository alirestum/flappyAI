using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Bolt;
using MathNet.Numerics;
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
                GameObject newBird = GameObject.Instantiate(Bird, new Vector3(-7.5f, 1.0f, 0.0f), Quaternion.identity);;
                Bird newBirdInstance = newBird.GetComponent<Bird>();
                newBirdInstance.onBecomeInvisible += BirdDeath;
                newBirdInstance.onHit += BirdDeath;
                newBirdInstance.PiperendererInstance = piperendererInstance;
                
                //HiddenLayer#1
                newBirdInstance.Brain.HiddenLayers[0].Weights = (parents[0].Brain.HiddenLayers[0].Weights
                    + parents[1].Brain.HiddenLayers[0].Weights) / 2;
                newBirdInstance.Brain.HiddenLayers[0].Biases = parents[random.Next(0, 1)].Brain.HiddenLayers[0].Biases;
                
                //HiddenLayer#2
                newBirdInstance.Brain.HiddenLayers[1].Weights = (parents[0].Brain.HiddenLayers[1].Weights 
                    + parents[1].Brain.HiddenLayers[1].Weights) / 2;
                newBirdInstance.Brain.HiddenLayers[1].Biases = parents[random.Next(0, 1)].Brain.HiddenLayers[1].Biases;

                //OutpuLayer
                newBirdInstance.Brain.OutputLayer.Weights =
                    (parents[0].Brain.OutputLayer.Weights + parents[1].Brain.OutputLayer.Weights) / 2;
                newBirdInstance.Brain.OutputLayer.Biases = parents[random.Next(0, 1)].Brain.OutputLayer.Biases;
                
                PopulationMembers.Add(newBird, newBirdInstance);
            }
            
            /*for (int i = 0; i < PopulationSize; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < parents[0].Brain.HiddenLayers[j].Weights.RowCount; k++)
                    {
                        for (int l = 0; l < parents[0].Brain.HiddenLayers[j].Weights.ColumnCount; l++)
                        {
                            if (random.Next(0,2) == 0)
                            {
                                double temp = parents[0].Brain.HiddenLayers[j].Weights[k,l];
                                parents[0].Brain.HiddenLayers[j].Weights[k, l] =
                                    parents[1].Brain.HiddenLayers[j].Weights[k, l];
                                parents[0].Brain.HiddenLayers[j].Weights[k, l] = temp;
                            }
                        }
                    }
                  
                }
                
                GameObject newBird = GameObject.Instantiate(Bird, new Vector3(-7.5f, 1.0f, 0.0f), Quaternion.identity);;
                Bird newBirdInstance = newBird.GetComponent<Bird>();
                newBirdInstance.onBecomeInvisible += BirdDeath;
                newBirdInstance.onHit += BirdDeath;
                newBirdInstance.PiperendererInstance = piperendererInstance;
                if (i.IsEven())
                {
                    //Debug.Log(parents[0].Brain.HiddenLayers[0].Weights.ToArray());
                    newBirdInstance.Brain.HiddenLayers[0].Weights =
                        DenseMatrix.OfArray(parents[0].Brain.HiddenLayers[0].Weights.ToArray());
                    newBirdInstance.Brain.HiddenLayers[1].Weights = DenseMatrix.OfArray(parents[0].Brain.HiddenLayers[1].Weights.ToArray());
                }
                else
                {
                    newBirdInstance.Brain.HiddenLayers[0].Weights = DenseMatrix.OfArray(parents[1].Brain.HiddenLayers[0].Weights.ToArray());
                    newBirdInstance.Brain.HiddenLayers[1].Weights = DenseMatrix.OfArray(parents[1].Brain.HiddenLayers[1].Weights.ToArray());
                }
                PopulationMembers.Add(newBird, newBirdInstance);
                
                /*GameObject newBird = GameObject.Instantiate(Bird, new Vector3(-7.5f, 1.0f, 0.0f), Quaternion.identity);;
               Bird newBirdInstance = newBird.GetComponent<Bird>();
               newBirdInstance.onBecomeInvisible += BirdDeath;
               newBirdInstance.onHit += BirdDeath;
               newBirdInstance.PiperendererInstance = piperendererInstance;
               if (i.IsEven())
               {
                   newBirdInstance.Brain = parents[0].GetComponent<Bird>().Brain;
               }
               else
               {
                   newBirdInstance.Brain = parents[1].GetComponent<Bird>().Brain;
               }
               PopulationMembers.Add(newBird, newBirdInstance);#1#
            }*/
        }

        public void DoGeneration()
        {
            piperendererInstance.resetPiperenderer();
            crossover();
            DeadCounter = 0;
            NewGenerationIsReady?.Invoke();
        }
        #endregion
    }
}