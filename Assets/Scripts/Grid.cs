using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(ConwayArray), typeof(BoxCollider2D))]
public class Grid : MonoBehaviour {

	// [Serializable]
	// class CellState {
	// 	public CellState(){
	// 		state = 0;
	// 		grad_pos = 0f;
	// 	}

	// 	public CellState(int s, float f){
	// 		state = s;
	// 		grad_pos = f;
	// 	}
	// 	public int state;
	// 	public float grad_pos;
	// 	public Color color;
	// }



	private int size_x;
	private int size_y;
	private float ppr;
	private float ppc;

	private ConwayArray c;

	// private SpriteRenderer sr;
	// private Sprite s;
	private Texture2D t;
	private BoxCollider2D bx;
	private Camera cam;
	private Transform tr;


	void update_texture() {
		for(int y = 0; y < size_y; y++) {
			for(int x = 0; x < size_x; x++) {
				if(c.cell_state[y,x].state == 1) 
					t.SetPixel(x, y, c.cell_state[y, x].color);
				else
					t.SetPixel(x, y, Color.black);
			}
		}
		t.Apply();
	}

	// Use this for initialization
	void Start () {
		c = GetComponent<ConwayArray>();
		bx = GetComponent<BoxCollider2D>();
		tr = transform;
		cam = Camera.main;

		float osize = cam.orthographicSize;
		float aspkt = cam.aspect;

		bx.size = new Vector2((osize * 2f) * aspkt, osize * 2f);
		tr.position = new Vector3(0, 0, 0);
		// tr.position *= new Vector3(tr.position.x, tr.position.y, 0);

		Debug.Log(bx.size);
		Debug.Log(bx.enabled);
		Debug.Log(bx.transform.position);

		size_x = c.width;
		size_y = c.height;

		ppr = (float)((float)Screen.height / (float)size_y);
		ppc = (float)((float)Screen.width / (float)size_x);

		t = new Texture2D(size_x, size_y, TextureFormat.ARGB32, false);
		t.filterMode = FilterMode.Point;

		for(int y = 0; y < size_y; y++){
			for(int x = 0; x < size_x; x++){
				t.SetPixel(x, y, Color.black);
			}
		}
		t.Apply();

	}
	
	void OnGUI() {
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), t);
	}

	// Update is called once per frame
	void Update () {

		// Handle inputs
		if(Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}

		update_texture();

	}

	void OnMouseDown(){
		Debug.Log("Cool dudes B)");

		// Get the mouse position.
		// Then, translate onto the conway data structure. 

		Vector3 pos = Input.mousePosition;
		int col = (int)((float)pos.x / ppc);
		int row = (int)((float)pos.y / ppr);
		Debug.Log(pos);
		Debug.Log("Col: " +col);
		Debug.Log("Row: " +row);

		c.turn_on_cell(row, col);
	}

}
