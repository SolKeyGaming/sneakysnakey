using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public int Score;
	public int EndLevelPoints;

	// Use this for initialization
	void Start () {
		if (EndLevelPoints <= 0) {
			Debug.LogError ("Points to win must be more than 0");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Score >= EndLevelPoints) {
			// TODO: hacer final del nivel y weas
			GameObject.Destroy(GameObject.Find("Serpiente"));
		}
	}
}
