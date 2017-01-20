using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour {

    public GameObject GameOverCanvas;

    int score;

	// Use this for initialization
	void Start () {
        score = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GameOver()
    {
        Time.timeScale = 0;
        var go = Instantiate(GameOverCanvas);
        go.GetComponentInChildren<Text>().text += score;
    }

    public void AddToScore(int points)
    {
        score += points;
    }

}
