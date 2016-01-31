using UnityEngine;
using System.Collections;

public class StoryController : MonoBehaviour {
	public PlayMusic playMusic;

	// Use this for initialization
	void Start () {
		playMusic = GameObject.Find("UI").GetComponentInChildren<PlayMusic>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){
		playMusic.StopSelectedMusic(0);
		playMusic.StopSelectedMusic(1);
		Application.LoadLevel("curtains");
	}
}
