using UnityEngine;
using System.Collections;

public class GameControllerScript : MonoBehaviour {
	public int fails = 0;
	public GameObject[] symbols;
	public int level = 1;
	public int MAX_LEVEL = 8;
	public int FAIL_LIMIT = 3;
	public int RUNE_PAUSE = 5;
	public float FADE_PAUSE = 0.8f;
	public object playerSelection; //the selection the player has just made
	public GameObject[] mirrorRunes;
	public Sprite[] availableMirrorSprites;
	public GameObject rainbowPonies;

	public AudioSource wrongPatternSound;
	public AudioSource correctPatternSound;

	public PlayMusic playMusic;

	public GameObject storyImage;

	public GameObject lightSourceObject;
	private Light lightSource;

	private ArrayList required_sequence;
	private ArrayList currentSequence; //what is the current sequence the player has been shown
	private ArrayList mirrorSequenceIndex;
	private bool gameOverTriggered = false;

	// Use this for initialization
	void Start () {
		playMusic = GameObject.Find("UI").GetComponentInChildren<PlayMusic>();
		lightSource = lightSourceObject.GetComponent<Light>();
		rainbowPonies.GetComponent<ParticleSystem>().Pause();
		fails = 0;
		level = 1;
		required_sequence = new ArrayList();
		currentSequence = new ArrayList();
		mirrorSequenceIndex = new ArrayList();
		StartGame();
	}
	
	// Update is called once per frame
	void Update () {
		if((fails >= FAIL_LIMIT || level > MAX_LEVEL) && !gameOverTriggered){
			gameOverTriggered = true;
			Debug.Log("GameOver???");
//			StartCoroutine(GameOver());
		}
	
	}

	public void StartGame(){
		Debug.Log("Game has started");
		while(GenerateSequence() == null){}
		StartCoroutine(ShowSequence());
	}

	//returns null if not able to generate
	//NEED TO CHECK RETURN FOR NULL
	object GenerateSequence(){
		required_sequence.Clear();
		mirrorSequenceIndex.Clear();
		for(int i = 0; i < level; i++){
			int random = Random.Range(0, symbols.Length);
			int try_count = 0;
			while(required_sequence.Contains(symbols[random]) && try_count < 10){
				random = Random.Range(0, symbols.Length);
				try_count++;
			}
			if(try_count >= 10){
				return null;
			}
			Debug.Log("Adding index: " + random);
			required_sequence.Add(symbols[random]);
			mirrorRunes[required_sequence.Count - 1].GetComponentInChildren<SpriteRenderer>().sprite = availableMirrorSprites[random];
			mirrorSequenceIndex.Add(random);
		}
		return true;
	}

	void ShowFrameRunes(){
		for(int i = 0; i < required_sequence.Count; i++){
			RuneController rune = ((GameObject)required_sequence[i]).GetComponent<RuneController>();
			rune.EnableRune();
		}

		int total_runes = required_sequence.Count;

		for(int i = 0; i < total_runes; i++){
			int random = Random.Range(0, symbols.Length - 1);
			while(required_sequence.Contains(symbols[random])){
				random = Random.Range(0, symbols.Length - 1);
			}
			Debug.Log("Extra index: " + random);
			RuneController rune = symbols[random].GetComponentInChildren<RuneController>();
			rune.EnableRune();
		}
	}

	IEnumerator ShowSequence(){
		//call function to layout and fade symbols in
		yield return new WaitForSeconds(1.0f);
		for(int i = 0; i < required_sequence.Count; i++){			
			mirrorRunes[i].active = true;
			yield return new WaitForSeconds(FADE_PAUSE);
		}

		yield return new WaitForSeconds(RUNE_PAUSE);

		for(int i = 0; i < required_sequence.Count; i++){
			mirrorRunes[i].active = false;
		}

		ShowFrameRunes();

/*		for(int i=1; i < level; i++){
			object tempObject;
			tempObject = currentSequence.GetValue (i);
				if (i = 1){
					tempObject.transform.rotation = rune1Location.transform.rotation;
				}
				else if (i = 2){
					tempObject.transform.rotation = rune2Location.transform.rotation;
				}
				else if (i = 3){
					tempObject.transform.rotation = rune3Location.transform.rotation;
				}
				else if (i = 4){
					tempObject.transform.rotation = rune4Location.transform.rotation;
				}
				else if (i = 5){
					tempObject.transform.rotation = rune5Location.transform.rotation;
				}
				else if (i = 6){
					tempObject.transform.rotation = rune6Location.transform.rotation;
				}
				else{
					tempObject.transform.rotation = rune7Location.transform.rotation;
				}
			}
					*/
	}

