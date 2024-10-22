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


    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            Vector3 convertedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(convertedPosition.x, convertedPosition.y, 0);
        }

    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log("In Book file on trigger enter 2d function");
    //    Debug.Log("Collision detected with: " + collision.gameObject.name);

    //    if (collision.gameObject.tag == "EmptyBookSlot")   // If the player moves found book to collide with correct book outline,
    //                                                       // place book in correct shelf spot, delete the outline and book so cant
    //                                                       // be moved again, and increase player score of total books found
    //    {
    //        Debug.Log("Player moved book to empty book slot on shelf!");
    //        //collision.gameObject.GetComponent<Player>().GrantImmunity(immunityDuration);
    //        //Destroy(gameObject);  // Remove the book and book slot from the game after it's collected
    //    }
        
    //}

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
    }

    public void snapBook() 
    {
        // stops being dragged by the mouse and is draggable false
        isDragging = false;
        isDraggable = false;

    }
}
