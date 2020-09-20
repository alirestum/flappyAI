using System;
using UnityEngine;

public delegate void onPipeBecomeInvisible(Pipe pipe);
public class Pipe : MonoBehaviour
{

    public event onPipeBecomeInvisible onBecameInvisible;
    public Piperenderer PiperendererInstance
    {
        get;
        set;
    }

    public GameObject Top
    {
        get;
        private set;
    }

    public GameObject Bottom
    {
        get;
        private set;
    }

    public Pipe Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Top = transform.Find("top").gameObject;
        Bottom = transform.Find("bottom").gameObject;
        Instance = GetComponent<Pipe>();
    }

    void Update()
    {
        Vector3 currentPos = gameObject.transform.position;
        currentPos += new Vector3(-PiperendererInstance.Speed, 0.0f, 0.0f);
        gameObject.transform.position = currentPos;
    }

    private void OnBecameInvisible()
    {
        onBecameInvisible?.Invoke(this);
    }

}
