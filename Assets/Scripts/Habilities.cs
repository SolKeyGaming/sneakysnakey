using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PowerUpTimerPair{
	public PowerUpType type;
	public float timeRemaining;
}

public class Habilities : MonoBehaviour {
	
	SnakeMovement movement;
	BehaviorController behaviorController;

	List<PowerUpTimerPair> activePowerUps;

	void Start(){
		activePowerUps = new List<PowerUpTimerPair> ();
		movement = GetComponent<SnakeMovement> ();
		behaviorController = GetComponent<BehaviorController> ();
	}

	void Update(){

		// Checking powerups state
		if (activePowerUps.Count != 0) {
			for (int i = 0; i < activePowerUps.Count; i++) {
				var pair = activePowerUps [i];
				if (pair.timeRemaining > 0) {
					// Subtract elapsed time to the powerup timer
					pair.timeRemaining -= Time.deltaTime;
				} else if (pair.timeRemaining <= 0) {
					// Invert powerup effect
					DeactivateHability(pair.type);
					// Remove from the list
					activePowerUps.RemoveAt(i);
				}
			} 
		}

	}

	public void ActivateHability(PowerUpType p_type){
		switch (p_type) {
		case PowerUpType.SlowMo:
			
			activePowerUps.Add (new PowerUpTimerPair () {
				type = p_type,
				timeRemaining = 5f
			});

			movement.SecondsPerMove *= 2;
			movement.CancelInvoke ();
			movement.InvokeRepeating ("Move", movement.SecondsPerMove, movement.SecondsPerMove);

			break;
		default:
			Debug.LogError ("No PowerUp of type: " + p_type + " found");
			return;
		}
	}

	public void DeactivateHability(PowerUpType p_type){
		switch (p_type) {
		case PowerUpType.SlowMo:
			
			movement.SecondsPerMove /= 2;
			movement.CancelInvoke ();
			movement.InvokeRepeating ("Move", movement.SecondsPerMove, movement.SecondsPerMove);

			break;
		default:
			Debug.LogError ("No PowerUp of type: " + p_type + " found");
			return;
		}
	}
}
