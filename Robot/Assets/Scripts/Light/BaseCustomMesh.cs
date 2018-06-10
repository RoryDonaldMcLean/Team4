using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

[ExecuteInEditMode]

public class BaseCustomMesh : MonoBehaviour 
{
	protected Mesh mesh;
	protected Vector3[] verts;
	protected Vector3[] norms;
	protected Vector2[] uvs;
	protected int[] triangles;

	public string meshName;

	// Use this for initialization
	void Start () 
	{
		DrawMesh();			
	}

	private void DrawMesh()
	{
		if (!mesh) 
		{
			mesh = new Mesh();
			GetComponent<MeshFilter>().mesh = mesh;
			mesh.name = meshName;
		}

		ConstructMesh();
	}

	private void ConstructMesh()
	{
		mesh.Clear();
		CustomMeshDetails();

		mesh.vertices = verts;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		mesh.normals = norms;

		mesh.RecalculateBounds();
	}

	protected virtual void CustomMeshDetails()
	{
		//inherit this func to the specfic custom mesh script 
		//with the verts/uvs/normals sets in this.  
	}
}