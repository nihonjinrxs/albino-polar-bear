using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public static GameManager instance = null;

	public BoardManager boardScript;
	public float levelStartDelay = 2f;
	public float turnDelay = 0.1f;
	public int playerEnergyPoints = 100;
	[HideInInspector] public bool playersTurn = true;

	private int level = 1;
	private Text levelText;
	private GameObject levelImage;
	private List<Enemy> enemies;
	private bool enemiesMoving;
	private bool doingSetup;

	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
		boardScript = GetComponent<BoardManager> ();
		enemies = new List<Enemy> ();
		InitGame ();
	}

	/*
	//This is called each time a scene is loaded.
	private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		//Add one to our level number.
		level++;
		//Call InitGame to initialize our level.
		InitGame();
	}

	private void OnEnable()
	{
		//Tell our ‘OnLevelFinishedLoading’ function to start listening for a scene change event as soon as this script is enabled.
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	private void OnDisable()
	{
		//Tell our ‘OnLevelFinishedLoading’ function to stop listening for a scene change event as soon as this script is disabled.
		//Remember to always have an unsubscription for every delegate you subscribe to!
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}
	*/

	private void OnLevelWasLoaded(int index)
	{
		level++;
		InitGame ();
	}

	void InitGame(){
		doingSetup = true;

		levelImage = GameObject.Find ("Level Image");
		levelText = GameObject.Find ("Level Text").GetComponent<Text> ();
		int levelBase;
		string levelTag;
		if (level < 16) {
			levelBase = 2;
			// levelTag = "\u2082";
			levelTag = Convert.ToString(level, levelBase).PadLeft (4,'0') + "/b";
		} else if (level < 512) {
			levelBase = 8;
			// levelTag = "\u2088";
			levelTag = Convert.ToString(level, levelBase).PadLeft (3,'0') + "/o";
		} else {
			levelBase = 16;
			// levelTag = "\u2081\u2086";
			levelTag = Convert.ToString(level, levelBase).PadLeft (4,'0') + "/x";
		}
		levelText.text = "Transmission\n" + levelTag;
		levelImage.SetActive (true);
		Invoke ("HideLevelImage", levelStartDelay);
		
		enemies.Clear ();
		boardScript.SetupScene (level);
	}

	private void HideLevelImage()
	{
		levelImage.SetActive (false);
		doingSetup = false;
	}

	public void GameOver()
	{
		levelText.text = "Game Over\nYou successfully retrieved " + (level - 1) + " transmissions.\nCan you do better next time?";
		levelImage.SetActive (true);
		enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (playersTurn || enemiesMoving || doingSetup)
			return;

		StartCoroutine (MoveEnemies ());
	}

	public void AddEnemyToList(Enemy script)
	{
		enemies.Add (script);
	}

	IEnumerator MoveEnemies()
	{
		enemiesMoving = true;
		yield return new WaitForSeconds (turnDelay);
		if (enemies.Count == 0) {
			yield return new WaitForSeconds (turnDelay);
		}

		for (int i = 0; i < enemies.Count; i++) {
			enemies [i].MoveEnemy ();
			yield return new WaitForSeconds (enemies [i].moveTime);
		}

		playersTurn = true;
		enemiesMoving = false;
	}
}
