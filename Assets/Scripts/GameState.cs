﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameState : MonoBehaviour {
	//TODO:
	//Dzivibas speletaja
	//Limenu parvaldiba

	public GameState()
	{
		guid = Guid.NewGuid();
		Debug.Log (guid);
	}
	public Guid guid;
	public int PlayerHealth;
	public GameObject Player;
	public Texture HealthPicture;

	List<Texture> backgrounds = new List<Texture>();
	List<GameObject> generators = new List<GameObject>();

	// Use this for initialization
	void Start () {
		//Seting up state
		//this.PlayerHealth = PlayerHealth;
		backgrounds.Add (Resources.Load<Texture> ("bacground_1"));
		backgrounds.Add (Resources.Load<Texture> ("bacground_2"));


		generators.Add (GameObject.Find ("RandomGenerator"));
		generators.Add (GameObject.Find ("RandomShipGenerator"));
		generators.Add (GameObject.Find ("Junk1Generator"));
		generators.Add (GameObject.Find ("Junk2Generator"));

		PrepareLevel (false);
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI()
	{
		for(int i =0; i< this.PlayerHealth; i++)
		GUI.DrawTexture (new Rect (10 + i*29, 10, 20, 27), HealthPicture);
		TimeSpan span = TimeSpan.FromSeconds (Time.timeSinceLevelLoad);

		GUI.Label (new Rect (Screen.width/2 - 50, 10, 100, 30), string.Format("{0:00}:{1:00}:{2:00}", span.Hours,span.Minutes, span.Seconds));
	}

	public void PlayerHit()
	{

		this.PlayerHealth = this.PlayerHealth - 1;
		if(this.PlayerHealth <= 0)
		{
			Destroy(Player);
		 	StartCoroutine(RestartGame());

		}
	}

	public void PortalHit()
	{

				PrepareLevel (true);
		}

	private IEnumerator RestartGame()
	{
		float length = gameObject.GetComponent<Fading> ().Begin (1);
		yield return new WaitForSeconds (length);
		Application.LoadLevel ("Main 1");
	}

	private void PrepareLevel(bool navigate = true)
	{	
		if (navigate) {
			Debug.Log ("GoingDark");
		 	StartCoroutine(LightsOff());
						
				
				}


		var rnd = new System.Random ();
		
		//background
		var bkg = GameObject.Find ("Quad");
		var backgid = rnd.Next(0, backgrounds.Count);
		bkg.renderer.material.mainTexture = backgrounds [backgid];

		//generators
		DisableGenerators ();
		var gencount = rnd.Next (1, 4);

		for (int i = 0; i< gencount; i++) {
			EnableGenerator(generators[rnd.Next(0,4)], rnd);
		}

		if (navigate)
					StartCoroutine (LightsOn());
		
	}

	IEnumerator LightsOff()
	{

		float length = gameObject.GetComponent<Fading> ().Begin (1);
		yield return new WaitForSeconds (length);
	}

	IEnumerator LightsOn()
	{
		float length = gameObject.GetComponent<Fading> ().Begin (-1);
		yield return new WaitForSeconds (length);
	}

	void EnableGenerator(GameObject r, System.Random rnd)
	{
		r.SetActive (true);
		var gen = r.GetComponent<RandomGenerator> ();
		if (gen == null)
						Debug.LogError ("Ǵeneratoram nav randomizétája");
		gen.minDelta = rnd.Next (4, 9) / 10.0f;
		gen.maxDelta = rnd.Next (10, 15) / 10.0f;
	}

	private void DisableGenerators()
	{
		foreach (var item in generators) {
			item.SetActive(false);
		}
	}

	private GameState m_Instance;
	public GameState Instance { get { return m_Instance; } }
	
	void Awake()
	{
		m_Instance = this;
	}
	
	void OnDestroy()
	{
		m_Instance = null;
	}
}
