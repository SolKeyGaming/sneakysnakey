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
    public float Step;
    public float SecondsPerMove;

    // para manejar cambios de direccion
    public Direction Direction;
    public Direction PreviousDirection;

    // Elementos
    GameObject _cabeza;
    GameObject _cuerpos;
    GameObject _cola;
    List<Vector2> _wayPoints;
    Vector2 _posicionAntCabeza;

    string input;

    // Use this for initialization
    void Start()
    {
        _cabeza = GameObject.Find("Cabeza");
        _cuerpos = GameObject.Find("Cuerpos");
        _cola = GameObject.Find("Cola");

        //Posicionar cola segun el primer cuerpo (el mas alejado de la cabeza)
        _cola.transform.position = ListarCuerpos()[0].position - new Vector3(0,-0.64f,0);

        Direction = Direction.Up;

        // Inicializacion de los waypoints
        _wayPoints = new List<Vector2>() { _cabeza.transform.position };

        foreach (Transform t in ListarCuerpos())
        {
            _wayPoints.Add(t.position);
        }

        _wayPoints.Add(_cola.transform.position);
        _wayPoints.Reverse();

        // Invocacion del Metodo 'Move' cada 'SecondsPerMove' segundos
        InvokeRepeating("Move", 0, SecondsPerMove);

    }

    // Update is called once per frame
    void Update()
    {
        // Si hubo input, se almacena hasta ser consumido
        if(Input.anyKeyDown)
            input = Input.inputString;
    }

    void HandleInput()
    {
        // Manejar cambio de Direccion
        //TODO: cambiar input a swipes
        PreviousDirection = Direction;

        if (input == "w" && Direction != Direction.Down)
            Direction = Direction.Up;

        if (input == "d" && Direction != Direction.Left)
            Direction = Direction.Right;

        if (input == "s" && Direction != Direction.Up)
            Direction = Direction.Down;

        if (input == "a" && Direction != Direction.Right)
            Direction = Direction.Left;

        if (PreviousDirection != Direction)
            RotateByDirection(_cabeza.transform);

        input = "";
    }

    void Move()
    {
        HandleInput();

        Vector2 headDirection;

        switch (Direction)
        {
            case (Direction.Up):
                headDirection = new Vector2(0, Step);
                break;
            case (Direction.Left):
                headDirection = new Vector2(-Step, 0);
                break;
            case (Direction.Down):
                headDirection = new Vector2(0, -Step);
                break;
            case (Direction.Right):
                headDirection = new Vector2(Step, 0);
                break;
            default:
                headDirection = new Vector2();
                break;
        }

        // Mover la cabeza
        MoverObjeto(_cabeza, headDirection);

        int wpIndex = _wayPoints.Count - 1;
        List<Transform> listaDeCuerpos = ListarCuerpos();

        // Mover cada cuerpo en _cuerpos
        for (int i = 0; i <= listaDeCuerpos.Count - 1; i++)
        {
            // Obtener datos para calculo de direccion
            Vector2 posicionCuerpo = listaDeCuerpos[i].position;
            Vector2 posicionWayPoint = _wayPoints[wpIndex];
            
            // Obtener el vector direccion
            Vector2 vectorDireccion = new Vector2(posicionWayPoint.x - posicionCuerpo.x, posicionWayPoint.y - posicionCuerpo.y);

            // Mover cuerpo
            MoverObjeto(listaDeCuerpos[i].gameObject, vectorDireccion);
            wpIndex--;

        }

        // Mover cola
        Vector2 posicionCola = _cola.transform.position;
        Vector2 posicionWayPointCola = _wayPoints[1];       // el waypoint de la cola siempre sera 0, por lo que le sigue el 1

        Vector2 vDireccionCola = new Vector2(posicionWayPointCola.x - posicionCola.x, posicionWayPointCola.y - posicionCola.y);
        //vDireccionCola *= step;
        MoverObjeto(_cola, vDireccionCola);
        //_cola.transform.position += (Vector3)vDireccionCola;

        // Agregar la nueva posicion de la cabeza a los waypoints
        _wayPoints.Add(_cabeza.transform.position);

        //eliminar primer waypoint
        _wayPoints.RemoveAt(0);
    }

    void MoverObjeto(GameObject go, Vector2 direccion)
    {
        go.transform.position += (Vector3)direccion;
    }


    void MoverCuerpo()
    {
    }


    void RotateByDirection(Transform t)
    {
        Quaternion q = new Quaternion();
        int rotation = GetAngleByDirection(Direction);

        q.eulerAngles = new Vector3(0, 0, rotation);
        t.rotation = q;
    }

    List<Transform> ListarCuerpos()
    {
        List<Transform> lista = new List<Transform>();

        for (int i = 0; i <= _cuerpos.transform.childCount - 1; i++)
        {
            // Si el GameObject es uno activo en la jerarquía de la escena
            if (_cuerpos.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                lista.Add(_cuerpos.transform.GetChild(i));
            }
        }
        return lista;
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

    //helper
    void PrintWayPoints()
    {
        string s = "";
        foreach (Vector2 v in _wayPoints)
        {
            s += v.ToString() + " ";
        }
        Debug.Log(s);
    }

    public void AddWaypoint(Vector2 pos)
    {
        _wayPoints.Add(pos);
    }
}



// Implementar? es mejor que mi algoritmo
//var x = transform.localPosition.x;
//var y = transform.localPosition.y;

//		switch (_currentDirection)
//		{
//			case MoveDirection.Left:
//				x--;
//				break;
//			case MoveDirection.Up:
//				y++;
//				break;
//			case MoveDirection.Right:
//				x++;
//				break;
//			case MoveDirection.Down:
//				y--;
//				break;
//			case MoveDirection.None:
//				throw new ArgumentOutOfRangeException();
//			default:
//				throw new ArgumentOutOfRangeException();
//		}