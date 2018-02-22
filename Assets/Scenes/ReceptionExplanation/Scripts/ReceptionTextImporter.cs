﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReceptionTextImporter : MonoBehaviour {


    private TextAsset textFile;
    public string[] textLines;
    public TextAsset receptionTextFile;

    private int currentLine = 0;
    private int endLine = -1;

    private Text TextBox;
    private Button nextDialogButton;
    private GameObject dialogBox;

    public int procedureSelected = 0; //0 = Failed selection, 1 = Reception Dialog

	// Update is called once per frame
	void Start () {

        //Check for a valid text file

        switch (procedureSelected)
        {
            case 0:
                textFile = null;
                break;

            case 1:
                textFile = receptionTextFile;
                break;


        }
        if(textFile != null)
        {
            //Create an array of dialog strings from a supplied newline-seperated text file
            textLines = (textFile.text.Split('\n'));
            endLine = textLines.Length;

            //Assign TextBox to the text component of this game object and assign nextDialogButton to a child button and give it a on click action listener
            TextBox = this.gameObject.GetComponent<Text>();
            nextDialogButton = GameObject.FindGameObjectWithTag("NextButton").GetComponent<Button>();
            nextDialogButton.onClick.AddListener(nextText);
            dialogBox = GameObject.FindGameObjectWithTag("DialogBox");
            dialogBox.SetActive(false);
        }

    }

    /***Increments Currentline, thusly scrolling through a supplied dialog text file
     */
    void nextText()
    {
        currentLine++;
        if (currentLine >= endLine && endLine != -1)
        {
            currentLine = 0;
            dialogBox.SetActive(true);
            nextDialogButton.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        //Check and assign text on every frame refresh
        if(endLine != -1 && TextBox != null && textFile != null)
        {
            TextBox.text = textLines[currentLine];
        }
    }
}
