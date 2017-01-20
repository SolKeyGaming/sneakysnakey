using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BodyController : MonoBehaviour
{

    Vector2 position { get { return transform.position; } }
    Vector2 previousPosition;
    SpriteRenderer sprites;
    Sprite StraightBodySprite;
    Sprite CornerBodySprite;
    
    public MoveDirection Direction;
    public MoveDirection PreviousDirection;

    // Entregado por el MovementController del GameObject 'Serpiente'
    public Vector2 NextWayPoint;
    

    public void Move(Vector2 direccion)
    {
        // Almacenar la posicion anterior
        previousPosition = position;
        // Mover Objeto
        transform.position += (Vector3)direccion;
        // Actualizar direccion
        UpdateDirection();
    }

    // Use this for initialization
    void Start()
    {
        sprites = GetComponent<SpriteRenderer>();
        previousPosition = position;
        StraightBodySprite = gameObject.GetComponentInParent<BodySpriteHolder>().StraightBodySprite;
        CornerBodySprite = gameObject.GetComponentInParent<BodySpriteHolder>().CornerBodySprite;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void UpdateDirection()
    {
        // Almacenar direccion anterior
        PreviousDirection = Direction;

        // Calcular proxima direccion
        if (position.x > NextWayPoint.x)
            Direction = MoveDirection.Left;

        else if (position.x < NextWayPoint.x)
            Direction = MoveDirection.Right;

        else if (position.y > NextWayPoint.y)
            Direction = MoveDirection.Down;

        else if (position.y < NextWayPoint.y)
            Direction = MoveDirection.Up;

        // Rotar objeto si ha cambiado de direccion
        HandleDirection();
    }

    void RotateByDirection(MoveDirection d)
    {
        Quaternion q = new Quaternion();
        int rotation = GetAngleByDirection(d);
        q.eulerAngles = new Vector3(0, 0, rotation);
        transform.rotation = q;
    }

    public int GetAngleByDirection(MoveDirection d)
    {
        int angle;
        //TODO: esto se repite en BehaviorController, limpiar
        switch (d)
        {
            case (MoveDirection.Up):
                angle = 0;
                break;
            case (MoveDirection.Right):
                angle = -90;
                break;
            case (MoveDirection.Down):
                angle = 180;
                break;
            case (MoveDirection.Left):
                angle = 90;
                break;
            default:
                throw new Exception("MovementController.RotateByDirection(): Bad direction value");
        }
        return angle;
    }

    void ChangeToCornerSprite()
    {
        // Cambiar sprite
        sprites.sprite = CornerBodySprite;
        // Restaurar rotacion
        transform.rotation = new Quaternion(0, 0, 0, 0);

        // Si va hacia arriba y a la izquierda, o hacia la derecha y abajo
        if ((PreviousDirection == MoveDirection.Up && Direction == MoveDirection.Left)
            || (PreviousDirection == MoveDirection.Right && Direction == MoveDirection.Down))
        {
            sprites.flipX = false;
            sprites.flipY = true;
        }
        // Si va hacia arriba y a la derecha, o hacia la izquierda y abajo
        else if ((PreviousDirection == MoveDirection.Up && Direction == MoveDirection.Right)
            || (PreviousDirection == MoveDirection.Left && Direction == MoveDirection.Down))
        {
            sprites.flipY = true;
            sprites.flipX = true;
        }
        // Si va hacia abajo y a la derecha, o hacia la izquierda y arriba
        else if ((PreviousDirection == MoveDirection.Down && Direction == MoveDirection.Right)
            || (PreviousDirection == MoveDirection.Left && Direction == MoveDirection.Up))
        {
            sprites.flipX = true;
            sprites.flipY = false;
        }
        // Si va hacia abajo y a la izquierda, o hacia la derecha y arriba
        else if ((PreviousDirection == MoveDirection.Down && Direction == MoveDirection.Left)
            || (PreviousDirection == MoveDirection.Right && Direction == MoveDirection.Up))
        {
            sprites.flipX = false;
            sprites.flipY = false;
        }

    }

    void ChangeToStraightSprite()
    {
        if (sprites.sprite != StraightBodySprite)
            sprites.sprite = StraightBodySprite;
        sprites.flipX = false;
        sprites.flipY = false;
    }

    public void HandleDirection()
    {
        // Rotar objeto si ha cambiado de direccion
        if (PreviousDirection != Direction)
        {
            ChangeToCornerSprite();
        }
        else
        {
            RotateByDirection(Direction);
            ChangeToStraightSprite();
        }
    }
}