using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class Book : MonoBehaviour
{
    [SerializeField] private int startingRotation; // Set to 0, 90, 180, or 270 in the Inspector to match initial Z rotation
    private int rotationState;
    private Vector3 initialPosition;
    private Vector3 initialRotation; 

    public string color;

    private bool isDragging = false;  // make public method and when is we drag to outline, then have book snap to correct position
    [SerializeField] private bool isDraggable = false;

    private bool shouldFall = false;
    private bool isSnapped = false;
    private float fallSpeed = 5f;
    private float fallTime = 2f;  // Time in seconds for the book to fall before respawning
    private float fallTimer = 0f; // Timer to track the fall time of the book

    void Start()
    {
        // Save the initial position and rotation at the start
        initialPosition = transform.position;
        initialRotation = transform.eulerAngles;

        rotationState = startingRotation;
    }

    void Update()
    {
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
                RespawnBook();
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
            shouldFall = true; // Start the falling process
            fallTimer = 0f;  // Reset the fall timer
        }
    }

    public void snapBook()
    {
        // Stop dragging and disable falling
        isDragging = false;
        isDraggable = false;

        shouldFall = false;
        isSnapped = true;
        fallTimer = 0f; // Reset the fall timer
    }

    private void RespawnBook()
    {
        // Reset books position and rotation
        transform.position = initialPosition;
        transform.rotation = Quaternion.Euler(initialRotation);

        // Reset books state
        rotationState = startingRotation;
        shouldFall = false;
        fallTimer = 0f;
        isDraggable = true;
        isSnapped = false;
    }

    public int GetRotationState()
    {
        return rotationState;
    }
}