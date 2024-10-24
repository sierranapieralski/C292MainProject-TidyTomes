using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    int booksFound = 0;

    public static GameManager instance;

    [SerializeField] TextMeshProUGUI booksFoundText;


    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseScore(int amount)
    {
        booksFound += amount;
        booksFoundText.text = "Books Found: " + booksFound + "/3";
    }
}
