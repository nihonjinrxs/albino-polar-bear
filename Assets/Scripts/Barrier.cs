using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Barrier : MonoBehaviour {

	public Sprite damageSprite;
	public AudioClip barrierDestroySound;
	public int hp = 4;

	private SpriteRenderer spriteRenderer;

	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	public void DamageBarrier(int loss)
	{
		spriteRenderer.sprite = damageSprite;
		hp -= loss;
		if (hp < 0) {
			SoundManager.instance.RandomizeSfx (barrierDestroySound);
			gameObject.SetActive (false);
		}
	}
}
