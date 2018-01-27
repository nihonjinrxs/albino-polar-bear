using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Enemy : MovingObject {

	public int playerDamage;

	private Animator animator;
	private Transform target;
	private bool skipMove;

	protected override void Start () {
		GameManager.instance.AddEnemyToList (this);
		animator = GetComponent<Animator> ();
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		base.Start ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	protected override void AttemptMove<T> (int xDir, int yDir)
	{
		if (skipMove) {
			skipMove = false;
			return;
		}

		base.AttemptMove<Player> (xDir, yDir);

		skipMove = true;
	}

	public void MoveEnemy()
	{
		// TODO: Change this to waypoint-based pathfinding patrol
		// Perhaps: https://docs.unity3d.com/Manual/nav-AgentPatrol.html
		int xDir = 0;
		int yDir = 0;

		if (Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon)
			yDir = target.position.y > transform.position.y ? 1 : -1;
		else
			xDir = target.position.x > transform.position.x ? 1 : -1;

		AttemptMove<Player> (xDir, yDir);
	}

	protected override void OnCantMove <T> (T component)
	{
		Player hitPlayer = component as Player;

		animator.SetTrigger ("enemyCapturePlayer");

		// TODO: Implement search visibility collision check using trigger "enemySpotPlayer"
		// TODO: Change this to capture when caught
		hitPlayer.LoseEnergy (playerDamage);
	}
}
