using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{

    [SerializeField] string color;
    [SerializeField] GameManager manager;

    // add int for orientation 0, 90, 180, 270 to match rotation which the if will check if the boot itself matches

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
            if (collision.gameObject.GetComponent<Book>().color == color)
            {
                Debug.Log("matched color!");

                // can check rotation

                // take book and snap to match outlines position, when it gets moved over outline, snaps to shelf
                collision.gameObject.GetComponent<Book>().snapBook();
                // move book to correct position
                collision.gameObject.transform.position = transform.position;
                GameManager.instance.IncreaseScore(1);

            }
        }
    }
}
