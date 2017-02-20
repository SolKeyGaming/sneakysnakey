using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType{
	SlowMo,
	dummy1,
	dummy2
}

public class PowerUp : Edible {

	public PowerUpType PowerUpType;

	void OnTriggerEnter2D(){
		Destroy (gameObject);
		edibleManager.EdibleEaten (this);
	}
}
