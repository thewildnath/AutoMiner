using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	public Transform Player;
	private GameObject World;
	public int[,] world = new int[50, 10000];

	public GUISkin guiSking;

	//Parameters
	public int money=0, points=0, life=100, fuel=100;

	public float col;

	void Start () 
	{
		QualitySettings.vSyncCount = 1;

		Set_World ("World_Normal");

		InvokeRepeating ("ConsumeFuel", 1, 2);
	}

	void ConsumeFuel ()
	{
		if (Player.transform.position.y < 0) 
		{
			fuel -= 1;
		}else
		{
			fuel = 100;
		}
	}

	void OnGUI ()
	{
		GUI.skin = guiSking;

		GUI.Label(new Rect(25, 80, 1000, 25), money + " $");

		GUI.Label(new Rect(25, 110, 1000, 25), points+ " PT");

		GUI.Label(new Rect(25, 20, 80, 100), "FUEL " + fuel);

		GUI.Label(new Rect(100, 20, 80, 100), "LIFE " + life);
	}

	void Set_World (string WorldName)
	{
		World = GameObject.Find (WorldName);
		World.GetComponent<WorldGenerator> ().IsCurrent = true;
		
		world = World.GetComponent<WorldGenerator> ().world;
		GameObject.Find ("Player").GetComponent<Player_Move> ().world = world;
	}

	public void DeleteTile (int x,int y)
	{
		if(x>=0 && x<50 && y>=0 && y<10000)
		{
			points+=world[x, y];

			World.GetComponent<WorldGenerator> ().ChangeTile (x, y, 0);
		}
	}
}
