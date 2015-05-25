using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;

// [System.Serializable]
// public class AnimationTrigger
// {
// 	public string name;
// 	public float length;
// }

public class AnimationRecorder : MonoBehaviour {
	
	// public AnimationTrigger [] anims;

	//public IKHandle ikHandle;

	//public LeapHandController leapController;
	//public MeshRenderer [] leapObjects;

	//public GUIStyle customGuiStyle;

	// the root transform of the animation
	// (the object that the animation is attached to)
	public Transform animationRoot;
	// the joints to include in the animation
	public Transform [] animationJoints;

	// the name to call the animation
	public string clipName = "Anim";
	// used to add numbers to ensure the animation is different
	public int clipCounter = 0;

	public string saveFolder = "Assets/RecordedAnimations/";

	public float waitTime = 5.0f;

	public float recordTime = 30.0f;

	// public Animator animatorToTrigger;
	// public string triggerName;

	//public Animation featureAnimation;
	
	//public GameObject kinectSensor;

	// the start tie of the animation
	private float startTime = -1;

	private bool recording = false;

	private float countDown = 0.0f;

	//public float StartTime{
	//	get {return startTime;}
	//}

	// the animation curves we are saving
	private AnimationCurve[,] curves;
	// the paths for the joints associated with 
	// each curve
	private string[] paths;

	private bool playing = false;
	
	// Use this for initialization
	void Start () {
	
	}

	// creates a keyframe from the current values of the joints
	void SaveKeyFrame(float t, AnimationCurve[,] _curves, Transform [] joints)
	{
		for (int i = 0; i < _curves.GetLength(0); i++)
		{
			_curves[i, 0].AddKey(new Keyframe(t, joints[i].localPosition.x));
			_curves[i, 1].AddKey(new Keyframe(t, joints[i].localPosition.y));
			_curves[i, 2].AddKey(new Keyframe(t, joints[i].localPosition.z));
			_curves[i, 3].AddKey(new Keyframe(t, joints[i].localRotation.x));
			_curves[i, 4].AddKey(new Keyframe(t, joints[i].localRotation.y));
			_curves[i, 5].AddKey(new Keyframe(t, joints[i].localRotation.z));
			_curves[i, 6].AddKey(new Keyframe(t, joints[i].localRotation.w));
		}
	}
	
	// Update is called once per frame
	// saves the current frame
	void Update () {
		if(recording)
		{

			if (countDown > 0.0f)
			{
				countDown -= Time.deltaTime;
				if(countDown < 0.0f)
				{
					countDown = 0.0f;
					StartRecording();
				}
			} 
			else
			{
				// checks if we have set startTime yet
				if(startTime < 0)
					startTime = Time.time;
					
				// the keyframe time
				float t = Time.time - startTime;

				//if(t < recordTime)
				//{
					// save the keyframe to the curves
				SaveKeyFrame(t, curves, animationJoints);
				//} 
				//else
				//{
				//	recording = false;
				//	EndRecording();
				//}
			}
		}
		//if(playing){
		//	animation[animation.clip.name].time += Time.deltaTime;
		//}
	}

	// works out the path string that will identify the transform
	String CalculateTransformPath(Transform joint, Transform root)
	{
		if(joint == root || joint == null)
			return "";
		String rest = CalculateTransformPath(joint.parent, root);
		if(rest == "")
			return joint.name;
		else
			return rest + "/"+ joint.name;
	}

	// creates the animation curves
	void SetUpCurves(AnimationCurve [,] _curves, string[] _paths, Transform root, Transform[] joints)
	{
		for (int i = 0; i < joints.Length; i++)
		{
			if(joints[i])
			{
				_paths[i] = CalculateTransformPath (joints[i], root); 
				print (_paths[i]);
				for(int j = 0; j < 7; j++)
				{
					_curves[i,j] = new AnimationCurve();
				}
			}
		}
	}

	public void StartRecording()
	{
		animation.Stop();
		animation.enabled = false;
		//GetComponent<Animator>().enabled = true;
		recording = true;
		startTime = -1;

		//ikHandle.following = true;

		//leapController.enabled = true;
		//for (int i = 0; i < leapObjects.Length; i++) {
		//	leapObjects[i].enabled = false;
		//}

		
		curves = new AnimationCurve[animationJoints.Length,7];
		paths = new string[animationJoints.Length];
		SetUpCurves(curves, paths, animationRoot, animationJoints);

		// animatorToTrigger.SetTrigger(triggerName);
		
	}

