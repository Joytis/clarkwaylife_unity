using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class GridScript : MonoBehaviour {
	public GameObject 			cell;

	private Transform 			parent_handle;
	

	// private CellScript[,] 		cells = new CellScript[height,width];
	// private GameObject[,] 		handles = new GameObject[height,width];
	

	void Start () {

		
		// update_cell_colors();
	}

	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}
		
	}
}
