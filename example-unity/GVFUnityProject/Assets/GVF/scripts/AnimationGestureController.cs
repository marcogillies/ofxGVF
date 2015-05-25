using UnityEngine;
using System.Collections;
using System;

public class AnimationGestureController : MonoBehaviour {

	GVF gvf;

	public AnimationRecorder animRecorder;
	public string [] animClipNames;

	// Use this for initialization
	void Start () {
		if(gvf == null){
			gvf = gameObject.GetComponent<GVF>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		// l to start and stop learning
		if (Input.GetKeyDown("l")){
			Debug.Log("pressed l");
			// if we are already learning, stop
            if(gvf.state == GVF.State.learning){
            	Debug.Log("ending learning");
            	gvf.FinishLearning();

				// save the animation of the target object
				// and set it as the clip attached to the
				// new gesture
             	animRecorder.EndRecording();
				Debug.Log("end recording");
				int numGestures = GVF.getNumberOfGestureTemplates();
             	if(numGestures > animClipNames.Length)
             	{
             		string [] temp = new string[numGestures];
             		for (int i = 0; i < animClipNames.Length; i++)
             			temp[i] = animClipNames[i];
             		animClipNames = temp;
             	}
             	animClipNames[numGestures-1] = animRecorder.animation.clip.name;

            } else {
            	Debug.Log("starting learning");
				gvf.BeginLearning();

		    	animRecorder.clipName = "Gesture" + gvf.GetNumGestures().ToString();
		    	animRecorder.StartRecording();
            }
		}

		if (Input.GetKeyDown("f")){
         	gvf.ToggleFollowing();   
		}


		if(gvf.state == GVF.State.following){
			int mostProbable = gvf.GetRecognisedGesture();
			//Debug.Log(mostProbable);
			if(mostProbable >= 0 && mostProbable < animClipNames.Length)
        	{
        		animRecorder.Play(animClipNames[mostProbable], gvf.GetTimeInGestureNormalized());
        	}
		}
	}
}
