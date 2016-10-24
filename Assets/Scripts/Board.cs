using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour {

	[Serializable]
	public class Count
	{
		public int min;
		public int max;

		public Count (int mi, int mx)
		{
			min = mi;
			max = mx;
		}
	}

	public int columns = 160;
	public int rows = 90;

	public float worldToPixels = ((Screen.height / 2.0f) / Camera.main.orthographicSize);

	public GameObject aliveTile;
	public GameObject deadTile;

	private Transform boardHolder;
	private List<Vector3> gridPositions = new List<Vector3>();

	void initialize_list()
	{
		gridPositions.Clear();

		for(int x = 0; x < columns; x++)
		{
			for(int y = 0; y < rows; y++)
			{
				gridPositions.Add(new Vector3(x, y, 0f));
			}
		}
	}

	void board_setup()
	{
		boardHolder = new GameObject("Board").transform;

		// Should initialize the board to random alive or dead things
		//		for now.
		for(int x = 0; x < columns; x++)
		{
			for(int y = 0; y < rows; y++)
			{
				GameObject handle = deadTile;

				GameObject ins = Instantiate(handle, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
				ins.transform.SetParent(boardHolder);
			}	
		}
	}

	Vector3 random_position()
	{
		int ri = Random.Range(0, gridPositions.Count);
		Vector3 rpos = gridPositions[ri];
		gridPositions.RemoveAt(ri);
		return rpos;
	}

	void layout_objects_random(GameObject tile, int min, int max)
	{
		int count = Random.Range(min, max);

		for(int i = 0; i < count; i++)
		{
			Vector3 rpos = random_position();
			Instantiate(tile, rpos, Quaternion.identity);
		}
	}

	public void SetupScene()
	{
		board_setup();
		initialize_list();
		layout_objects_random(aliveTile, aliveCount.min, aliveCount.max);
	}
}
