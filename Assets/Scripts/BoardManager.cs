using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using NUnit.Framework;

public class BoardManager : MonoBehaviour {
	[Serializable]
	public class Count
	{
		public int maximum;
		public int minimum;

		public Count (int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}

	public int columns = 12;
	public int rows = 12;

	public Count barrierCount = new Count(6,10);
	public Count energyCount = new Count(2, 6);
	public GameObject goal;
	public GameObject[] floorTiles;
	public GameObject[] barrierTiles;
	public GameObject[] energyTiles;
	public GameObject[] enemyTiles;
	public GameObject[] outerWallTiles;
	public GameObject[] destroyedBarrier;

	private Transform boardHolder;
	private List<Vector3> gridPositions = new List<Vector3>();

	void InitializeList()
	{
		gridPositions.Clear();

		for (int x = 1; x < columns - 1; x++) {
			for (int y = 1; y < rows - 1; y++) {
				gridPositions.Add (new Vector3(x, y, 0f));
			}
		}
	}

	void BoardSetup()
	{
		boardHolder = new GameObject ("Board").transform;

		for (int x = -1; x < columns + 1; x++) {
			for (int y = -1; y < rows + 1; y++) {
				GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];
				if (x == -1 || y == -1 || x == columns || y == rows)
					toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];

				GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
				instance.transform.SetParent (boardHolder);
			}
		}
	}

	Vector3 RandomPosition(bool removeTile = true)
	{
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		if (removeTile)
			gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}

	List<Vector3> LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum, bool removeTiles = true)
	{
		int objectCount = Random.Range (minimum, maximum);
		List<Vector3> positions = new List<Vector3>();

		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPosition ();
			positions.Add (randomPosition);
			GameObject tileChoice = tileArray [Random.Range (0, tileArray.Length)];
			Instantiate (tileChoice, randomPosition, Quaternion.identity);
		}
		return positions;
	}

	public void SetupScene(int level)
	{
		BoardSetup ();
		InitializeList ();
		List<Vector3> positions = LayoutObjectAtRandom (destroyedBarrier, barrierCount.minimum, barrierCount.maximum);
		for (int i = 0; i < positions.Count; i++) {
			GameObject tileChoice = barrierTiles [Random.Range (0, barrierTiles.Length)];
			Instantiate (tileChoice, positions [i], Quaternion.identity);
		}
		LayoutObjectAtRandom (energyTiles, energyCount.minimum, energyCount.maximum);
		int enemyCount = (int)Mathf.Log (level, 2f);
		LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
		Instantiate (goal, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
	}
}
