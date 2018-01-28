using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject {

	public int barrierDamage = 2;
	public int barrierHitEnergyLoss = 3;
	public int pointsPerEnergy = 20;
	public float restartLevelDelay = 1f;
	public Text energyText;

	private Animator animator;
	private int energy;

	// Use this for initialization
	protected override void Start () {
		animator = GetComponent<Animator> ();

		energy = GameManager.instance.playerEnergyPoints;

		energyText.text = "Energy: " + energy;

		base.Start ();
	}

	private void OnDisable()
	{
		GameManager.instance.playerEnergyPoints = energy;
	}

	// Update is called once per frame
	void Update () {
		if (!GameManager.instance.playersTurn)
			return;

		int horizontal = 0;
		int vertical = 0;

		horizontal = (int)Input.GetAxisRaw ("Horizontal");
		vertical = (int)Input.GetAxisRaw ("Vertical");

		if (horizontal != 0)
			vertical = 0;

		if (horizontal != 0 || vertical != 0)
			AttemptMove<Barrier> (horizontal, vertical);
	}

	protected override void AttemptMove <T> (int xDir, int yDir)
	{
		energy--;
		energyText.text = "Energy: " + energy;

		base.AttemptMove <T> (xDir, yDir);

		if (xDir == -1)
			animator.SetTrigger ("playerMoveLeft");
		if (yDir == -1)
			animator.SetTrigger ("playerMoveDown");
		if (xDir == 1)
			animator.SetTrigger ("playerMoveRight");
		if (yDir == 1)
			animator.SetTrigger ("playerMoveUp");

		RaycastHit2D hit;

		CheckIfGameOver ();

		GameManager.instance.playersTurn = false;
	}

	private void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Goal") {
			Invoke ("Restart", restartLevelDelay);
			enabled = false;
		} else if (other.tag == "Energy") {
			energy += pointsPerEnergy;
			energyText.text = "Collected " + pointsPerEnergy + " energy!\nEnergy: " + energy;
			other.gameObject.SetActive (false);
		}
	}

	protected override void OnCantMove <T> (T component) {
		animator.SetTrigger ("playerHit");
		Barrier hitBarrier = component as Barrier;
		hitBarrier.DamageBarrier (barrierDamage);
		LoseEnergy (barrierHitEnergyLoss);
	}

	private void Restart()
	{
		SceneManager.LoadScene (0);
	}

	public void LoseEnergy(int loss)
	{
		energy -= loss;
		energyText.text = "Lost " + loss + " energy!\nEnergy: " + energy;
		CheckIfGameOver ();
	}

	private void CheckIfGameOver()
	{
		if (energy <= 0)
			GameManager.instance.GameOver ();
	}
}
