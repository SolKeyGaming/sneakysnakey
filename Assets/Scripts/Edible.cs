using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edible : MonoBehaviour
{

    public EdibleType EdibleType;
    protected EdibleManager edibleManager;

    // Use this for initialization
    protected void Awake()
    {
        edibleManager = FindObjectOfType<EdibleManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SnakeHead"))
        {
            Destroy(gameObject);

            // Desactivado por errores en los nuevos cuerpos
            other.GetComponentInParent<BehaviorController>().AddBody();

            edibleManager.EdibleEaten(this);
        }

    }
}
