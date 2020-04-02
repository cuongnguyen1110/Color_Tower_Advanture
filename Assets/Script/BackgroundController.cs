using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {

    public Transform mCameraTranform;

    private Vector3 mCameraOffset;
	// Use this for initialization
	void Start ()
    {
        mCameraOffset =  mCameraTranform.position - transform.position;
    }

    public void SetColor(Color TopColor, Color BottomColor)
    {

        //Set the main Color of the Material to green
        //  rend.material.shader = Shader.Find("_Color");
        // rend.material.SetColor("_Color", mColor);
        GetComponent<Renderer>().material.SetColor("_Color1", TopColor);
        GetComponent<Renderer>().material.SetColor("_Color", BottomColor);
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = mCameraTranform.position - mCameraOffset;
		
	}
}
