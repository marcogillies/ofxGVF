using UnityEngine;
using System.Collections;

public class ParameterMapper : MonoBehaviour {

	public string xParameterName;
	int xParameterId;
	public float xParameterScale = 1.0f;
	public string yParameterName;
	int yParameterId;
	public float yParameterScale = 1.0f;
	public string zParameterName;
	int zParameterId;
	public float zParameterScale = 1.0f;

	public Animator animator;

	// Use this for initialization
	void Start () {
		xParameterId = Animator.StringToHash(xParameterName);
		Debug.Log(xParameterName + " " + xParameterId);
		yParameterId = Animator.StringToHash(yParameterName);
		Debug.Log(yParameterName + " " + yParameterId);
		zParameterId = Animator.StringToHash(zParameterName);
		Debug.Log(zParameterName + " " + zParameterId);
	}
	
	// Update is called once per frame
	void Update () {
		if(xParameterId != 0){
			animator.SetFloat(xParameterId, transform.position.x/xParameterScale);
		}
		if(yParameterId != 0){
			animator.SetFloat(yParameterId, transform.position.y/yParameterScale);
		}
		if(zParameterId != 0){
			animator.SetFloat(zParameterId, transform.position.z/zParameterScale);
		}
	}
}
