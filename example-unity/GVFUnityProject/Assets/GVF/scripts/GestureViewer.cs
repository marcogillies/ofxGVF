using UnityEngine;
using System.Collections;

public class GestureViewer : MonoBehaviour {

	public Vector3 [] points;
	private int currentPoint = 0;

	// Use this for initialization
	void Start () {
		ClearPoints();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ClearPoints()
	{
		currentPoint = 0;
	}

	public void AddPoint(Vector3 v){
		// if we've reached the end of the array
		// bunch up the existing points into the first half 
		// of the array
		//Debug.Log("current point " + currentPoint + " points.Length " + points.Length);
		if(currentPoint >= points.Length - 1){
			for(int i = 1; i < (points.Length-1)/2; i++){
				points[i] = points[2*i];
				//Debug.Log("setting point " + i);
			}
			currentPoint = (points.Length-1)/2;
		}
		for(int i = currentPoint; i < points.Length; i++)
		{
			points[i].Set(v.x, v.y, v.z);
		}
		currentPoint += 1;
		RedrawLines();
	}

	public void RedrawLines()
	{
		//Debug.Log("redrawing lines");
		for (int i = 0; i < transform.childCount-1 && i < points.Length-1; i++)
		{
			Vector3 v1 = points[i];
			Vector3 v2 = points[i+1];
			Vector3 difference = v2 - v1;
			//Debug.Log("setting child " + i);
			//Debug.Log(points[i]);
			Transform t = transform.GetChild(i);

			t.localScale = new Vector3(0.1f, 0.1f, difference.magnitude);
			Quaternion rot = new Quaternion();
			//rot.SetFromToRotation(Vector3.forward, difference);
			if(difference.magnitude > 0.00001){
				rot.SetLookRotation(difference);
				t.rotation = rot;
			}
			
			//t.position.Set(points[i].x, points[i].y, points[i].z);
			t.position = v1 + difference/2;
			//Debug.Log(t.position);
		}
	}
}
