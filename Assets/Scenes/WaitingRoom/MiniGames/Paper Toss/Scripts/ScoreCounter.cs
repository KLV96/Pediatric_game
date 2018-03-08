﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour {

	public BoxCollider2D scoreIncrementCollider;
	public int score;
	public Text scoreText;

	void Start(){
		score = 0;
		scoreText.text = "WAITING ROOM";
	}

	void Update(){
	}

	void OnTriggerEnter2D(Collider2D other){
		
		score++;
		scoreText.text = "Score: " + score.ToString();
		Wait();
		other.gameObject.transform.Translate(new Vector3(250f, transform.position.y-90f), Space.World); //instead of 250f you could use a randomiser between -200 and 250.
			
	}

	IEnumerator Wait(){
		yield return new WaitForSeconds(3);
	}

}
