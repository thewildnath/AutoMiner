using UnityEngine;
using System.Collections;

public class Player_Move : MonoBehaviour {
	
	//Misc
	private GameObject GM;
	private Main main;
	public int[,] world = new int[50, 10000];
	
	//Controls
	public GUIStyle arrowu, arrowd, arrowl, arrowr;
	public GUISkin guiSking;

	private bool move=false, up = false, down = false, left = false, right = false;
	public bool can_Move=true;
	private float playerX, playerY, dx, dy, lastY, deltaY;
	private int mX, mY;
	private float speed = 0f;
	Vector3 targetPos = new Vector3();
	
	//Collisions
	public GameObject[] colliders = new GameObject[8];
	private int[] dirX = new int[8];
	private int[] dirY = new int[8];
	private Vector2 collSpeed = new Vector2();

	void Start () 
	{
		GM = GameObject.Find ("_GM");
		main = GM.GetComponent<Main> ();
		
		SetDir ();
		SetColliders ();
	}
	
	void Update () 
	{
		//Info
		playerX = transform.position.x;
		playerY = transform.position.y;
		mX = (int)Mathf.Floor(playerX);
		mY = -(int)Mathf.Floor(playerY)-1;
		dx = playerX - (int)playerX;
		dy = playerY - (int)playerY;
		
		speed = transform.rigidbody2D.velocity.magnitude;
		
		UpdateColliders ();
		
		if (can_Move == true)
			GetInput ();
		
		if (move == true)
			Move (targetPos);
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		collSpeed = coll.relativeVelocity;

		collSpeed.y = Mathf.Abs (collSpeed.y);

		if(collSpeed.y > 13)
		{
			main.life -= (int)collSpeed.y;
		}
	}
	
	void Move(Vector3 targetPos)
	{
		float step = 3f * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
		
		if(Vector3.Distance(transform.position,targetPos)<0.1)
		{
			can_Move = true;
			move=false;
			transform.rigidbody2D.gravityScale = 1;
		}
	}
	
	void GoTo (Vector3 aux)
	{
		targetPos = aux;
		
		can_Move = false;
		move = true;
		transform.rigidbody2D.gravityScale = 0;
	}
	
	void GetInput ()
	{
		if (Input.GetKey (KeyCode.W) || up==true)
		{
			transform.rigidbody2D.AddForce (new Vector2 (0, 20));
		}
		if (Input.GetKey (KeyCode.A) || left==true)
		{
			transform.rigidbody2D.AddForce (new Vector2 (-10, 0));
			
			if(world[mX-1, mY]!=0 && world[mX, mY+1]!=0 && dx<=0.41 && dy<=-0.59)
			{
				main.DeleteTile(mX-1, mY);
				
				GoTo (new Vector3(mX-1+0.5f, playerY, 0));
			}
		}
		if (Input.GetKey (KeyCode.D) || right==true)
		{
			transform.rigidbody2D.AddForce (new Vector2 (10, 0));
			
			if(world[mX+1, mY]!=0 && world[mX, mY+1]!=0 && dx>=0.59 && dy<=-0.59)
			{
				main.DeleteTile(mX+1, mY);
				
				GoTo (new Vector3(mX+1+0.5f, playerY, 0));
			}
		}
		if (Input.GetKey (KeyCode.S) || down==true) 
		{
			//transform.rigidbody2D.AddForce (new Vector2 (0, -20));
			
			if(world[mX, mY+1]!=0 && (dy<=-0.59 || playerY>=0))
			{
				main.DeleteTile(mX, mY+1);
				
				GoTo (new Vector3(mX+0.5f, playerY-1, 0));
			}
		}
	}

	void OnGUI ()
	{
		if (GUI.RepeatButton (new Rect (Screen.width - 84, Screen.height - 158, 64, 64), "", arrowu))
			up = true;
		else
			up = false;
		
		if (GUI.RepeatButton (new Rect (Screen.width - 84, Screen.height - 84, 64, 64), "", arrowd))
			down = true;
		else
			down = false;
		
		if (GUI.RepeatButton (new Rect (20, Screen.height - 84, 64, 64), "", arrowl))
			left = true;
		else
			left = false;
		
		if (GUI.RepeatButton (new Rect (94, Screen.height - 84, 64, 64), "", arrowr))
			right = true;
		else
			right = false;
	}
	
	void UpdateColliders ()
	{
		int x, y;
		bool ok;
		for (int i=0; i<8; ++i)
		{
			ok = false;
			
			x = mX+dirX[i];
			y = mY+dirY[i];
			
			if(x>=0 && x<50 && y>=0 && y<10000)
				if(world[x, y] !=0)
					ok = true;
			
			if(ok == true)
				colliders[i].GetComponent<BoxCollider2D>().center= new Vector2((float)x+0.5f, (float)-y-1+0.5f);
			else
				colliders[i].GetComponent<BoxCollider2D>().center= new Vector2(-2, i);
		}
	}
	
	void SetColliders ()
	{
		for (int i=0; i<8; ++i)
		{
			colliders[i] = new GameObject();
			colliders[i].name = "Collider"+i;
			
			colliders[i].AddComponent("BoxCollider2D");
			colliders[i].GetComponent<BoxCollider2D>().size = new Vector2 (1, 1);
		}
	}
	
	void SetDir ()
	{
		dirX [0] = 1;
		dirX [1] = 0;
		dirX [2] = -1;
		dirX [3] = 0;
		dirX [4] = 1;
		dirX [5] = -1;
		dirX [6] = -1;
		dirX [7] = 1;
		
		dirY [0] = 0;
		dirY [1] = 1;
		dirY [2] = 0;
		dirY [3] = -1;
		dirY [4] = 1;
		dirY [5] = 1;
		dirY [6] = -1;
		dirY [7] = -1;
	}
}
