using System;
using System.Collections.Generic;
using System.Linq;
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
            set;
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

        #endregion

        public Population(Piperenderer piperendererInstance, int populationSize, GameObject birdPrefab)
        {
            PopulationMembers = new Dictionary<GameObject, Bird>(PopulationSize);
            this.piperendererInstance = piperendererInstance;
            PopulationSize = populationSize;
            Bird = birdPrefab;
        }
        
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
            DeadCounter = EventExecutors.BirdDeath(bird, piperendererInstance, DeadCounter);
            if (DeadCounter == PopulationSize)
            {
                AllDead?.Invoke();
            }
        }

        private List<Bird> calculateFittness()
        {
            List<Bird> parents = new List<Bird>();  
            
            Bird bestBird = (from bird in PopulationMembers orderby bird.Value.TravelledDistance
                descending select bird.Value).First();

            
            Bird secondBestBird = (from bird in PopulationMembers
                orderby bird.Value.TravelledDistance
                    descending
                select bird.Value).Skip(1).First();
            
            parents.Add(bestBird);
            parents.Add(secondBestBird);
            return parents;
        }

        public void crossover()
        {
            List<Bird> parents = calculateFittness();
            Debug.Log(parents[0].Brain.HiddenLayers[0].Weights.ToString());
            Debug.Log(parents[0].Brain.HiddenLayers[1].Weights.ToString());
            Debug.Log(parents[1].Brain.HiddenLayers[0].Weights.ToString());
            Debug.Log(parents[1].Brain.HiddenLayers[1].Weights.ToString());
        }
    }
}