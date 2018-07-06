using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesLife : MonoBehaviour
{

    private float particleLifeTime = 0.3f, startTime;
    private Vector3 from, to;
    private Vector3 direction;

    private Vector3 p1, p2;

    // Use this for initialization
    void Awake()
    {
        
    }

    void Start()
    {
        startTime = Time.time;
        from = this.GetComponent<Transform>().position;

        float rand1 = Random.Range(0.15f, 1f);
        float rand2 = Random.Range(-1f, 1f);
        float rand3 = Random.Range(-1f, 1f);

        to = from + rand1 * direction + rand2 * Vector3.up + rand3 * Vector3.Cross(direction, Vector3.up);

        p1 = Random.insideUnitSphere / 4 + from;
        p2 = Random.insideUnitSphere / 2 + from;
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Transform>().position = Bezier(p1, p2, (Time.time - startTime) / particleLifeTime);
        ScaleDown();
    }

    public void SetLifeTime(float time)
    {
        particleLifeTime = time;
    }

    private void ScaleDown()
    {
        if (Time.time - startTime > particleLifeTime)
            Destroy(gameObject);
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    public void SetStartPosition(Vector3 startPosition)
    {
        this.from = startPosition;
    }

    private Vector3 Bezier(Vector3 p1, Vector3 p2, float alpha)
    {
        Vector3 _f1 = Vector3.Lerp(from, p1, alpha);
        Vector3 _12 = Vector3.Lerp(p1, p2, alpha);
        Vector3 _2t = Vector3.Lerp(p2, to, alpha);

        Vector3 _v1 = Vector3.Lerp(_f1, _12, alpha);
        Vector3 _v2 = Vector3.Lerp(_12, _2t, alpha);

        Vector3 position = Vector3.Lerp(_v1, _v2, alpha);

        return position;
    }
}
