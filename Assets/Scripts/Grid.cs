using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(ConwayArray))]
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

	private ConwayArray c;

	private SpriteRenderer sr;
	private Sprite s;
	private Texture2D t;


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
		c = gameObject.GetComponent<ConwayArray>();
		// sr = gameObject.GetComponent<SpriteRenderer>();

		size_x = c.width;
		size_y = c.height;

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
		update_texture();

		// Debug.Log(c.cell_state[50, 50].state);
	}

}
