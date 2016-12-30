using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Up,
    Right,
    Down,
    Left
}

public class MovementController : MonoBehaviour
{

    Rigidbody2D _rigidBody2D;

    [SerializeField] //por mientras, deberia se manejado SOLO por detras
    float _velocity;

    // para manejar cambios de direccion
    public Direction Direction;
    public Direction PreviousDirection;


    // Use this for initialization
    void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        Direction = Direction.Up;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        PreviousDirection = Direction;
        Move();

        HandleInput();
    }

    void HandleInput()
    {
        // Manejar cambio de Direccion
        //TODO: cambiar input a swipes
        if (Input.GetKeyDown(KeyCode.W) && PreviousDirection != Direction.Down)
            Direction = Direction.Up;
        if (Input.GetKeyDown(KeyCode.D) && PreviousDirection != Direction.Left)
            Direction = Direction.Right;
        if (Input.GetKeyDown(KeyCode.S) && PreviousDirection != Direction.Up)
            Direction = Direction.Down;
        if (Input.GetKeyDown(KeyCode.A) && PreviousDirection != Direction.Right)
            Direction = Direction.Left;

        if (PreviousDirection != Direction)
            RotateByDirection();
    }

    void Move()
    {
        Vector2 vDirection;
        float step = _velocity * Time.deltaTime;
        switch (Direction)
        {
            case (Direction.Up):
                vDirection = new Vector2(0, step);
                break;
            case (Direction.Left):
                vDirection = new Vector2(-step, 0);
                break;
            case (Direction.Down):
                vDirection = new Vector2(0, -step);
                break;
            case (Direction.Right):
                vDirection = new Vector2(step, 0);
                break;
            default:
                vDirection = new Vector2();
                break;
        }
        _rigidBody2D.velocity = vDirection;
    }

    void RotateByDirection()
    {
        Quaternion q = new Quaternion();
        int rotation = GetAngleByDirection(Direction);
        
        q.eulerAngles = new Vector3(0,0,rotation);
        transform.rotation = q;
        //para rotar solo la cabeza
        //var cabeza = GameObject.Find("Cabeza");
        //cabeza.transform.rotation = q;
    }

    //helper
    public int GetAngleByDirection(Direction d)
    {
        int angle;
        //TODO: esto se repite en BehabiourController, limpiar
        switch (Direction)
        {
            case (Direction.Up):
                angle = 0;
                break;
            case (Direction.Right):
                angle = -90;
                break;
            case (Direction.Down):
                angle = 180;
                break;
            case (Direction.Left):
                angle = 90;
                break;
            default:
                throw new Exception("MovementController.RotateByDirection(): Bad direction value");
        }
        return angle;
    }
}
