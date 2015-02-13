using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class GVF : MonoBehaviour {
	//Lets make our calls from the Plugin
	[DllImport ("ASimplePlugin")]
	private static extern int PrintANumber();
	
	[DllImport ("ASimplePlugin")]
	private static extern IntPtr PrintHello();
	
//	[DllImport ("ASimplePlugin")]
//	private static extern int AddTwoIntegers(int i1,int i2);
//
//	[DllImport ("ASimplePlugin")]
//	private static extern float AddTwoFloats(float f1,float f2);	
//	

	[DllImport ("ASimplePlugin")]
	private static extern void setDimensions(int dim);
	[DllImport ("ASimplePlugin")]
	private static extern void initGVF();	
	[DllImport ("ASimplePlugin")]
	private static extern void addObservation(float [] data, int numItems);

	[DllImport ("ASimplePlugin")]
	private static extern int  getNumObservations(int gestureNum);
    [DllImport ("ASimplePlugin")]
	private static extern void getObservation(int gestureNum, int observationNum, float [] data);
    [DllImport ("ASimplePlugin")]
	private static extern void getObservationZeroOrigin(int gestureNum, int observationNum, float [] data);
    
    [DllImport ("ASimplePlugin")]
	private static extern void infer();

    [DllImport ("ASimplePlugin")]
	private static extern int getNumberOfGestureTemplates();
    
    [DllImport ("ASimplePlugin")]
	private static extern int getMostProbable();
    [DllImport ("ASimplePlugin")]
	private static extern float getProbability(int i);
    [DllImport ("ASimplePlugin")]
	private static extern float getPhase(int i);
    [DllImport ("ASimplePlugin")]
	private static extern float getSpeed(int i);
    [DllImport ("ASimplePlugin")]
	private static extern float getScale(int i);
    [DllImport ("ASimplePlugin")]
	private static extern float getRotation(int i);
//    void getScale(float *s);
//    void getRotation(float *r);
//    
    [DllImport ("ASimplePlugin")]
	private static extern void startLearning();
    [DllImport ("ASimplePlugin")]
	private static extern void endLearning();
	[DllImport ("ASimplePlugin")]
	private static extern bool isLearning();

    [DllImport ("ASimplePlugin")]
	private static extern void startFollowing();
    [DllImport ("ASimplePlugin")]
	private static extern void endFollowing();
    [DllImport ("ASimplePlugin")]
	private static extern bool isFollowing();

    
    [DllImport ("ASimplePlugin")]
	private static extern void  setTolerance(float v);
    [DllImport ("ASimplePlugin")]
	private static extern float getTolerance();
    [DllImport ("ASimplePlugin")]
	private static extern void  setScaleVariance(float v);
    [DllImport ("ASimplePlugin")]
	private static extern float getScaleVariance();
    [DllImport ("ASimplePlugin")]
	private static extern void  setSpeedVariance(float v);
    [DllImport ("ASimplePlugin")]
	private static extern float getSpeedVariance();

	public enum State {nothing, learning, following};
	public State state = State.nothing;

	public Transform [] trackedObjects;
	public float [] data;

	public Slider [] sliders;
	public Text stateElement;

	public LineRenderer CurrentGestureViewer;
	public LineRenderer [] GestureViewers;
	private int numbObservations = 0;
	
	void Start () {
		state = State.nothing;
		setDimensions (3*trackedObjects.Length);
		initGVF ();

		for(int i = 0; i < sliders.Length; i++)
    	{
    		sliders[i].gameObject.SetActive(false);
    	}
    	for(int i = 0; i < GestureViewers.Length; i++)
    	{
    		GestureViewers[i].gameObject.SetActive(false);
    	}
		//float [] data = new float[5];
		//for(int i = 0; i < data.Length; i++)
		//	data [i] = i;
		//addObservation (data, 5);
		//float [] newdata = new float[5];
		//getObservation (newdata);
		//Debug.Log (newdata);
		//for (int i = 0; i < 5; i++)
		//	Debug.Log (newdata [i]);

		//Debug.Log(PrintANumber());
		//Debug.Log(Marshal.PtrToStringAuto (PrintHello()));
//		Debug.Log(AddTwoIntegers(2,2));
//		Debug.Log(AddTwoFloats(2.5F,4F));
	}

	void Update(){
		stateElement.text = state.ToString();
		if (Input.GetKeyDown("l")){
            if(state == State.learning){
            	Debug.Log("end learning");
            	state = State.nothing;
            	endLearning();

            	int numGestures = getNumberOfGestureTemplates();
            	if(numGestures <= GestureViewers.Length)
            	{
			    	GestureViewers[numGestures-1].gameObject.SetActive(true);
			    	GestureViewers[numGestures-1].SetVertexCount(getNumObservations(numGestures));
					
			    	for (int i = 0; i < getNumObservations(numGestures-1); i++)
			    	{
			    		getObservationZeroOrigin(numGestures-1, i, data);
			    		Vector3 startPos = GestureViewers[numGestures-1].gameObject.transform.position;
			    		GestureViewers[numGestures-1].SetPosition(i, 
							new Vector3(data[0]+startPos.x, data[1]+startPos.y, data[2]+startPos.z));
			    	}
            	}
            } else {
            	state = State.learning;
            	startLearning();
            	numbObservations = 0;
            	for(int i = 0; i < sliders.Length; i++)
            	{
            		sliders[i].gameObject.SetActive(false);
            	}
            }
		}
		if (Input.GetKeyDown("f")){
            if(state == State.following){
            	state = State.nothing;
            	endFollowing();
            } else {
            	Debug.Log("about To Start Following");
            	state = State.following;
            	startFollowing();
            	numbObservations = 0;
            	for(int i = 0; i < sliders.Length && i < getNumberOfGestureTemplates(); i++)
            	{
            		sliders[i].gameObject.SetActive(true);
            	}
            }
		}
		if(state == State.learning || state == State.following){
			/*float [] */ data = new float[3*trackedObjects.Length];
			for (int i = 0; i < trackedObjects.Length; i++)
			{
				data[3*i]   = trackedObjects[i].position.x;
				data[3*i+1] = trackedObjects[i].position.y;
				data[3*i+2] = trackedObjects[i].position.z;
			}
			Debug.Log(data);
			addObservation(data, data.Length);
			numbObservations++;
			CurrentGestureViewer.SetVertexCount(numbObservations);
			CurrentGestureViewer.SetPosition(numbObservations-1, 
				new Vector3(data[0], data[1], data[2]));
		}
		if(state == State.following){
			//Debug.Log("about to infer");
			infer();
			//Debug.Log("about to get most probable");
			Debug.Log("most probable " + getMostProbable());
			for(int i = 0; i < sliders.Length && i < getNumberOfGestureTemplates(); i++)
        	{
        		sliders[i].value = getProbability(i);
        	}
		}
	}
	/*
	void OnGUI()
	{
		 GUI.Label(new Rect(10, 10, 100, 20), state.ToString());
		 if(state == State.following){
		 	GUI.Label(new Rect(120, 10, 100, 20), "most probable " + getMostProbable());
		 	for (int i = 0; i < getNumberOfGestureTemplates(); i++){
		 		GUI.Label(new Rect(120, 60+50*i, 40, 20), getProbability(i).ToString());
		 		GUI.Label(new Rect(160, 60+50*i, 40, 20), getPhase(i).ToString());
		 		GUI.Label(new Rect(200, 60+50*i, 40, 20), getSpeed(i).ToString());
		 	}
		 }
	}
	*/
}
