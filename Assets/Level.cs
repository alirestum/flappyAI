using System;
using UnityEngine;

public class Level : MonoBehaviour
{

    public GameObject pipeRenderer;
    public GameObject bird;

    private bool _paused;

    private void Awake()
    {
        _paused = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        pipeRenderer = Instantiate(pipeRenderer, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        pipeRenderer.transform.parent = this.transform;
        bird = Instantiate(bird, new Vector3(-7.5f, 1.0f, 0.0f), Quaternion.identity);
        bird.transform.parent = this.transform;
        bird.GetComponent<Bird>().onHit += onBirdCollision;
        bird.GetComponent<Bird>().onBecomeInvisible += onBirdDeath;
        bird.GetComponent<Bird>().PiperendererInstance = pipeRenderer.GetComponent<Piperenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        restartGame();
        pauseGame();
    }

    private void onBirdCollision(Collision2D collision, Bird collidedBird)
    {
        if (bird.GetComponent<Bird>().Dead)
        {
            pipeRenderer.GetComponent<Piperenderer>().Speed = 0;
        }
        collidedBird.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void onBirdDeath(Bird deadBird)
    {
        if (bird.GetComponent<Bird>().Dead)
        {
            pipeRenderer.GetComponent<Piperenderer>().Speed = 0;
        }
        deadBird.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void restartGame()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            bird.GetComponent<Bird>().resetBird();
            pipeRenderer.GetComponent<Piperenderer>().Speed = 0.1f;
            pipeRenderer.GetComponent<Piperenderer>().resetPiperenderer();
        }
    }

    private void pauseGame()
    {
        if (Input.GetKeyDown(KeyCode.P) && !_paused)
        {
            bird.GetComponent<Bird>()._rigidbody2d.constraints = RigidbodyConstraints2D.FreezeAll;
            pipeRenderer.GetComponent<Piperenderer>().Speed = 0;
            _paused = true;
        } else if (Input.GetKeyDown(KeyCode.P) && _paused)
        {
            pipeRenderer.GetComponent<Piperenderer>().Speed = 0.1f;
            bird.GetComponent<Bird>()._rigidbody2d.constraints = RigidbodyConstraints2D.None;
            bird.GetComponent<Bird>()._rigidbody2d.constraints = RigidbodyConstraints2D.FreezePositionX;
            bird.GetComponent<Bird>()._rigidbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            _paused = false;
        }
    }
}
