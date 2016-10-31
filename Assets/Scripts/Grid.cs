using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(ConwayArray), typeof(MeshRenderer), typeof(MeshFilter))]
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

	private Mesh m;
	private SpriteRenderer sr;
	private Sprite s;
	private Texture2D t;

	private Vector3[] vertices_;


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

	void Generate() {
		GetComponent<MeshFilter>().mesh = m = new Mesh();
		m.name = "The Grid";

		vertices_ = new Vector3[(size_x + 1) * (size_y + 1)];
		Vector2[] uv = new Vector2[vertices_.Length];

		for(int i = 0, y = 0; y <= size_y; y++) {
			for(int x = 0; x <= size_x; x++, i++) {
				vertices_[i] = new Vector3(x, y);
				uv[i] = new Vector2((float)x / size_x, (float)y / size_y);
			}
		}

		m.vertices = vertices_;
		m.uv = uv;

		int[] ts = new int[size_x * size_y * 6];
		for(int ti = 0, vi = 0, y = 0; y < size_y; y++, vi++){
			for(int x = 0; x < size_x; x++, ti += 6, vi++) {
				ts[ti] = vi;
				ts[ti + 3] = ts[ti + 2] = vi + 1;
				ts[ti + 4] = ts[ti + 1] = vi + size_x + 1;
				ts[ti + 5] = vi + size_x + 2;
			}
		}

		m.triangles = ts;
		m.RecalculateNormals();
	}

	void Awake() {

		c = gameObject.GetComponent<ConwayArray>();
		// sr = gameObject.GetComponent<SpriteRenderer>();

		size_x = c.width;
		size_y = c.height;

		Generate();
	}

	void OnDrawGizmos() {
		if(vertices_ == null) {
			return;
		}

		Gizmos.color = Color.black;
		for(int i = 0; i < vertices_.Length; i++) {
			Gizmos.DrawSphere(vertices_[i], 0.1f);
		}
	}

	
	void OnGUI() {
		// GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), t);
	}

	// Update is called once per frame
	void Update () {
		// update_texture();

		// Debug.Log(c.cell_state[50, 50].state);
	}

}
