using System.Collections.Generic;
using UnityEngine;

namespace GeneticAlgorithm
{
    public class EventExecutors
    {
        public static int BirdDeath(Bird bird, Piperenderer piperenderer, int deadcounter)
        {
            bird._rigidbody2d.constraints = RigidbodyConstraints2D.FreezeAll;
           // GameObject.Destroy(bird.gameObject);
            bird.TravelledDistance = piperenderer.TravelDistance;
            bird.Dead = true;
            deadcounter++;
            return deadcounter;
        }

        public static void RemovePipe(Pipe pipe, List<GameObject> pipes)
        {
            GameObject.Destroy(pipe.gameObject);
            pipes.RemoveAt(0);
        }
    }
    

}