using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class GVF : MonoBehaviour {
	//Lets make our calls from the Plugin
	[DllImport ("GVFUnity")]
	private static extern int PrintANumber();
	
	[DllImport ("GVFUnity")]
	private static extern IntPtr PrintHello();
	
//	[DllImport ("GVFUnity")]
//	private static extern int AddTwoIntegers(int i1,int i2);
//
//	[DllImport ("GVFUnity")]
//	private static extern float AddTwoFloats(float f1,float f2);	
//	

	[DllImport ("GVFUnity")]
	private static extern void setDimensions(int dim);
	[DllImport ("GVFUnity")]
	private static extern void setMultiPoint(bool multipoint);
	[DllImport ("GVFUnity")]
	private static extern void setSegmentation(bool segmentation);
	[DllImport ("GVFUnity")]
	private static extern void setRotationFeatures(bool rotationFeatures);

	[DllImport ("GVFUnity")]
	private static extern void initGVF();	
	[DllImport ("GVFUnity")]
	private static extern void addObservation(float [] data, int numItems);

	[DllImport ("GVFUnity")]
	private static extern int  getNumObservations(int gestureNum);
    [DllImport ("GVFUnity")]
	private static extern void getObservation(int gestureNum, int observationNum, float [] data);
    [DllImport ("GVFUnity")]
	private static extern void getObservationZeroOrigin(int gestureNum, int observationNum, float [] data);
    

    [DllImport ("GVFUnity")]
	public static extern int  getNumObservationsCurrentGestures();
    [DllImport ("GVFUnity")]
	public static extern void  getObservationCurrentGestures(int observationNum, float [] data);

    [DllImport ("GVFUnity")]
	private static extern void infer();

    [DllImport ("GVFUnity")]
	public static extern int getNumberOfGestureTemplates();
    
    [DllImport ("GVFUnity")]
	private static extern int getMostProbable();
    [DllImport ("GVFUnity")]
	private static extern float getProbability(int i);
    [DllImport ("GVFUnity")]
	private static extern float getPhase(int i);
    [DllImport ("GVFUnity")]
	private static extern float getSpeed(int i);
    [DllImport ("GVFUnity")]
	private static extern float getScale(int i);
    [DllImport ("GVFUnity")]
	private static extern float getRotation(int i);
//    void getScale(float *s);
//    void getRotation(float *r);
//    
    [DllImport ("GVFUnity")]
	private static extern void startLearning();
    [DllImport ("GVFUnity")]
	private static extern void endLearning();
    [DllImport ("GVFUnity")]
	private static extern void setGestureToCurrent(int index);
	[DllImport ("GVFUnity")]
	private static extern bool isLearning();

    [DllImport ("GVFUnity")]
	private static extern void startFollowing();
    [DllImport ("GVFUnity")]
	private static extern void endFollowing();
    [DllImport ("GVFUnity")]
	private static extern bool isFollowing();

    
    [DllImport ("GVFUnity")]
	private static extern void  setTolerance(float v);
    [DllImport ("GVFUnity")]
	private static extern float getTolerance();
    [DllImport ("GVFUnity")]
	private static extern void  setScaleVariance(float v);
    [DllImport ("GVFUnity")]
	private static extern float getScaleVariance();
    [DllImport ("GVFUnity")]
	private static extern void  setSpeedVariance(float v);
    [DllImport ("GVFUnity")]
	private static extern float getSpeedVariance();

	public enum State {nothing, learning, following};
	public State state = State.nothing;

	public Transform [] trackedObjects;
	public float [] data;

	//public Slider [] sliders;
	//public Text stateElement;

	public GestureViewer [] CurrentGestureViewers;
	public Transform [] GestureThumbnailHolders;

	public GameObject [] gestureViewerPrefabs;

	//private LineRenderer [][] GestureViewers;
	private int numbObservations = 0;

	private Vector3 [] gestureStartPoint;

	private int dimensionsPerPoint;

	public int GetNumGestures()
	{
		return getNumberOfGestureTemplates();
	}

	public int GetRecognisedGesture()
	{
		return getMostProbable();
	}

	public float GetTimeInGestureNormalized()
	{
		int mostProbable = getMostProbable();
		if(mostProbable >= 0)
			return getPhase(mostProbable);
		else 
			return 0.0f;
	}

	public float GetBestProbability()
	{
		int mostProbable = getMostProbable();
		if(mostProbable >= 0)
			return getProbability(mostProbable);
		else 
			return 0.0f;
	}

	void Start () {
		state = State.nothing;
		setMultiPoint(true);
		setSegmentation(true);
		setRotationFeatures(true);
		dimensionsPerPoint = 6; // 6 if we include rotation, 3 if not
		setDimensions (dimensionsPerPoint*trackedObjects.Length);
		initGVF ();

	}

	public void BeginLearning()
	{
		// start learning
    	state = State.learning;
    	startLearning();
    	numbObservations = 0;
    	if(gestureStartPoint == null || gestureStartPoint.Length < trackedObjects.Length)
    		gestureStartPoint = new Vector3[trackedObjects.Length];
    	for (int i = 0; i < trackedObjects.Length; i++)
    	{
    		gestureStartPoint[i] = new Vector3(trackedObjects[i].position.x,
    											trackedObjects[i].position.y,
    											trackedObjects[i].position.z);
    	}
		for(int i = 0; i < trackedObjects.Length && i < CurrentGestureViewers.Length; i++)
		{
			GestureViewer line = CurrentGestureViewers[i].GetComponent<GestureViewer>();
			line.ClearPoints();
		}

	}

	public void FinishLearning(int index = -1)
	{
		Debug.Log("end learning " + index);
		//Debug.Log("num observations " + getNumObservationsCurrentGestures());
    	state = State.nothing;
    	if(index < 0)
    	{
    		Debug.Log("calling endLearning");
	    	endLearning();
    		Debug.Log("finished calling endLearning");
    	}
	    else
	    {
    		Debug.Log("calling setGestureToCurrent");
	    	setGestureToCurrent(index);
	    }


		int numGestures = getNumberOfGestureTemplates();
		//Debug.Log("num observations " + getNumObservations(numGestures-1));
		if(numGestures <= GestureThumbnailHolders.Length)
     	{
     		for(int i = 0; i < trackedObjects.Length && i < CurrentGestureViewers.Length; i++)
			{
				//GameObject viewer = (GameObject) Network.Instantiate(gestureViewerPrefabs[i], GestureThumbnailHolders[numGestures-1].position, GestureThumbnailHolders[numGestures-1].rotation,0);
				GameObject viewer = (GameObject) Instantiate(gestureViewerPrefabs[i], GestureThumbnailHolders[numGestures-1].position, GestureThumbnailHolders[numGestures-1].rotation);
				
				viewer.transform.SetParent(GestureThumbnailHolders[numGestures-1]);
				
				GestureViewer line = viewer.GetComponent<GestureViewer>();
				
				int numbObservations = getNumObservations(numGestures-1);
				Vector3 startPos = GestureThumbnailHolders[numGestures-1].position;
				//Debug.Log(line.points.Length);
				for (int j = 0; j < line.points.Length; j++){
					getObservation(numGestures-1, j*numbObservations/line.points.Length, data);
					line.points[j].x = 0.2f*data[dimensionsPerPoint*i+0]+startPos.x;
					line.points[j].y = 0.2f*data[dimensionsPerPoint*i+1]+startPos.y;
					line.points[j].z = 0.2f*data[dimensionsPerPoint*i+2]+startPos.z;
					//Debug.Log(data[dimensionsPerPoint*i+0] + " "
					//	+ data[dimensionsPerPoint*i+1] + " "
					//	+ data[dimensionsPerPoint*i+2]);
					//Debug.Log("line points " + line.points[j]);
				}
				line.RedrawLines();

	     	}
     	}
	}

	public void ToggleLearning()
	{
		if(state == State.learning){
			FinishLearning();
        } else {
			BeginLearning();
        }
	}

	public void BeginFollowing()
	{
		// Debug.Log("about To Start Following");
     	state = State.following;
     	startFollowing();
     	numbObservations = 0;

    	if(gestureStartPoint == null || gestureStartPoint.Length < trackedObjects.Length)
    		gestureStartPoint = new Vector3[trackedObjects.Length];
    	for (int i = 0; i < trackedObjects.Length; i++)
    	{
    		gestureStartPoint[i] = new Vector3(trackedObjects[i].position.x,
    											trackedObjects[i].position.y,
    											trackedObjects[i].position.z);
    	}

		for(int i = 0; i < trackedObjects.Length && i < CurrentGestureViewers.Length; i++)
		{
			GestureViewer line = CurrentGestureViewers[i].GetComponent<GestureViewer>();
			line.ClearPoints();
		}
	}

	public void FinishFollowing()
	{
		state = State.nothing;
        endFollowing();
	}

	public void ToggleFollowing()
	{
		if(state == State.following){
        	FinishFollowing();
        } else {
        	BeginFollowing();
        }
	}

	void Update(){
		//stateElement.text = state.ToString();
		
		
		if(state == State.learning || state == State.following){
			// 3 or 6 per point, depending on whether you include rotation
			//data = new float[3*trackedObjects.Length];
			data = new float[dimensionsPerPoint*trackedObjects.Length];
			for (int i = 0; i < trackedObjects.Length; i++)
			{
				data[dimensionsPerPoint*i]   = trackedObjects[i].position.x;
				data[dimensionsPerPoint*i+1] = trackedObjects[i].position.y;
				data[dimensionsPerPoint*i+2] = trackedObjects[i].position.z;
				if(dimensionsPerPoint == 6)
				{
					// yuck Euler angles!
					data[dimensionsPerPoint*i+3] = trackedObjects[i].rotation.eulerAngles.x;
					data[dimensionsPerPoint*i+4] = trackedObjects[i].rotation.eulerAngles.y;
					data[dimensionsPerPoint*i+5] = trackedObjects[i].rotation.eulerAngles.z;
				}
			}
			//Debug.Log(data);
			addObservation(data, data.Length);
			//Debug.Log("num observations " + getNumObservationsCurrentGestures());
			numbObservations++;
			for(int i = 0; i < trackedObjects.Length && i < CurrentGestureViewers.Length; i++)
        	{
				GestureViewer line = CurrentGestureViewers[i].GetComponent<GestureViewer>();
				
				getObservationCurrentGestures(0, data);
				//Debug.Log("first observation " + data[3*i+0] + " " + data[3*i+1] + " " + data[3*i+2]);
				
				int totalObservations = getNumObservationsCurrentGestures();
				//Vector3 startPos = new Vector3(0, 0, 0);//GestureThumbnailHolders[numGestures-1].position;
				for (int j = 0; j < line.points.Length; j++){
					getObservationCurrentGestures(j*totalObservations/line.points.Length, data);
					line.points[j].x = data[dimensionsPerPoint*i+0]+gestureStartPoint[i].x;
					line.points[j].y = data[dimensionsPerPoint*i+1]+gestureStartPoint[i].y;
					line.points[j].z = data[dimensionsPerPoint*i+2]+gestureStartPoint[i].z;
				}
				line.RedrawLines();

			}
		}
		if(state == State.following){
			
			infer();

			// int mostProbable = GetRecognisedGesture();
			// if(mostProbable >= 0)
			// {
			// 	Debug.Log("speed " + getSpeed(mostProbable));
   //       		Debug.Log("scale " + getScale(mostProbable));
   //       		Debug.Log("rotation " + getRotation(mostProbable));
			// }
			// int mostProbable = getMostProbable();
			// Debug.Log("most probable " + getMostProbable());
			
        	for(int i = 0; i < GestureThumbnailHolders.Length && i < getNumberOfGestureTemplates(); i++)
        	{
				Component[] meshes = GestureThumbnailHolders[i].GetComponentsInChildren<MeshRenderer>();
				float probability = getProbability(i);
				if(probability < 0){
					continue;
				}
				foreach (MeshRenderer mesh in meshes) {
					Color textureColor = mesh.renderer.material.color;
	        		//Debug.Log("texture color " + textureColor);
	        		textureColor.a = probability;
					//Debug.Log("prob " + getProbability(i));
					mesh.renderer.material.color = textureColor;
		        }
        	}
        	
		}
		
	}
	
}
