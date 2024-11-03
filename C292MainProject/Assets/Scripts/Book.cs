using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Book : MonoBehaviour
{

    [SerializeField] GameObject moveBook;
    Vector3 initialPosition;
    Vector3 initialRotation;

    public string color;

    bool isDragging = false;    // make public method and when is we drag to outline, then have book snap to correct position
    [SerializeField] bool isDraggable = false;


    bool shouldFall = false;
    bool isSnapped = false;
    float fallSpeed = 5f; 
    float fallTime = 2f;  // time in seconds for the book to fall before respawning
    float fallTimer = 0f; // timer to track the fall time of the book


    // add int for orientation 0, 90, 180, 270 to match rotation
    int rotationState = 0;  // tracks degrees of rotation of book


    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.eulerAngles; // saves the initial rotation of each book
    }

    // Update is called once per frame
    void Update()
    {

        // Rotate only if the book is currently being dragged
        if (isDragging)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) // Rotate 90 degrees clockwise
            {
                RotateBook(90);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) // Rotate 90 degrees counterclockwise
            {
                RotateBook(-90);
            }

            // Update the position to follow the mouse
            Vector3 convertedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(convertedPosition.x, convertedPosition.y, 0);
        }
        else if (shouldFall && !isSnapped)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            fallTimer += Time.deltaTime;

            if (fallTimer >= fallTime)
            {
                shouldFall = false;
                fallTimer = 0f;
                transform.position = initialPosition;
                transform.rotation = Quaternion.Euler(initialRotation);  // Reset rotation using Euler angles
                rotationState = 0;  // resets rotation state if book respawns
                isDraggable = true;
            }
        }

    }

    private void RotateBook(int angle)
    {
        rotationState = (rotationState + angle + 360) % 360; // Keep within 0, 90, 180, 270
        transform.rotation = Quaternion.Euler(0, 0, rotationState);
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

        if (!isSnapped)     
        {
            shouldFall = true;
            fallTimer = 0f;  // reset the timer when the fall starts
        }
    }

    public int GetRotationState()
    {
        return rotationState;
    }

    public void snapBook() 
    {
        // stops being dragged by the mouse and is draggable false
        isDragging = false;
        isDraggable = false;


        shouldFall = false;
        isSnapped = true;
        fallTimer = 0f;  // stop any ongoing fall and reset the timer

    }

}
