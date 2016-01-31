using UnityEngine;
using System.Collections;

public class RuneController : MonoBehaviour
{


	public GameObject runeStandard;
	public GameObject runeHoverOver;
	public GameObject runeSelected;



	// Use this for initialization
	void Start ()
	{
		DisableRune();
	}
	
	// Update is called once per frame
	void Update ()
	{

	
	}

	public void EnableRune(){
		runeStandard.active = true;
	}

	public void DisableRune(){
		runeStandard.active = false;
		runeHoverOver.active = false;
		runeSelected.active = false;
	}

	void OnMouseEnter ()
	{

		if (runeStandard.active) {
			runeStandard.active = false;
			runeHoverOver.active = true;
		}
	}

	void OnMouseExit ()
	{
		if (runeHoverOver.active) {
			runeHoverOver.active = false;
			runeStandard.active = true;
		}
	}

	void OnMouseDown ()
	{
		if (runeStandard.active || runeHoverOver.active) {
			GetComponent<AudioSource>().Play();
			runeHoverOver.active = false;
			runeSelected.active = true;
			GameControllerScript controller = GameObject.FindGameObjectWithTag ("GameController").GetComponentInChildren<GameControllerScript> ();
			controller.playerSelection = gameObject.name;
			controller.GameCheck (gameObject);
		}
	}
}
