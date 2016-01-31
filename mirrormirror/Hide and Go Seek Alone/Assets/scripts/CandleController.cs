using UnityEngine;
using System.Collections;

public class CandleController : MonoBehaviour
{
	public GameObject[] candles;
	public AudioSource sound;

	private int count = 0;
	// Use this for initialization
	void Start ()
	{
		count = 0;
		for(int i = 0; i < candles.Length; i++){
			candles[i].SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public void disableCandle(int v){
		if(candles[v] != null){
//			candles[v].GetComponent<AudioSource>().
			candles[v].SetActive(false);
		}
	}
}
