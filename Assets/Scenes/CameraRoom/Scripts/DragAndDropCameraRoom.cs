﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropCameraRoom : MonoBehaviour
{
    private bool strap1inPlace = false;
    private bool strap2inPlace = false;
    private bool sandbag1inPlace = false;
    private bool sandbag2inPlace = false;
    private bool camera1inPlace = false;
    private bool camera2inPlace = false;
    private bool tableInPLace = false;
    private bool zoomToLevel2 = false;
    private bool inLevel2 = false;
    private bool levelOver = false;
    private bool draggingItem = false; //whether the player is currently dragging an item
    private GameObject draggedObject;  //holds a reference to an object being dragged
    private Vector2 touchOffset;  // allows a grabbed object to stick realistically to the player’s touch position (more about this later).
    private string[] tags = { "Strap1", "Strap2", "Sandbag1", "Sandbag2", "Table", "CameraTop", "CameraBottom" };

    private int score = 0;
    private float elapsed = 0.0f;




    void Start()
    {
        makeTargetRed();
    }

    void makeTargetRed()
    {
        GameObject.FindGameObjectWithTag("Target").GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
    }

    void disableHitbox(string name)
    {
        GameObject.FindGameObjectWithTag(name).GetComponent<BoxCollider2D>().enabled = false;
    }

    void enableHitbox(string name)
    {
        GameObject.FindGameObjectWithTag(name).GetComponent<BoxCollider2D>().enabled = true;
    }

    void cameraOverTarget()
    {
        float x = GameObject.FindGameObjectWithTag("CameraTop").transform.position.x;
        if (x <= 3 && x >= 0.2)
        {
            score++;
            float targetR = GameObject.FindGameObjectWithTag("Target").GetComponent<SpriteRenderer>().color.r;
            float targetG = GameObject.FindGameObjectWithTag("Target").GetComponent<SpriteRenderer>().color.g;
            if (targetR <= 1 && targetG < 1 && targetR > 0 && targetG >= 0)
            {
                GameObject.FindGameObjectWithTag("Target").GetComponent<SpriteRenderer>().color = new Color(targetR - 0.005f, targetG + 0.005f, 0, 1);
            }
        }
    }


    /// <summary>
    /// calls the methods DropItems() and DragOrPickup() when required
    /// checks if the player is currently touching the screen and if 
    /// he is, Drag or pick up the object, otherwise drop the item
    /// </summary>
    void Update()
    {

        if (HasInput)
        {
            DragOrPickUp();
        }
        else
        {
            if (draggingItem)
                DropItem();
        }
        if (zoomToLevel2)
        {
            cameraZoom();
        }

        if (score >= 200 && !levelOver)
        {
            levelCleared();
        }
    }

    void FixedUpdate()
    {
        if (inLevel2)
        {
            cameraOverTarget();
        }
    }

    private void cameraZoom()
    {
        elapsed += Time.deltaTime;
        Camera.main.orthographicSize = Mathf.SmoothStep(5f, 3.5f, elapsed);
        Camera.main.transform.position = new Vector3(Mathf.SmoothStep(0f, 2.1f, elapsed), 0f, -10f);
        GameObject.FindGameObjectWithTag("CameraTop").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1f, 0f, elapsed));


        if (elapsed > 1.0f)
        {
            zoomToLevel2 = false;
            inLevel2 = true;
            elapsed = 0f;
        }
    }

    /// <summary>
    /// returns the position of a detected touch/mouse input
    /// </summary>
    Vector2 CurrentTouchPosition
    {
        get
        {
            Vector2 inputPos;
            inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return inputPos;
        }
    }

    /// <summary>
    /// if an item is being dragged, move it with the input; 
    /// if an object is not being dragged, pick up an object 
    /// that’s being touched.
    /// </summary>
    private void DragOrPickUp()
    {

        var inputPosition = CurrentTouchPosition;

        if (draggingItem)
        {
            draggedObject.transform.position = inputPosition + touchOffset;
            clickIntoPlace();
        }
        else
        {
            if (sandbag1inPlace && sandbag2inPlace && strap1inPlace && strap2inPlace && !camera1inPlace && !camera2inPlace)
            {
                enableHitbox("CameraTop");
                enableHitbox("CameraBottom");



            }
            if (camera1inPlace && camera2inPlace && !tableInPLace)
            {
                enableHitbox("Table");
            }
            if (inLevel2)
            {
                enableHitbox("CameraTop");
            }
            RaycastHit2D[] touches = Physics2D.RaycastAll(inputPosition, inputPosition, 0.5f);
            if (touches.Length > 0)
            {
                var hit = touches[0];

                PickUp(tags, hit, inputPosition);
            }
        }
    }

    void disableDragableItem(float x, float y)
    {
        DropItem();
        draggedObject.transform.position = new Vector2(x, y);
        draggedObject.GetComponent<BoxCollider2D>().enabled = false;
    }

   public void clickIntoPlace()
    {
        if (draggedObject.name.Equals("Table"))
        {
            draggedObject.transform.position = new Vector2(draggedObject.transform.position.x, 0.0f);
            GameObject.FindWithTag("Table").transform.position = new Vector2(GameObject.FindWithTag("Table").transform.position.x, 0.0f);
            if (GameObject.FindWithTag("Table").transform.position.x > 1.5f && GameObject.FindWithTag("Table").transform.position.x < 2.0f)
            {
                disableDragableItem(2.1f, 0.0f);
                disableHitbox("Table");
                tableInPLace = true;
                zoomToLevel2 = true;
            }
        }
        else if
            (draggedObject.name.Equals("Strap1") &&
            draggedObject.transform.position.y < 0.5f && draggedObject.transform.position.y > -0.5f
            && draggedObject.transform.position.x < -3.5f && draggedObject.transform.position.x > -4.5f
            && sandbag1inPlace && sandbag2inPlace)
        {
            disableDragableItem(-4.0f, 0.0f);
            strap1inPlace = true;
            draggedObject.transform.parent = GameObject.FindWithTag("Table").transform;
        }
        else if
            (draggedObject.name.Equals("Strap2") &&
            draggedObject.transform.position.y < 0.5f && draggedObject.transform.position.y > -0.5f
            && draggedObject.transform.position.x < 0.5f && draggedObject.transform.position.x > -0.5f
            && sandbag1inPlace && sandbag2inPlace)
        {
            disableDragableItem(0.0f, 0.0f);
            strap2inPlace = true;
            draggedObject.transform.parent = GameObject.FindWithTag("Table").transform;
        }
        else if
            (draggedObject.name.Equals("Sandbag1") &&
            draggedObject.transform.position.y < -1.15f && draggedObject.transform.position.y > -2.15f
            && draggedObject.transform.position.x < -1.5f && draggedObject.transform.position.x > -2.5f)
        {
            disableDragableItem(-2.0f, -1.65f);
            sandbag1inPlace = true;
            draggedObject.transform.parent = GameObject.FindWithTag("Table").transform;
        }
        else if
           (draggedObject.name.Equals("Sandbag2") &&
           draggedObject.transform.position.y > 1.15f && draggedObject.transform.position.y < 2.15f
           && draggedObject.transform.position.x < -1.5f && draggedObject.transform.position.x > -2.5f)
        {
            disableDragableItem(-2.0f, 1.65f);
            sandbag2inPlace = true;
            draggedObject.transform.parent = GameObject.FindWithTag("Table").transform;

        }
        else if
            (draggedObject.name.Equals("CameraTop") && sandbag1inPlace && sandbag2inPlace && strap1inPlace && strap2inPlace)
        {
            if (!inLevel2)
            {
                draggedObject.transform.position = new Vector2(6.3f, draggedObject.transform.position.y);
                if (draggedObject.transform.position.y > -0.5f && draggedObject.transform.position.y < 0.5f)
                {
                    disableDragableItem(6.3f, 0f);
                    disableHitbox("CameraTop");
                    camera1inPlace = true;
                }
            }
            else
            {
                draggedObject.transform.position = new Vector2(draggedObject.transform.position.x, 0f);
            }


        }
        else if
            (draggedObject.name.Equals("CameraBottom") && sandbag1inPlace && sandbag2inPlace && strap1inPlace && strap2inPlace)
        {
            draggedObject.transform.position = new Vector2(6.3f, draggedObject.transform.position.y);
            if (draggedObject.transform.position.y > -0.5f && draggedObject.transform.position.y < 0.5f)
            {
                disableDragableItem(6.3f, 0f);
                camera2inPlace = true;
                disableHitbox("CameraBottom");
            }

        }


    }

    void PickUp(string[] tags, RaycastHit2D hit, Vector2 inputPosition)
    {
        if (hit.transform != null)
        {
            foreach (string tag in tags)
            {
                if (hit.collider.name.Equals(tag))
                {
                    draggedObject = GameObject.FindGameObjectWithTag(tag);
                    Debug.Log("hit = :" + hit.collider.name);
                    draggingItem = true;
                    touchOffset = (Vector2)hit.transform.position - inputPosition;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// returns true when the player is currently
    /// touching the screen/holding the mouse button
    /// </summary>
    private bool HasInput
    {
        get
        {
            // returns true if either the mouse button is down or at least one touch is felt on the screen
            return Input.GetMouseButton(0);
        }
    }

    /// <summary>
    /// releases a picked up item
    /// </summary>
    void DropItem()
    {

        draggingItem = false;
        //draggedObject.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void setDraggedObject(GameObject draggedComponent)
    {
        draggedObject = draggedComponent;
    }

    #region
    /// <summary>
    /// Getters for all booleans, for testing
    /// </summary>
    public bool getstrap1inPlace()
    {
        return strap1inPlace;
    }
    public bool getstrap2inPlace()
    {
        return strap2inPlace;
    }
    public bool getsandbag1inPlace()
    {
        return sandbag1inPlace;
    }
    public bool getsandbag2inPlace()
    {
        return sandbag2inPlace;
    }
    public bool getcamera1inPlace()
    {
        return camera1inPlace;
    }
    public bool getcamera2inPlace()
    {
        return camera2inPlace;
    }
    public bool gettableInPLace()
    {
        return tableInPLace;
    }
    public bool getzoomToLevel2()
    {
        return zoomToLevel2;
    }
    public bool getinLevel2()
    {
        return inLevel2;
    }
    public bool getlevelOver()
    {
        return levelOver;
    }
    public bool getdraggingItem()
    {
        return draggingItem;
    }
    #endregion


    void levelCleared()
    {
        levelOver = true; //Signals to the script that the game has ended
        //TODO: Alex's transition work
    }
}
