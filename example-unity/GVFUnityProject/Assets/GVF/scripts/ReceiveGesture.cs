using UnityEngine;
using System.Collections;

public class ReceiveGesture : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Gesture1(float phase)
	{
		Debug.Log("gesture 1 " + phase);
	}

	void Gesture2(float phase)
	{
		Debug.Log("gesture 2 " + phase);
	}

	void Gesture3(float phase)
	{
		Debug.Log("gesture 3 " + phase);
	}

	void Gesture4(float phase)
	{
		Debug.Log("gesture 4 " + phase);
	}
}
