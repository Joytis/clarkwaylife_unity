using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

	// Use this for initialization

	Camera cam;
	float height;
	float width;



	void Awake() {

		cam = Camera.main;
		height = 2f * cam.orthographicSize;
		width = height * cam.aspect;
		
		Screen.SetResolution(1024, 768, false);

		transform.position += new Vector3(width/2f, height/2f, 0);
	}

	// void Start () {
	// 
	// }
	
	// Update is called once per frame
	void Update () {
	
	}
}
