using UnityEngine;
using System.Collections;

[System.Serializable]
public class Handle
{
	public string name;
	public Transform joint;
	public Transform target;
	public AvatarIKGoal goal;
}

[System.Serializable]
public class RotationHandle
{
	public string name;
	public Transform joint;
	public Transform target;
}

public class IKHandle : MonoBehaviour {


    protected Animator animator;

	public Handle [] handles;
	public RotationHandle [] rotationHandles;

	public bool following;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(following){
			for(int i = 0; i < handles.Length; i++)
			{
				handles[i].target.position = handles[i].joint.position;
				handles[i].target.rotation = handles[i].joint.rotation;
			}
			for(int i = 0; i < rotationHandles.Length; i++)
			{
				rotationHandles[i].target.position = rotationHandles[i].joint.position;
				rotationHandles[i].target.rotation = rotationHandles[i].joint.rotation;
			}
			
		} 
	}

	void OnAnimatorIK()
    {
     	if(animator) {

     		if(!following){
	     		for(int i = 0; i < handles.Length; i++)
				{
					animator.SetIKPositionWeight(handles[i].goal,1.0f);
	                //animator.SetIKRotationWeight(handles[i].goal,1.0f);
	                //Debug.Log(handles[i].target.position + " " + handles[i].joint.position);
	                animator.SetIKPosition(handles[i].goal,handles[i].target.position);
	                //animator.SetIKRotation(handles[i].goal,handles[i].target.rotation);
				}
			}
			else {
				for(int i = 0; i < handles.Length; i++)
				{
					animator.SetIKPositionWeight(handles[i].goal,0);
                	animator.SetIKRotationWeight(handles[i].goal,0);    
				}
			}

            
        }
    }   

    void LateUpdate()
    {
    	if(!following){
    		for(int i = 0; i < rotationHandles.Length; i++)
			{
				rotationHandles[i].joint.rotation = rotationHandles[i].target.rotation;
			}
    	}
    } 
}
