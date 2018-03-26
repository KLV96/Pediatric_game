﻿using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraRoomIntegrationTest {

	private void LoadSceneByName(string name){

		SceneManager.LoadScene(name, LoadSceneMode.Single);
	}

	private string[] tags = { "Strap1", "Strap2", "Sandbag1", "Sandbag2", "Table", "CameraTop", "CameraBottom" };

	[UnityTest]
	public IEnumerator CameraRoomCanRestartLevelPasses() {
		LoadSceneByName ("CameraRoom");
		yield return null;

		var pause = GameObject.FindGameObjectWithTag ("Pause").GetComponent<Button> ();
		pause.onClick.Invoke ();

		var restart = GameObject.Find ("RestartLevelButton").GetComponent<Button> ();
		restart.onClick.Invoke ();

		Assert.AreEqual ("CameraRoom", SceneManager.GetActiveScene ().name);

	}

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator CameraRoomCanMoveToMainMenuPasses() {
		LoadSceneByName ("CameraRoom");
		yield return null;
		yield return new WaitForSeconds (10);

		var sandbag1 = GameObject.FindGameObjectWithTag(tags[2]);
		sandbag1.GetComponent<DragAndDropCameraRoom>().setDraggedObject(GameObject.FindGameObjectWithTag(tags[2]));
		sandbag1.transform.position = new Vector2(-2f, -2f);
		yield return null;
		yield return new WaitForSeconds(1);
		var testingScript = sandbag1.GetComponent<DragAndDropCameraRoom>();
		testingScript.clickIntoPlace();
		yield return new WaitForSeconds(1);

		var sandbag2 = GameObject.FindGameObjectWithTag(tags[3]);
		sandbag2.GetComponent<DragAndDropCameraRoom>().setDraggedObject(GameObject.FindGameObjectWithTag(tags[3]));
		sandbag2.transform.position = new Vector2(-2f, 2f);
		yield return null;
		yield return new WaitForSeconds(1);
		testingScript.clickIntoPlace();
		yield return new WaitForSeconds(1);


	}
}
