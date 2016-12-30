using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour {

    List<GameObject> _listaDeGameObjects;
    GameObject _serpiente;

	// Use this for initialization
	void Start () {

        // buscar cada hijo de la serpiente (tag:Player)
        _serpiente = GameObject.FindGameObjectsWithTag("Player")[0];

        for (int i = 0; i < _serpiente.transform.childCount; i++)
        {
            //if(_serpiente.transform.GetChild(i).Get)
        }

        //    _listaDeGameObjects = _serpiente.transform.chil

    }

    // Update is called once per frame
    void Update () {
		
	}
}
