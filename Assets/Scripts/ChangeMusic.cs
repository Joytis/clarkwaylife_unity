using UnityEngine;
using System.Collections;

public class ChangeMusic : MonoBehaviour {

	public AudioClip game_music;
	public AudioClip main_music;
	public AudioClip win_music;

	private AudioSource source;

	// Use this for initialization
	void Awake () {
		source = GetComponent<AudioSource>();
	}
	
	void OnLevelWasLoaded(int level) {
		if(level == 1){
			source.clip = null;
			source.clip = game_music;
			source.Play();
		}
		else if(level == 2) {
			source.clip = null;
			source.clip = win_music;
			source.Play();
		}
		else if(level == 3) {
			source.clip = null;
			source.clip = main_music;
			source.Play();
		}
	}
}
