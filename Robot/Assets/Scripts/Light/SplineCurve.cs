using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineCurve : MonoBehaviour
{
    public List<Vector3> controlPoints = new List<Vector3>();
    public Color color = Color.white;
    public float width = 0.2f;
    public int numberOfPoints = 20;
    LineRenderer lineRenderer;

    //upon loading this script, immediately grab the required components needed to draw the line. 
    void Awake()
    {
        lineRenderer = this.gameObject.AddComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.material = new Material(Shader.Find("Particles/Alpha Blended"));
        lineRenderer.name = this.name;
        lineRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
    }

    //setup function used to create a spline curve that is straight, of size length, which ranges from 1 to 10.
    public void StraightLine(int length)
    {
        Vector3 startPoint = new Vector3(0, 0, 0);
        Vector3 midPoint = new Vector3(0, 0, 1 * length);
        Vector3 endPoint = new Vector3(0, 0, 2 * length);

        controlPoints.Add(startPoint);
        controlPoints.Add(startPoint);
        controlPoints.Add(midPoint);
        controlPoints.Add(endPoint);
        controlPoints.Add(endPoint);
    }

    //setup function used to create a spline curve that is custom, of size length, which ranges from 1 to 10.
    public void CustomLine(int length, Vector3 startPoint, List<Vector3> midPoints, Vector3 endPoint)
    {
        controlPoints.Add(startPoint);
        controlPoints.Add(startPoint);

        foreach(Vector3 point in midPoints)
        {
            controlPoints.Add(point);
        }

        controlPoints.Add(endPoint);
        controlPoints.Add(endPoint);
    }

    //draws the curve and resets the local position of the object to zero, in order to allow for easy placement and manipulation 
    void Start()
    {
       DrawLine();
    }

    public void Initialise()
    {
        CalculateColliders();

        Vector3 globalParentScale = this.transform.parent.lossyScale;
        if(globalParentScale != Vector3.one) this.transform.localScale = new Vector3(1/ globalParentScale.x, 1 / globalParentScale.y, 1 / globalParentScale.z);

        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

        Vector3 temp = this.transform.position;
        //temp.y += 3.49f;
        this.transform.position = temp;
        //this.transform.GetChild(0).rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

    private void CreateCollider(Vector3 location)
    {
        //creates an endpoint collider, to be used for collision detection with the beam.
        GameObject colliderObject = Instantiate(Resources.Load("Prefabs/Light/LineCollider")) as GameObject;
        colliderObject.name = "LineColliderObject";
        colliderObject.transform.SetParent(lineRenderer.transform);
        colliderObject.transform.localPosition = location;
        //colliderObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    private void CalculateColliders()
    {
        Vector3 StartPosition = controlPoints[0];
        Vector3 EndPosition = controlPoints[controlPoints.Count - 1];

        CreateCollider(EndPosition);

        CapsuleCollider capsule = this.gameObject.AddComponent<CapsuleCollider>();
        capsule.radius = 0.2f;
        capsule.center = Vector3.zero;
        capsule.direction = 2;
        capsule.transform.position = StartPosition + ((EndPosition - StartPosition) / 2);
        capsule.transform.LookAt(EndPosition);
        capsule.height = (EndPosition - StartPosition).magnitude;
        capsule.center = capsule.transform.position;
        capsule.isTrigger = true;
    }

    private void DrawLine()
    {
        if (null == lineRenderer || controlPoints == null || controlPoints.Count < 3)
        {
            return; // not enough points specified
        }

        //setting up colour and width for the line renderer
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

        if (numberOfPoints < 2)
        {
            numberOfPoints = 2;
        }
        lineRenderer.positionCount = numberOfPoints * (controlPoints.Count - 2);

        Vector3 p0, p1, p2;
        for (int j = 0; j < controlPoints.Count - 2; j++)
        {
            //determine control points of segment
            p0 = 0.5f * (controlPoints[j] + controlPoints[j + 1]);
            p1 = controlPoints[j + 1];
            p2 = 0.5f * (controlPoints[j + 1] + controlPoints[j + 2]);

            //set points of quadratic Bezier curve
            Vector3 position;
            float t;
            float pointStep = 1.0f / numberOfPoints;
            if (j == controlPoints.Count - 3)
            {
                pointStep = 1.0f / (numberOfPoints - 1.0f);
                //last point of last segment should reach p2
            }
            Vector3 prevPosition = Vector3.zero;
            for (int i = 0; i < numberOfPoints; i++)
            {
                t = i * pointStep;
                position = (1.0f - t) * (1.0f - t) * p0
                + 2.0f * (1.0f - t) * t * p1 + t * t * p2;
                lineRenderer.SetPosition(i + j * numberOfPoints, position);
            }
        }
    }
}
