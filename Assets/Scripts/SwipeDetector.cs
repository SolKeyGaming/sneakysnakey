using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour {

	SnakeMovement movementController;

	// Use this for initialization
	void Start () {

		movementController = GameObject.Find ("Serpiente").GetComponent<SnakeMovement> ();

		var _recognizer = new TKPanRecognizer();

		// when using in conjunction with a pinch or rotation recognizer setting the min touches to 2 smoothes movement greatly
		if( Application.platform == RuntimePlatform.IPhonePlayer )
			_recognizer.minimumNumberOfTouches = 2;

		_recognizer.gestureRecognizedEvent += ( r ) =>
		{
			//TODO: fine tune minimum translation to detect IF NEEDED
			//if(recognizer.deltaTranslation.x>1 || recognizer.deltaTranslation.y>1){
				movementController.SwipeDetected(r);
				Debug.Log( "pan recognizer fired: " + r );
			//}
		};

		// continuous gestures have a complete event so that we know when they are done recognizing
		_recognizer.gestureCompleteEvent += r =>
		{
			Debug.Log( "pan gesture complete" );
		};
		TouchKit.addGestureRecognizer( _recognizer );
	}

}
