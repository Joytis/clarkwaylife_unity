using UnityEngine;
using System.Collections;

public class LoadOnClick : MonoBehaviour {


	public GameObject loadingImage;

	// Use this for initialization
	public void LoadScene(int level){
		loadingImage.SetActive(true); 
		Application.LoadLevel(level); 
	}
}
