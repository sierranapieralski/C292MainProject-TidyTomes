using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Book : MonoBehaviour
{

    [SerializeField] GameObject moveBook;
    Vector3 initialPosition;

    public string color;

    bool isDragging = false;    // make public method and when is we drag to outline, then have book snap to correct position
    [SerializeField] bool isDraggable = false;


    // new
    bool shouldFall = false;
    bool isSnapped = false;
    float fallSpeed = 5f; 
    float fallTime = 2f;  // time in seconds for the book to fall before respawning
    float fallTimer = 0f; // timer to track the fall time of the book


    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        // new
        if (isDragging)
        {
            Vector3 convertedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(convertedPosition.x, convertedPosition.y, 0);
        }
        else if (shouldFall && !isSnapped)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;   // make the book fall off the screen

            fallTimer += Time.deltaTime;    // increast fall time

            // check if the fall time has passed
            if (fallTimer >= fallTime)
            {
                // reset variables about book
                shouldFall = false;
                fallTimer = 0f;
                transform.position = initialPosition;
                isDraggable = true; // draggable is true after respawn
            }
        }

    }

    private void OnMouseDown()
    {
       if (isDraggable)
        {
            isDragging = true;
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;

        // new
        if (!isSnapped)     
        {
            shouldFall = true;
            fallTimer = 0f;  // reset the timer when the fall starts
        }
    }

    public void snapBook() 
    {
        // stops being dragged by the mouse and is draggable false
        isDragging = false;
        isDraggable = false;


        // new
        shouldFall = false;
        isSnapped = true;
        fallTimer = 0f;  // stop any ongoing fall and reset the timer

    }

}
