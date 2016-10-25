using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

	// Use this for initialization

	Camera cam;
	float height;
	float width;

	public float orsize;
	public float aspect = 1.7777777777f;

	void Awake() {

		// cam = Camera.main;
		// height = 2f * cam.orthographicSize;
		// width = height * cam.aspect;
		
		// Screen.SetResolution(1024, 768, false);

		// transform.position += new Vector3(width/2f, height/2f, 0);
	}

	void Start () {
	    cam = Camera.main;

	    cam.orthographicSize = orsize;

	    cam.projectionMatrix = Matrix4x4.Ortho(
	             -orsize * aspect, orsize * aspect,
	             -orsize, orsize,
	             cam.nearClipPlane, cam.farClipPlane);

		height = 2f * cam.orthographicSize;
		width = height * cam.aspect;

		// cam.orthographicSize = height;

		transform.position += new Vector3((width/2f), height/2f, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
