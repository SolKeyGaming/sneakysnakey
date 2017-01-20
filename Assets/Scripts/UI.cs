using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour {

    MovementController movementControl;
    BehaviorController behaviorControl;

    // Use this for initialization
    void Start () {

        movementControl = GameObject.Find("Serpiente").GetComponent<MovementController>();
        behaviorControl = GameObject.Find("Serpiente").GetComponent<BehaviorController>();

        var swipe = new TKSwipeRecognizer();
        //swipe.boundaryFrame = new TKRect(0, 0, 50f, 50f); // TKRect origin is in bottomleft corner 
        swipe.gestureRecognizedEvent += (s) =>
        {
            movementControl.SwipeDetected(s);
        };
        TouchKit.addGestureRecognizer(swipe);

        var longPress = new TKLongPressRecognizer();
        longPress.gestureRecognizedEvent += (lp) =>
        {
            behaviorControl.AddBody();
            Debug.Log("longpress");
        };
        TouchKit.addGestureRecognizer(longPress);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
