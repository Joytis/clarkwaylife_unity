using UnityEngine;
using System.Collections;

public class CellScript : MonoBehaviour {

	public int is_alive;

	public Sprite 			aliveTile;
	public Sprite 			deadTile;

	private SpriteRenderer 	srend;
	private Transform 		trans;

	public void set_alive(){
		is_alive = 1;
		srend.sprite = aliveTile;
	}

	public void set_dead(){
		is_alive = 0;
		srend.sprite = deadTile;
	}

	public void set_color(Color c){
		srend.color = c;
	}

	public void init() {
		srend = gameObject.GetComponent<SpriteRenderer>();
		trans = gameObject.GetComponent<Transform>();

		set_dead();
	}

	void OnMouseDown(){
		set_alive();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