	//increases the level user is at
	//acts as # for getting random symbols
	void IncreaseLevel(){
		level += 1;
	}

	//increases the fail count
	void IncreaseFails(){
		if(fails == 1){
			GetComponent<CandleController>().disableCandle(fails - 1);
		}
		else if(fails == 2){
			GetComponent<CandleController>().disableCandle(fails - 1);
		}

		fails += 1;

		if(fails == 1){
			HornGrower controller = GameObject.FindGameObjectWithTag("HornGrower").GetComponentInChildren<HornGrower>();
			controller.Horn1In();
			lightSource.intensity = 3.5f;
			lightSource.color = Color.red;
		}
		else if(fails > 1){
			HornGrower controller = GameObject.FindGameObjectWithTag("HornGrower").GetComponentInChildren<HornGrower>();
			controller.Horn2In();
			lightSource.intensity = 6.0f;
		}
		else if(fails >= FAIL_LIMIT){
			lightSource.intensity = 15.0f;
		}
	}

	//handles Game Over logic if user hits the FAIL_LIMIT
	IEnumerator GameOver(){
		if(level > MAX_LEVEL){
			rainbowPonies.SetActive(true);
			rainbowPonies.GetComponent<ParticleSystem>().Play();
		}
		yield return new WaitForSeconds(5);
		if(level > MAX_LEVEL){
			playMusic.PlaySelectedMusic(0);
		}
		else{
			playMusic.PlaySelectedMusic(1);
		}

		storyImage.active = true;

		yield return new WaitForSeconds(30);

		if(level > MAX_LEVEL){
			playMusic.StopSelectedMusic(0);
		}
		else{
			playMusic.StopSelectedMusic(1);
		}
		Application.LoadLevel("curtains");
	}

	void HideFrameRunes(){
		for(int i = 0; i < symbols.Length; i++){
			symbols[i].GetComponent<RuneController>().DisableRune();
		}
	}


	//handles verification of user selection to randomly generated number
	public void GameCheck (GameObject rune){
		currentSequence.Add(rune);
		if(required_sequence.Contains(rune)){//[currentSequence.Count - 1].Equals(rune)){
			int index = required_sequence.IndexOf(rune);
			int mirror_index = (int)mirrorSequenceIndex[index];
			mirrorRunes[index].SetActive(true);

			//correct individual rune
			if(required_sequence.Count == currentSequence.Count){
				correctPatternSound.Play();
				HideFrameRunes();
				
				NewRound();
			}
		}
		else{
			wrongPatternSound.Play();
			//die!
			IncreaseFails();
			currentSequence.Clear();
			HideFrameRunes();
			HideMirrorRunes();
			if(fails >= FAIL_LIMIT){
				StartCoroutine(GameOver());
				return;
			}
			StartCoroutine(ShowSequence());
		}
		/*
		object checkNum = currentSequence.GetValue (selectionNumber);
		Debug.Log(checkNum);
		if (checkNum == playerSelection){
			selectionNumber++;
			if (selectionNumber > level) {
				NewRound ();
			}
			Debug.Log ("MATCH!");
		}
		else {
			IncreaseFails ();
			ShowSequence ();
			Debug.Log ("Not Match");
		}
*/

	}

	void HideMirrorRunes(){
		for(int i = 0; i < mirrorRunes.Length; i++){
			mirrorRunes[i].SetActive(false);
		}
	}

	//handles if player successfully chooses an entire sequence and moves to new level
	void NewRound(){
		HideMirrorRunes();
		IncreaseLevel ();
		if(level > MAX_LEVEL){
			StartCoroutine(GameOver());
			return;
		}
		required_sequence.Clear();
		currentSequence.Clear();
		GenerateSequence ();

		StartCoroutine(ShowSequence ());
	}





}
