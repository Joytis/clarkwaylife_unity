using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Board board;



	// Use this for initialization
	void Awake() 
	{
		board = GetComponent<Board>();
		InitGame();
	}

	void InitGame()
	{
		board.SetupScene();
	}

	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
