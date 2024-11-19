using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{

    [SerializeField] string color;
    [SerializeField] GameManager manager;
    [SerializeField] int requiredRotation;  // Set this to 0, 90, 180, or 270 to match the outline's orientation


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        if (collision.gameObject.tag == "MoveBook")
        {
            Debug.Log("is a book!");


            var book = collision.gameObject.GetComponent<Book>();


            Debug.Log("current rotation " + book.GetRotationState());

            // checks if book color and rotation matched the outlines color and rotation
            if (book.color == color && book.GetRotationState() == requiredRotation)
            {
                // take book and snap to match outlines position, when it gets moved over outline, snaps to shelf
                book.snapBook();

                // move book to correct position
                collision.gameObject.transform.position = transform.position;

                // Check if this is a bonus item
                if (book.color == "plant1" || book.color == "plant2" || book.color == "lantern")
                {
                    GameManager.instance.AddBonusPoint(1); // Regular decor point addition
                }
                else
                {
                    // Increase book score
                    GameManager.instance.IncreaseScore(1);
                }
            }

            else
            {
                // Incorrect placement: Show hint bubble
                GameManager.instance.ShowHintBubble("Book must be placed in the correct outline in the correct orientation.");
            }
        }
    }
}
