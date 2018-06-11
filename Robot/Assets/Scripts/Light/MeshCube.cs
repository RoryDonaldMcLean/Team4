using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCube : BaseCustomMesh 
{
    //the configuration specs for a cube shape of dimensions of 1 for width, height and depth.
	protected override void CustomMeshDetails()
	{
		//amount of slices
		int width = 1;
		int height = 1;

		int numberOfFaces = 6;
		
		int meshSize = height * width;
		int amountOfPointsPerQuad = 4;
		int amountOfTrianglesPerQuad = 6;

		verts = new Vector3[meshSize*amountOfPointsPerQuad*numberOfFaces];
		norms = new Vector3[meshSize*amountOfPointsPerQuad*numberOfFaces];
		uvs = new Vector2[meshSize*amountOfPointsPerQuad*numberOfFaces];
		triangles = new int[meshSize*amountOfTrianglesPerQuad*numberOfFaces];

		Vector3[] cubeRefPoints = new Vector3[8*meshSize]; 
		
		//bottom
		cubeRefPoints [0] = new Vector3 (-0.5f, -0.5f, -0.5f);
		cubeRefPoints [1] = new Vector3 (0.5f, -0.5f, -0.5f);
		cubeRefPoints [2] = new Vector3 (-0.5f, -0.5f, 0.5f);
		cubeRefPoints [3] = new Vector3 (0.5f, -0.5f, 0.5f);

		//top
		cubeRefPoints [4] = new Vector3 (-0.5f, 0.5f, -0.5f);
		cubeRefPoints [5] = new Vector3 (0.5f, 0.5f, -0.5f);
		cubeRefPoints [6] = new Vector3 (-0.5f, 0.5f, 0.5f);
		cubeRefPoints [7] = new Vector3 (0.5f, 0.5f, 0.5f);

		//front
		verts [0] = cubeRefPoints [0];
		verts [1] = cubeRefPoints [1];
		verts [2] = cubeRefPoints [4];
		verts [3] = cubeRefPoints [5];

		//front
		norms [0] = new Vector3 (0, 0, -1);
		norms [1] = new Vector3 (0, 0, -1);
		norms [2] = new Vector3 (0, 0, -1);
		norms [3] = new Vector3 (0, 0, -1);

		//right
		verts [4] = cubeRefPoints [1];
		verts [5] = cubeRefPoints [3];
		verts [6] = cubeRefPoints [5];
		verts [7] = cubeRefPoints [7];

		//right
		norms [4] = new Vector3 (1, 0, 0);
		norms [5] = new Vector3 (1, 0, 0);
		norms [6] = new Vector3 (1, 0, 0);
		norms [7] = new Vector3 (1, 0, 0);

		//back
		verts [8] = cubeRefPoints [3];
		verts [9] = cubeRefPoints [2];
		verts [10] = cubeRefPoints [7];
		verts [11] = cubeRefPoints [6];

		//back
		norms [8] = new Vector3 (0, 0, 1);
		norms [9] = new Vector3 (0, 0, 1);
		norms [10] = new Vector3 (0, 0, 1);
		norms [11] = new Vector3 (0, 0, 1);

		//left
		verts [12] = cubeRefPoints [2];
		verts [13] = cubeRefPoints [0];
		verts [14] = cubeRefPoints [6];
		verts [15] = cubeRefPoints [4];

		//left
		norms [12] = new Vector3 (-1, 0, 0);
		norms [13] = new Vector3 (-1, 0, 0);
		norms [14] = new Vector3 (-1, 0, 0);
		norms [15] = new Vector3 (-1, 0, 0);

		//top
		verts [16] = cubeRefPoints [4];
		verts [17] = cubeRefPoints [5];
		verts [18] = cubeRefPoints [6];
		verts [19] = cubeRefPoints [7];

		//top
		norms [16] = new Vector3 (0, 1, 0);
		norms [17] = new Vector3 (0, 1, 0);
		norms [18] = new Vector3 (0, 1, 0);
		norms [19] = new Vector3 (0, 1, 0);

		//bottom
		verts [20] = cubeRefPoints [1];
		verts [21] = cubeRefPoints [0];
		verts [22] = cubeRefPoints [3];
		verts [23] = cubeRefPoints [2];

		//bottom
		norms [20] = new Vector3 (0, -1, 0);
		norms [21] = new Vector3 (0, -1, 0);
		norms [22] = new Vector3 (0, -1, 0);
		norms [23] = new Vector3 (0, -1, 0);

		for(int i = 0; i < numberOfFaces; i++)
		{
			int triangleIndex = (i * amountOfTrianglesPerQuad);
			int quadIndex = (amountOfPointsPerQuad * i);
			
			float textureRange = (1 / (float)numberOfFaces);
			float textureLocation = (i * textureRange);
			float textureDestination = ((i+1) * textureRange);

			uvs [quadIndex] = new Vector2 (textureLocation, 0);
			uvs [1 + quadIndex] = new Vector2 (textureDestination, 0);
			uvs [2 + quadIndex] = new Vector2 (textureLocation, 1);
			uvs [3 + quadIndex] = new Vector2 (textureDestination, 1);

			triangles [triangleIndex] = quadIndex;
			triangles [1 + triangleIndex] = 2 + quadIndex;
			triangles [2 + triangleIndex] = 1 + quadIndex;

			triangles [3 + triangleIndex] = 2 + quadIndex;
			triangles [4 + triangleIndex] = 3 + quadIndex;
			triangles [5 + triangleIndex] = 1 + quadIndex;
		}		
	}
}
