using UnityEngine;
using System.Collections;



public class GesturePuppet : MonoBehaviour {

	public string [] animNames;

	public GVF gvf;

	public string currentAnim;

	// the start tie of the animation
	private float startTime = -1;

	private bool recording = false;

	private bool following = false;

	private float countDown = 5.0f;

	public float waitTime;

	// Use this for initialization
	void Start () {
		animNames = new string[animation.GetClipCount()];
		int i = 0;
		foreach (AnimationState state in animation) {
            animNames[i] = state.name;
			i++;
        }
	}
	
	// Update is called once per frame
	void Update () {
		if(recording)
		{

			if (countDown > 0.0f)
			{
				countDown -= Time.deltaTime;
				if(countDown < 0.0f)
				{
					countDown = 0.0f;
					gvf.BeginLearning();
					animation.Play(currentAnim);
				}
			} 
			else
			{
				//foreach (AnimationState state in animation) {
		        //    Debug.Log(state.name);
		        //}
				//Debug.Log(currentAnim);
				//Debug.Log(animation[currentAnim]);
				if(animation[currentAnim].normalizedTime > 0.99f)
				{
					recording = false;
					gvf.FinishLearning();
				}
			}
		}
		

		if(following)
		{

			if (countDown > 0.0f)
			{
				countDown -= Time.deltaTime;
				if(countDown < 0.0f)
				{
					countDown = 0.0f;
					gvf.BeginFollowing();
					//animation.Play(animNames[currentAnim]);
				}
			} 
			else
			{
				// if(animation[animNames[currentAnim]].normalizedTime > 0.99f)
				// {
				// 	recording = false;
				// 	gvf.FinishLearning();
				// }

				int mostProbable = gvf.GetRecognisedGesture();
				// //Debug.Log(mostProbable);
				if(mostProbable >= 0 && mostProbable < animNames.Length)
	        	{
	         		if(!animation.IsPlaying(animNames[mostProbable])){
	         			animation.Play(animNames[mostProbable]);
	         			animation[animNames[mostProbable]].speed = 0;
	         		}
	         		animation[animNames[mostProbable]].normalizedTime = gvf.GetTimeInGestureNormalized();
	         	}
			}
		}
	}


	void OnGUI(){

		if(recording){
			// if (GUI.Button(new Rect(10, Screen.height - 40, 70, 20), "Stop")){
			// 	recording = false;
			// 	gvf.FinishLearning();
			// }
			GUI.Label(new Rect(90, Screen.height - 40, 70, 20), "" + countDown);
			
			GUI.HorizontalSlider(new Rect(180, Screen.height - 40, Screen.width-200, 30), 
							animation[currentAnim].time, 0.0F, animation[currentAnim].length);
		} else if(following){
			if (GUI.Button(new Rect(10, Screen.height - 40, 70, 20), "Stop")){
				following = false;
				gvf.FinishFollowing();
			}
			GUI.Label(new Rect(90, Screen.height - 40, 70, 20), "" + countDown);
			
		} else {
			int i = 0;
			foreach (AnimationState state in animation) {
	            if (GUI.Button(new Rect(120*i + 10, Screen.height - 80, 100, 20), state.name)){
					recording = true;
					countDown = waitTime;
					//recordTime = anims[i].length;
					//triggerName = anims[i].name;
					currentAnim = state.name;
				}
				i++;
	        }
			// for(int i = 0; i < animNames.Length; i++){
			// 	if (GUI.Button(new Rect(120*i + 10, Screen.height - 80, 100, 20), animNames[i])){
			// 		recording = true;
			// 		countDown = waitTime;
			// 		//recordTime = anims[i].length;
			// 		//triggerName = anims[i].name;
			// 		currentAnim = i;
			// 	}
			// }

			if (GUI.Button(new Rect(10, Screen.height - 120, 100, 20), "follow")){
				following = true;
				countDown = waitTime;
				//recordTime = anims[i].length;
				//triggerName = anims[i].name;
				//currentAnim = i;
			}

			// if(animation.clip != null)
			// {
			// 	if(playing){
			// 		if(GUI.Button(new Rect(90, Screen.height - 40, 70, 20), "Pause"))
			// 		{
			// 			playing = false;
			// 			animation[animation.clip.name].speed = 0.0f;
			// 		}
			// 	} else {
			// 		if(GUI.Button(new Rect(90, Screen.height - 40, 70, 20), "Play"))
			// 		{
			// 			playing = true;
			// 			animation[animation.clip.name].speed = 1.0f;
			// 		}
			// 	}
			// 	animation[animation.clip.name].time =  GUI.HorizontalSlider(new Rect(180, Screen.height - 40, Screen.width-200, 30), 
			// 				animation[animation.clip.name].time, 0.0F, animation[animation.clip.name].length);
			// 	if(animation[animation.clip.name].time > animation[animation.clip.name].length)
			// 		animation[animation.clip.name].time = animation[animation.clip.name].time 
			// 					% animation[animation.clip.name].length;
			// }
		}
	}
}
