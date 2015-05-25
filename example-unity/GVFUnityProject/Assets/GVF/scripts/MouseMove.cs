using UnityEngine;
using System.Collections;

public class MouseMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(Input.GetAxis("Horizontal") + " " + Input.GetAxis("Vertical"));
		transform.Translate(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0));
	}
}
