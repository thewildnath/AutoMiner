using UnityEngine;
using System.Collections;

public class Player_Draw : MonoBehaviour {

	private float playerX, playerY;

	//Mesh
	private Mesh mesh;
	private float ut = 0.125f;
	private Vector2 texture = new Vector2 (1, 4);
	private Vector3[] newVertices = new Vector3[4];
	private int[] newTriangles = new int[6];
	private Vector2[] newUV = new Vector2[4];
	
	void Start () 
	{
		mesh = transform.GetComponent<MeshFilter> ().mesh;

		MakeMesh ();
		DrawMesh ();
	}

	void Update () 
	{
		playerX = transform.position.x;
		playerY = transform.position.y;
	}
	
	void MakeMesh ()
	{
		newVertices [0] = new Vector3 (playerX-0.5f, playerY+0.55f, 0);
		newVertices [1] = new Vector3 (playerX+0.5f, playerY+0.55f, 0);
		newVertices [2] = new Vector3 (playerX+0.5f, playerY-0.45f, 0);
		newVertices [3] = new Vector3 (playerX-0.5f, playerY-0.45f, 0);
		
		newTriangles [0] = 0;
		newTriangles [1] = 1;
		newTriangles [2] = 3;
		newTriangles [3] = 1;
		newTriangles [4] = 2;
		newTriangles [5] = 3;
		
		newUV [0] = new Vector2 (ut * texture.x, ut * texture.y + ut);
		newUV [1] = new Vector2 (ut * texture.x + ut, ut * texture.y + ut);
		newUV [2] = new Vector2 (ut * texture.x + ut, ut * texture.y);
		newUV [3] = new Vector2 (ut * texture.x, ut * texture.y);
	}
	
	void DrawMesh ()
	{
		mesh.Clear ();
		mesh.vertices = newVertices;
		mesh.triangles = newTriangles;
		mesh.uv = newUV;
		mesh.Optimize ();
		mesh.RecalculateNormals ();
	}
}
