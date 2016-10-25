using UnityEngine;
using System.Collections.Generic;

public class GridScript : MonoBehaviour {

	public int width = 160;
	public int height = 90;

	public GameObject cell;

	private Transform parent_handle;
	private CellScript[,] handles;
	private GameObject[,] cells = new GameObject[90,160];

	// Use this for initialization
	void Start () {

		for(int x = 0; x < width; x++){
			for(int y = 0; y < height; y++){
				cells[x, y] = (GameObject)Instantiate(cell, new Vector3(x, y, 0f), Quaternion.identity, parent_handle);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
