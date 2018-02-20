﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syringe : MonoBehaviour {

	public bool resize = true;
	float timeCounter = 0.0f;
	int timeInteger = 0;
	SpriteRenderer m_SpriteRenderer;

	// Use this for initialization
	void Start () {
		m_SpriteRenderer = GetComponent<SpriteRenderer>();
	}


	// Update is called once per frame
	void Update () {
		if(resize == true) {
			if (timeInteger % 2 == 0) {
				m_SpriteRenderer.color = Color.blue;
				transform.localScale += new Vector3(0.01F, 0, 0);
			}else {
				m_SpriteRenderer.color = Color.white;
				transform.localScale -= new Vector3(0.01F, 0, 0);
			}
		}
		timeCounter += Time.deltaTime;
		timeInteger = (int)timeCounter;
	}



	public void OnMouseDown(){
		resize = false;	
	}

}
