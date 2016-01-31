using UnityEngine;
using System.Collections;

public class HornGrower : MonoBehaviour {


	public GameObject Horn1;
	public GameObject Horn2;



	// Use this for initialization
	void Start () {



	}
	
	// Update is called once per frame
	void Update () {
	

	}
		

	public void Horn1In(){
		Horn1.active = true;
	}

	public void Horn2In(){
		Horn2.active = true;
	}

	void Horn1Out(){
		Horn1.active = false;
	}

	void Horn2Out(){
		Horn2.active = false;
	}
}
