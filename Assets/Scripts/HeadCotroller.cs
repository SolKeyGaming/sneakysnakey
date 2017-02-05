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
        if (collision.gameObject.tag == "Edible")
        {

            float x = Random.Range(_playArea.xMin + 0.5f, _playArea.xMax - 0.5f);
            float y = Random.Range(_playArea.yMin + 0.5f, _playArea.yMax - 0.5f);

            // Redondeado a factores de 0.64 para posicion estilo grid
            x -= x % 0.64f;
            y -= y % 0.64f;

            var go = Instantiate(collision.gameObject, new Vector2(x,y), Quaternion.identity);

            Destroy(collision.gameObject);
            _behaviour.AddBody();

            // TODO: buscar una solucion al problema de usar .Find()
            UiController ui = GameObject.Find("ScreenManager").GetComponent<UiController>();
            ui.AddToScore(1);

        }

        if (collision.gameObject.name.StartsWith("Cuerpo") || collision.gameObject.name == "Cola")
        {
            GameObject.Find("ScreenManager").GetComponent<UiController>().GameOver();
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Cuerpo" || collision.gameObject.name == "Cola")
        {
            Debug.Log(collision.gameObject.name);
        }

    }


}
