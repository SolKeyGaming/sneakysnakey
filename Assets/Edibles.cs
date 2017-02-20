using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Edibles {

	public class Edible : MonoBehaviour
    {
        public int puntaje;
    }

    public class EdibleBasico : Edible
    {
        private void Awake()
        {
            puntaje = 10;
        }
    }

    public class EdibleEspecial : Edible
    {
        public float tiempoDeVida;
        public float duracion;
    }

    public class DoblePuntos : EdibleEspecial
    {
        private void Awake()
        {
            puntaje = 15;
            tiempoDeVida = 5;
            duracion = 10;
        }
    }

}