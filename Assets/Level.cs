using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{

    public GameObject pipeRenderer;
    public GameObject bird;

    // Start is called before the first frame update
    void Start()
    {
        pipeRenderer = Instantiate(pipeRenderer, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        pipeRenderer.transform.parent = this.transform;
        bird = Instantiate(bird, new Vector3(-7.5f, 1.0f, 0.0f), Quaternion.identity);
        bird.transform.parent = this.transform;
        bird.GetComponent<Bird>().onHit += onBirdCollision;
    }

    // Update is called once per frame
    void Update()
    {
        restartGame();
    }

    private void onBirdCollision(Collision2D collision)
    {
        pipeRenderer.GetComponent<Piperenderer>().Speed = 0;
        bird.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void restartGame()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            bird.GetComponent<Bird>().resetBird();
            pipeRenderer.GetComponent<Piperenderer>().speed = 0.1f;
            pipeRenderer.GetComponent<Piperenderer>().resetPiperenderer();
        }
    }
}
