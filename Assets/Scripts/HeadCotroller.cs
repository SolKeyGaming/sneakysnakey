using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCotroller : MovementManager
{

    BehaviorController _behaviour;
    Rect _playArea;
    
    // Use this for initialization
    void Start()
    {
        _behaviour = GetComponentInParent<BehaviorController>();
        _playArea = GetComponentInParent<SnakeMovement>().PlayArea;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.StartsWith("Cuerpo") || collision.gameObject.name == "Cola")
        {
            GameObject.Find("ScreenManager").GetComponent<UiController>().GameOver();
        }

    }

}
