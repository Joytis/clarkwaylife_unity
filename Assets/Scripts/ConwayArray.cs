using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;

public class ConwayArray : MonoBehaviour {

	[Serializable]
	public class CellState {
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
		public Color color;
	}

	private enum GRID_STATES {
		ON,
		OFF
	};

	public int frames_per_update = 5;
	public int width = 96;
	public int height = 54;

	public CellState[,] cell_state;
	public CellState[,] prev_cell_state;

	private int COL_MAX;
	private int ROW_MAX;

	public Gradient g;
	private Vector3 grid_diag;

	private float color_offset;

	private GRID_STATES grid_state = GRID_STATES.ON;
	private int frames = 0;


	// UPDATE GRID
	//===============================================================

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

		// for(int row = 0; row < height; row++){
		// 	for(int col = 0; col < width; col++){
		// 		prev_cell_state[row, col].state = cell_state[row, col].state;
		// 	}
		// }

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
					}
					else if(state == 2){
						cell_state[row, col].state = 0;
					}
				}
				else {
					cell_state[row, col].state = prev_cell_state[row, col].state;
				}
			}
		}
	}

	// UPDATE CELL COLORS
	//===============================================================
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

				cell_state[row,col].color = g.Evaluate(t_offset);
				prev_cell_state[row,col].color = g.Evaluate(t_offset);
			}
		}
	}

	// GRID STATE TOGGLE
	//===============================================================
	void grid_state_toggle(){
		grid_state = (grid_state == GRID_STATES.OFF) ? GRID_STATES.ON : GRID_STATES.OFF;
	}


	// private GradientColorKey[] 	gck;
	// private GradientAlphaKey[] 	gak;

	// Use this for initialization
	void Start () {

		// Initialize the lists. 
		COL_MAX = width - 1;
		ROW_MAX = height - 1;

		cell_state = new CellState[height, width];
		prev_cell_state = new CellState[height, width];

		Debug.Log(g.Evaluate(0.35f));

		// Initialize the cell_states
		for(int row = 0; row < height; row++){
			for(int col = 0; col < width; col++){
				cell_state[row,col] = new CellState();
				prev_cell_state[row,col] = new CellState();
			}
		}

		// Do some linear
		grid_diag = new Vector3((float)width, (float)height, 0f);

		Vector3 vtemp;
		float mtemp;
		float gpos;
		for(int x = 0; x < height; x++){
			for(int y = 0; y < width; y++){
				// Set up the cells
				// handles[x, y] = (GameObject)Instantiate(cell, new Vector3((y + 0.5f), (x + 0.5f), 0f), Quaternion.identity, parent_handle);
				// cells[x, y] = handles[x, y].GetComponent<CellScript>();
				// cells[x, y].init();
				if(Random.Range(0, 2) == 0)
					cell_state[x, y].state = 0;
				else
					cell_state[x, y].state = 1;

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

		update_cell_colors();

	}


	
	// Update is called once per frame
	void Update () {
		// CHECKING INPU TMETHODS.
		if(Input.GetKeyDown(KeyCode.Space)){
			grid_state_toggle();
		}

		if(Input.GetKeyDown(KeyCode.RightArrow)){
			frames_per_update++;
			if(frames_per_update > 60){
				frames_per_update = 60; 
				frames = 0;
			}
		}

		if(Input.GetKeyDown(KeyCode.LeftArrow)){
			frames_per_update--;
			if(frames_per_update < 1){
				frames_per_update = 1;
				frames = 0;
			}
		}

		// STATES :D
		switch (grid_state) 
		{
			case GRID_STATES.ON:
				// HACKY!
				frames++;
				if(frames % frames_per_update == 0)
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
