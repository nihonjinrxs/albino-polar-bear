using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class SoundManager : MonoBehaviour {

	public AudioSource efxSource;
	public AudioSource musicSource;
	public AudioSource narrationSource;
	public static SoundManager instance = null;

	public float lowPitchRange = 0.95f;
	public float highPitchRange = 1.05f;

	// Use this for initialization
	void Start () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
		
	}

	public void PlaySingle(AudioClip clip)
	{
		efxSource.clip = clip;
		efxSource.Play ();
	}

	public void RandomizeSfx(AudioClip clip)
	{
		float randomPitch = Random.Range (lowPitchRange, highPitchRange);
		efxSource.pitch = randomPitch;
		efxSource.clip = clip;
		efxSource.Play ();
	}
}
