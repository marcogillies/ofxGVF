using UnityEngine;
using System.Collections;

[System.Serializable]
public class GestureCallback
{
	public GameObject gameObject;
	public string message;
}

public class DispatchGestureController : MonoBehaviour {

	public GVF gvf;

	public GestureCallback [] callbacks;

	// Use this for initialization
	void Start () {
		if(gvf == null){
			gvf = gameObject.GetComponent<GVF>();
		}
	}
	
	// Update is called once per frame
	void Update () {

		if(gvf.state == GVF.State.following){
			int mostProbable = gvf.GetRecognisedGesture();
			// //Debug.Log(mostProbable);
			if(mostProbable >= 0 && mostProbable < callbacks.Length)
        	{
         		callbacks[mostProbable].gameObject.SendMessage(callbacks[mostProbable].message, gvf.GetTimeInGestureNormalized());
         		
         	}
		}

		// l to start and stop learning
		if (Input.GetKeyDown("l")){
			// if we are already learning, stop
            if(gvf.state == GVF.State.learning){
            	
            	gvf.FinishLearning();
            } else {
				gvf.BeginLearning();
            }
		}

		if (Input.GetKeyDown("f")){
         	gvf.ToggleFollowing();   
		}

	}
}
