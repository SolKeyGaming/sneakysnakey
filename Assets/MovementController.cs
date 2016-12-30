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

    // Elementos
    GameObject _cabeza;
    GameObject _cuerpos;
    GameObject _cola;
    List<Vector2> _wayPoints;
    Vector2 _posicionAntCabeza;

    // Use this for initialization
    void Start()
    {
        _cabeza = GameObject.Find("Cabeza");
        _cuerpos = GameObject.Find("Cuerpos");
        _cola = GameObject.Find("Cola");

        _rigidBody2D = GetComponent<Rigidbody2D>();
        Direction = Direction.Up;

        _wayPoints = new List<Vector2>() { _cola.transform.position };
        foreach (Transform t in ListarCuerpos())
        {
            _wayPoints.Add(t.position);
        }
        _wayPoints.Add(_cabeza.transform.position);
        //_wayPoints.Reverse();

        _posicionAntCabeza = _cabeza.transform.position;

        InvokeRepeating("Move", 1, 1f);

    }

    // Update is called once per frame
    void Update()
    {

        // Agregar waypoint si hubo movimiento
        //if((Vector3)_posicionAntCabeza != _cabeza.transform.position)
        //{
        //    _wayPoints.Add(_cabeza.transform.position);
        //    _wayPoints.RemoveAt(0);
        //}
            

        PreviousDirection = Direction;
        //Move();

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
        Move();
    }

    void Move()
    {
        Vector2 headDirection;
        float step = _velocity * Time.deltaTime;
        switch (Direction)
        {
            case (Direction.Up):
                headDirection = new Vector2(0, step);
                break;
            case (Direction.Left):
                headDirection = new Vector2(-step, 0);
                break;
            case (Direction.Down):
                headDirection = new Vector2(0, -step);
                break;
            case (Direction.Right):
                headDirection = new Vector2(step, 0);
                break;
            default:
                headDirection = new Vector2();
                break;
        }

        // Se mueve la cabeza
        _posicionAntCabeza = _cabeza.transform.position;
        MoverObjeto(_cabeza, headDirection);


        int wpIndex = _wayPoints.Count - 1;
        List<Transform> listaDeCuerpos = ListarCuerpos();
        // Mover cada cuerpo en _cuerpos
        for (int i = 0; i <= listaDeCuerpos.Count - 1; i++)
        {
            // Mover HACIA el siguiente waypoint

            // Obtener datos para calculo de direccion
            Vector2 posicionCuerpo = listaDeCuerpos[i].position;
            Vector2 posicionWayPoint = _wayPoints[wpIndex];
            //Debug.Log("cuerpo "+i+" moviendose a waypoint "+wpIndex);
            // Obtener el vector direccion
            Vector2 vectorDireccion = new Vector2(posicionWayPoint.x - posicionCuerpo.x, posicionWayPoint.y - posicionCuerpo.y);
            // Factorizar la direccion segun el step
            vectorDireccion *= step;

            Debug.Log("elemento "+i+" sigue a "+posicionWayPoint);
            PrintWayPoints();
            // Mover cuerpo
            // FIX ME: 'listaDeCuerpos[i].position += (Vector3)vectorDireccion;' transform.position assign attempt for 'Cuerpo' is not valid. Input position is { -176267416332926980.000000, -Infinity, 0.000000 }. UnityEngine.Transform:set_position(Vector3) MovementController: Move()(at Assets / MovementController.cs:126)
            // Probablemente un error de calculo al vector direccion
            MoverObjeto(listaDeCuerpos[i].gameObject, vectorDireccion);
            //listaDeCuerpos[i].position += (Vector3)vectorDireccion;
            // Mover el index de los waypoints
            wpIndex--;

        }

        // Mover cola
        Vector2 posicionCola = _cola.transform.position;
        Vector2 posicionWayPointCola = _wayPoints[1];       // el waypoint de la cola siempre sera 1

        Vector2 vDireccionCola = new Vector2(posicionWayPointCola.x - posicionCola.x, posicionWayPointCola.y - posicionCola.y);
        vDireccionCola *= step;
        MoverObjeto(_cola, vDireccionCola);
        //_cola.transform.position += (Vector3)vDireccionCola;

        // Agregar la nueva posicion de la cabeza a los waypoints
        _wayPoints.Add(_cabeza.transform.position);

        //eliminar primer waypoint
        _wayPoints.RemoveAt(0);


        //Debug.Log("cola sigue a " + _wayPoints[1].ToString());
    }

    void MoverObjeto(GameObject go, Vector2 direccion)
    {
        var rigidBody2dCabeza = go.GetComponent<Rigidbody2D>();
        rigidBody2dCabeza.velocity = direccion;
    }


    void MoverCuerpo()
    {

    }


    void RotateByDirection()
    {
        Quaternion q = new Quaternion();
        int rotation = GetAngleByDirection(Direction);

        q.eulerAngles = new Vector3(0, 0, rotation);
        //para rotar el cuerpo entero
        //transform.rotation = q;
        //para rotar solo la cabeza
        var cabeza = GameObject.Find("Cabeza");
        cabeza.transform.rotation = q;
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
    void PrintWayPoints()
    {
        string s = "";
        foreach (Vector2 v in _wayPoints)
        {
            s += v.ToString() + " ";
        }
        Debug.Log(s);
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