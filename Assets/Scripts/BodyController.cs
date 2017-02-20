using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BodyController : MovementManager
{

    //Vector2 position { get { return transform.position; } }
    //Vector2 previousPosition;
    SpriteRenderer sprites;
    Sprite StraightBodySprite;
    Sprite CornerBodySprite;

    // Use this for initialization
    void Start()
    {
        sprites = GetComponent<SpriteRenderer>();
        StraightBodySprite = gameObject.GetComponentInParent<BodySpriteHolder>().StraightBodySprite;
        CornerBodySprite = gameObject.GetComponentInParent<BodySpriteHolder>().CornerBodySprite;
    }

    // Update is called once per frame
    void Update()
    {
        HandleDirection();
    }

    public new void Move(Vector2 direction)
    {
        base.Move(direction);
        UpdateDirection();
        HandleDirection();
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
            transform.rotation = Direction.ToQuaternion();
            ChangeToStraightSprite();
        }
    }
}