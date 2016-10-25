using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class GridScript : MonoBehaviour {

	[Serializable]
	class CellState {
		public CellState(){
			state = 0;
			grad_pos = 0f;
		}

		public CellState(int s, float f){
			state = s;
			grad_pos = f;
		}
		public int state;
		public float grad_pos;
	}

	public static int width = 80;
	public static int height = 45;

	private static int COL_MAX = width - 1;
	private static int ROW_MAX = height - 1;

	public GameObject cell;

	private Transform parent_handle;
	private CellState[,] cell_state;
	private CellState[,] prev_cell_state;

	private CellScript[,] cells = new CellScript[height,width];
	private GameObject[,] handles = new GameObject[height,width];

	private Gradient g;
	private GradientColorKey[] gck;
	private GradientAlphaKey[] gak;

	private Vector3 grid_diag;

	private float color_offset;

	private enum GRID_STATES {
		ON,
		OFF
	};

	private GRID_STATES grid_state = GRID_STATES.ON;

	private int frames = 0;

	// Use this  for initialization
	// Any live cell with fewer than two live neighbours dies, as if caused by under-population.
	// Any live cell with two or three live neighbours lives on to the next generation.
	// Any live cell with more than three live neighbours dies, as if by over-population.
	// Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
	void update_grid()
	{
		// Helper variables
		int neighbours = 0;
		int state = 0; // 0: no change, 1: alive, 2: dead

		CellState[,] temp_state = prev_cell_state;
		prev_cell_state = cell_state;
		cell_state = temp_state;

		for(int row = 0; row < height; row++){
			for(int col = 0; col < width; col++){
				prev_cell_state[row, col].state = cells[row, col].is_alive;
			}
		}


		for(int row = 0; row < height; row++){
			for(int col = 0; col < width; col++){
				neighbours = 0;
				state = 0;
				// Get the cardinal positions
				if( row != 0 && col != 0 ) 				// SOUTH WEST
					neighbours += prev_cell_state[row - 1, col - 1].state;

				if(row != 0 )							// SOUTH
					neighbours += prev_cell_state[row - 1, col].state;

				if( row != 0 && col != COL_MAX )		// SOUTH EAST
					neighbours += prev_cell_state[row - 1, col + 1].state;

				if( col != COL_MAX )					// EAST
					neighbours += prev_cell_state[row, col + 1].state;

				if( row != ROW_MAX && col != COL_MAX) 	// NORTH EAST
					neighbours += prev_cell_state[row + 1, col + 1].state;

				if( row != ROW_MAX )					// NORTH
					neighbours += prev_cell_state[row + 1, col].state;

				if( row != ROW_MAX && col != 0 )		// NORT WEST
					neighbours += prev_cell_state[row + 1, col - 1].state;

				if( col != 0 )							// WEST
					neighbours += prev_cell_state[row, col - 1].state;

				// Do the stuff
				if(prev_cell_state[row, col].state == 1){
					if(neighbours < 2)
						state = 2;
					else if(neighbours > 3)
						state = 2;
				}
				else{
					if(neighbours == 3)
						state = 1;
				}

				// Change the state if it needs to be changed. 
				if(state != 0){
					if(state == 1){
						cell_state[row, col].state = 1;
						cells[row, col].set_alive();
					}
					else if(state == 2){
						cell_state[row, col].state = 0;
						cells[row, col].set_dead();
					}
				}
				else {
					cell_state[row, col].state = prev_cell_state[row, col].state;
				}
			}
		}
	}

	void update_cell_colors()
	{
		float t_time = Time.deltaTime / 5.0f;
		color_offset += t_time;

		if(color_offset > 1.0f)
			color_offset -= 1.0f;

		for(int row = 0; row < height; row++){
			for(int col = 0; col < width; col++){
				float t_offset = cell_state[row,col].grad_pos + color_offset;
				if(t_offset > 1.0f)
					t_offset -= 1.0f;

				cells[row,col].set_color(g.Evaluate(t_offset));
			}
		}
	}

	void Start () {
		//	Collapses them in the view nicely
		parent_handle = new GameObject("BoardHolder").transform;

		cell_state = new CellState[height, width];
		prev_cell_state = new CellState[height, width];

		// Initialize the cell_states
		for(int row = 0; row < height; row++){
			for(int col = 0; col < width; col++){
				cell_state[row,col] = new CellState();
				prev_cell_state[row,col] = new CellState();
			}
		}

		// Color keys
		g = new Gradient();
		gck = new GradientColorKey[4];
		gak = new GradientAlphaKey[2];
		gck[0].color = Color.red;
		gck[0].time = 0.0f;
		gck[1].color = Color.green;
		gck[1].time = 0.333333333333f;
		gck[2].color = Color.blue;
		gck[2].time = 0.666666666666f;
		gck[3].color = Color.red;
		gck[3].time = 1.0f;

		// Alpha keys
		gak[0].alpha = 1.0f;
		gak[0].time = 0.0f;
		gak[1].alpha = 1.0f;
		gak[1].time = 1.0f;

		// Apply keys
		g.SetKeys(gck, gak);
		Debug.Log(g.Evaluate(0.35f));

		// Do some linear
		grid_diag = new Vector3((float)width, (float)height, 0f);

		Vector3 vtemp;
		float mtemp;
		float gpos;
		for(int x = 0; x < height; x++){
			for(int y = 0; y < width; y++){
				// Set up the cells
				handles[x, y] = (GameObject)Instantiate(cell, new Vector3((y + 0.5f), (x + 0.5f), 0f), Quaternion.identity, parent_handle);
				cells[x, y] = handles[x, y].GetComponent<CellScript>();
				cells[x, y].init();
				if(Random.Range(0, 2) == 0)
					cells[x, y].set_dead();
				else
					cells[x, y].set_alive();

				// Debug.Log(cell_state[x,y].state);		

				cell_state[x, y].state = cells[x, y].is_alive;

				// Calculate the gradient position
				vtemp = new Vector3((float)y, (float)x, 0f);
				vtemp = Vector3.Project(vtemp, grid_diag);
				mtemp = grid_diag.magnitude - vtemp.magnitude;
				gpos = mtemp / grid_diag.magnitude;

				// Set the gradiant position;
				cell_state[x, y].grad_pos = gpos;
				prev_cell_state[x, y].grad_pos = gpos;
			}
		}

		// update_cell_colors();
	}

	void grid_state_toggle(){
		grid_state = (grid_state == GRID_STATES.OFF) ? GRID_STATES.ON : GRID_STATES.OFF;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			grid_state_toggle();
		}

		switch (grid_state) 
		{
			case GRID_STATES.ON:
				// HACKY!
				frames++;
				if(frames % 3 == 0)
				{
					update_grid();
				}
				update_cell_colors();
				break;

			case GRID_STATES.OFF:
				break;
		}
		
	}
}
