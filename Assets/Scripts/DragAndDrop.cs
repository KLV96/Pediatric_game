using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour {

    private bool isDragged = false; //whether the player is currently dragging an item
    private GameObject spoon;   //holds a reference to an object being dragged
    private Vector2 touchOffset;    // allows a grabbed object to stick realistically to the player’s touch position (more about this later).

    Animator anim;
    public GameObject food;
    Animator spoonAnimation;
    public Sprite fullSpoon, emptySpoon;


    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        spoonAnimation = food.GetComponent<Animator>();
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
            if (isDragged)
                DropItem();
        }
    }

    /// <summary>
    ///  returns the position of a detected touch/mouse input
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

        if (isDragged)
        {
            spoon.transform.position = inputPosition + touchOffset;
        }
        else
        {
            RaycastHit2D[] touches = Physics2D.RaycastAll(inputPosition, inputPosition, 0.5f);
            if (touches.Length > 0)
            {
                var hit = touches[0];
                if (hit.transform != null)
                {
                    isDragged = true;
                    spoon = hit.transform.gameObject;
                    touchOffset = (Vector2)hit.transform.position - inputPosition;
                    //draggedObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
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
        isDragged = false;
        //draggedObject.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {

            this.GetComponent<SpriteRenderer>().sprite = fullSpoon;

    }
}