	// finish recording and save the animation
	public void EndRecording()
	{
		//GetComponent<Animator>().enabled = false;
		animation.enabled = true;

		//ikHandle.following = false;
		
		AnimationClip clip = new AnimationClip();
	
		for (int i = 0; i < curves.GetLength(0); i++)
		{
			if(curves[i,0] != null)
			{
				//print(paths[i]);
				clip.SetCurve(paths[i], typeof(Transform), "localPosition.x", curves[i,0]);
				clip.SetCurve(paths[i], typeof(Transform), "localPosition.y", curves[i,1]);
				clip.SetCurve(paths[i], typeof(Transform), "localPosition.z", curves[i,2]);
				clip.SetCurve(paths[i], typeof(Transform), "localRotation.x", curves[i,3]);
				clip.SetCurve(paths[i], typeof(Transform), "localRotation.y", curves[i,4]);
				clip.SetCurve(paths[i], typeof(Transform), "localRotation.z", curves[i,5]);
				clip.SetCurve(paths[i], typeof(Transform), "localRotation.w", curves[i,6]);
			}
		}

		string animationName = clipName;
		
		//if(clipCounter > 0)
		//	animationName = animationName + "_" + clipCounter;
		
		
		animation.AddClip(clip, animationName);
		animation.clip = animation[animationName].clip;


		//AssetDatabase.CreateAsset(clip, saveFolder + animationName + "_" + DateTime.Now.ToString("hh_mm_ss_dd_MM_yyyy") + ".anim");
		//AssetDatabase.SaveAssets();
		
		animation.Play();
		animation[animation.clip.name].speed = 0.0f;
		animation[animation.clip.name].wrapMode = WrapMode.Loop;

		//leapController.enabled = false;
		//for (int i = 0; i < leapObjects.Length; i++) {
		//	leapObjects[i].enabled = true;
		//}

		clipCounter += 1;
		recording = false;
	}

	public void Play(string clipName, float time)
	{
		if (!animation.IsPlaying(clipName)){
		    animation.Play(clipName);	
		    animation[clipName].speed = 0.0f;
		}
		animation[clipName].normalizedTime = time;
	}

	// void OnGUI(){

	// 	if(recording){
	// 		if (GUI.Button(new Rect(10, Screen.height - 40, 70, 20), "Stop")){
	// 			recording = false;
	// 			EndRecording();
	// 		}
	// 		GUI.Label(new Rect(90, Screen.height - 40, 70, 20), "" + countDown);
	// 		float t = 0.0f;
	// 		if(startTime > 0.0f){
	// 			t = Time.time - startTime;
	// 		}
	// 		GUI.HorizontalSlider(new Rect(180, Screen.height - 40, Screen.width-200, 30), 
	// 						t, 0.0F, recordTime);
	// 	} else {
	// 		// for(int i = 0; i < anims.Length; i++){
	// 		// 	if (GUI.Button(new Rect(120*i + 10, Screen.height - 80, 100, 20), anims[i].name)){
	// 		// 		recording = true;
	// 		// 		countDown = waitTime;
	// 		// 		recordTime = anims[i].length;
	// 		// 		triggerName = anims[i].name;
	// 		// 	}
	// 		// }

	// 		if(animation.clip != null)
	// 		{
	// 			if(playing){
	// 				if(GUI.Button(new Rect(90, Screen.height - 40, 70, 20), "Pause"))
	// 				{
	// 					playing = false;
	// 					animation[animation.clip.name].speed = 0.0f;
	// 				}
	// 			} else {
	// 				if(GUI.Button(new Rect(90, Screen.height - 40, 70, 20), "Play"))
	// 				{
	// 					playing = true;
	// 					animation[animation.clip.name].speed = 1.0f;
	// 				}
	// 			}
	// 			animation[animation.clip.name].time =  GUI.HorizontalSlider(new Rect(180, Screen.height - 40, Screen.width-200, 30), 
	// 						animation[animation.clip.name].time, 0.0F, animation[animation.clip.name].length);
	// 			if(animation[animation.clip.name].time > animation[animation.clip.name].length)
	// 				animation[animation.clip.name].time = animation[animation.clip.name].time 
	// 							% animation[animation.clip.name].length;
	// 		}
	// 	}

	// 	// if(GUI.Toggle(new Rect(10, Screen.height - 40, 50, 20), recording, "Record"))
	// 	// {
	// 	// 	if(recording == false){
	// 	// 		recording = true;
	// 	// 		StartRecording();
	// 	// 	}
	// 	// } else {
	// 	// 	if(recording == true){
	// 	// 		recording = false;
	// 	// 		EndRecording();
	// 	// 	}

	// 	// 	if(animation.clip != null)
	// 	// 	{
	// 	// 		if(GUI.Toggle(new Rect(70, Screen.height - 40, 50, 20), playing, "Play"))
	// 	// 		{
	// 	// 			if(playing == false){
	// 	// 				playing = true;

	// 	// 				animation[animation.clip.name].speed = 1.0f;
	// 	// 			}
	// 	// 		} else {
	// 	// 			if(playing == true){
	// 	// 				playing = false;
	// 	// 				animation[animation.clip.name].speed = 0.0f;
	// 	// 			}
	// 	// 		}
	// 	// 		animation[animation.clip.name].time =  GUI.HorizontalSlider(new Rect(150, Screen.height - 40, 100, 30), 
	// 	// 					animation[animation.clip.name].time, 0.0F, animation[animation.clip.name].length);
	// 	// 		if(animation[animation.clip.name].time > animation[animation.clip.name].length)
	// 	// 			animation[animation.clip.name].time = animation[animation.clip.name].time 
	// 	// 						% animation[animation.clip.name].length;
	// 	// 	}
	// 	// }
	// }
	
}
