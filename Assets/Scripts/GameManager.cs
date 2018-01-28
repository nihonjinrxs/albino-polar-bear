using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.UI;
using System;
using UnityEngine.Analytics;

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

	private void OnLevelWasLoaded(int index)
	{
		level++;
		InitGame ();
	}

	void InitGame(){
		doingSetup = true;

		levelImage = GameObject.Find ("Level Image");
		levelText = GameObject.Find ("Level Text").GetComponent<Text> ();
		levelText.text = "Transmission\n" + Convert.ToString(level, 2);
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
