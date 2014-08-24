using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour {

	public Transform Player;
	public bool IsCurrent = false;

	private List<Vector3> newVertices = new List<Vector3>();
	private List<int> newTriangles = new List<int>();
	private List<Vector2> newUV = new List<Vector2>();
	private int tiles = 0;

	private GameObject[] chunks = new GameObject[40];
	private bool[] generated = new bool[40];

	private float ut = 0.125f;
	private Vector2[] terrain = new Vector2[10];

	public int[,] world = new int[50, 10000];
	private Vector2 worldBegin = new Vector2 (0, 0);
	private Vector2 worldSize = new Vector2 (50, 10000);
	private Vector2 chunkSize = new Vector2 (50, 250);
	


	// Use this for initialization
	void Start () 
	{
		for (int i=0; i<39; ++i) 
		{
			chunks[i] = new GameObject();
			chunks[i].name="Chunk Normal"+i;

			chunks[i].AddComponent("MeshFilter");
			chunks[i].AddComponent ("MeshRenderer");

			chunks[i].GetComponent<MeshRenderer> ().material = this.GetComponent<MeshRenderer> ().material;
		}

		SetTextures ();

		GenWorld ();

		InvokeRepeating ("LoadChunks", 0, 1);
	}

	public void ChangeTile (int x,int y,int type)
	{
		world [x, y] = 0;

		/*for (int i=0; i<39; ++i)
		{
			if (Mathf.Abs ((i * -250) - 125 - Player.position.y) < 300)
			{
				GenChunk(i);

				break;
			}
		}*/

		int i = (int)Player.position.y / -250;

		GenChunk (i);
	}

	void LoadChunks ()
	{
		if (IsCurrent == true)
		{
			for(int i=0; i<39; ++i)
			{
				if(Mathf.Abs((i*-250)-125 - Player.position.y)<300)
				{
					if(generated[i] == false)
					{
						generated[i] = true;
						GenChunk (i);
					}
				}else
				{
					if(generated[i] == true)
					{
						generated[i] = false;
						//Destroy(chunks[i]);
						chunks[i].GetComponent<MeshFilter>().mesh.Clear ();
					}
				}
			}
		}
	}

	void GenChunk (int number)
	{
		BuildMesh (number*(-250));
		UpdateMesh(chunks[number].GetComponent<MeshFilter>().mesh);
	}

	void GenWorld ()
	{
		for (int i=0; i<worldSize.x; ++i) 
		{
			world[i,0]=1;
			world[i,1]=2;

			for(int j=2; j<worldSize.y; ++j)
			{
				int rand = Random.Range(0, 100);
				
				if(rand<6)
				{
					world[i,j]=0;
				}else if(rand<70)
				{
					world[i,j]=2;
				}else if(rand<78)
				{
					world[i,j]=3;
				}else if(rand<100)
				{
					world[i,j]=4;
				}
			}
		}
	}

	void GenTile (int x,int y, Vector2 texture)
	{
		if (texture != terrain [0]) 
		{
			newVertices.Add (new Vector3 (x, y, 0));
			newVertices.Add (new Vector3 (x + 1, y, 0));
			newVertices.Add (new Vector3 (x + 1, y - 1, 0));
			newVertices.Add (new Vector3 (x, y - 1, 0));
		
			newTriangles.Add (tiles * 4);
			newTriangles.Add (tiles * 4 + 1);
			newTriangles.Add (tiles * 4 + 3);
			newTriangles.Add (tiles * 4 + 1);
			newTriangles.Add (tiles * 4 + 2);
			newTriangles.Add (tiles * 4 + 3);
		
			newUV.Add (new Vector2 (ut * texture.x, ut * texture.y + ut-0.001f));
			newUV.Add (new Vector2 (ut * texture.x + ut-0.001f, ut * texture.y + ut-0.001f));
			newUV.Add (new Vector2 (ut * texture.x + ut-0.001f, ut * texture.y));
			newUV.Add (new Vector2 (ut * texture.x, ut * texture.y));

			++tiles;
		}
	}

	void BuildMesh (int StartY)
	{
		for (int i=0; i<chunkSize.x; ++i)
		{
			for (int j=0; j<chunkSize.y; ++j)
			{
				GenTile ((int)worldBegin.x + i, (int)worldBegin.y - j + StartY,terrain[world[i,j - StartY]]);
			}
		}
	}

	void UpdateMesh (Mesh mesh)
	{
		mesh.Clear ();
		mesh.vertices = newVertices.ToArray ();
		mesh.triangles = newTriangles.ToArray ();
		mesh.uv = newUV.ToArray ();
		mesh.Optimize ();
		mesh.RecalculateNormals ();

		tiles = 0;
		newVertices.Clear ();
		newTriangles.Clear ();
		newUV.Clear ();
	}

	void SetTextures ()
	{	
		terrain [1] = new Vector2 (0, 6);//Grass
		terrain [2] = new Vector2 (0, 7);//Dirt
		terrain [3] = new Vector2 (1, 7);//Stone
		terrain [4] = new Vector2 (3, 7);//Copper
		terrain [5] = new Vector2 (2, 7);//Iron
		terrain [6] = new Vector2 (4, 7);//Silver
		terrain [7] = new Vector2 (5, 7);//Gold

	}

	int Noise (int x,int y, float scale, float mag, float exp)
	{
		return (int)(Mathf.Pow((Mathf.PerlinNoise(x/scale,y/scale)*mag),(exp)));
	}
}
