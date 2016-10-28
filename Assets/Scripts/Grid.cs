using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour {

	public int size_x;
	public int size_y;

	private Vector3[] vertices_;

	private void Generate() {
		vertices_ = new Vector3[(size_x + 1) * (size_y + 1)];
		for(int i = 0, y = 0; y < size_y; y++){
			for(int x = 0; x < size_x; x++, i++){
				vertices_[i] = new Vector3(x, y);
			}
		}
	}

	private void OnDrawGizmos() {
		// Can run in editor mode
		if(vertices_ == null){
			return;
		}

		Gizmos.color = Color.black;
		for(int i = 0; i < vertices_.Length; i++){
			Gizmos.DrawSphere(vertices_[i], 0.1f);
		}
	}

	void Awake(){
		Generate();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
