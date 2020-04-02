using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainObjectContrller : MonoBehaviour {

    // Use this for initialization

    private float mLastTouch;
    private bool mIsTouchDown;

    public float mRotateSpd;

    private float mScaleRate;
    private bool mActive;


    void Start () {
        Debug.unityLogger.logEnabled = false;
        mActive = false;
        mLastTouch = 0;
        mIsTouchDown = false;
        float width = (float)Screen.width;
        mScaleRate = 1080.0f / width ;

    }

    public void SetActive(bool isActive)
    {
        mActive = isActive;
    }

    public void Restart()
    {
        mIsTouchDown = false;
        mLastTouch = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log("------------------FPS: " + (1.0f / Time.deltaTime) );
        if(!mActive)
        {
            return;
        }

        if(mIsTouchDown)
        {
            Vector3 mousePos = Input.mousePosition;
            mLastTouch = - (mousePos.x - mLastTouch);
            //Debug.Log("===========mLastTouch :"+ mLastTouch);
            if (mLastTouch != 0)
            {
                //transform.rotation = Quaternion.Euler(0f, 0f,  90);
                //transform.rotation = Quaternion.AngleAxis(mLastTouch*mRotateSpd * Time.deltaTime, Vector3.up);
                transform.Rotate(Vector3.up * mLastTouch * mRotateSpd * mScaleRate * Time.deltaTime, Space.Self);
            }
            mLastTouch = mousePos.x;

        }
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("========click===========");
            Vector3 mousePos = Input.mousePosition;
            mLastTouch = mousePos.x;

            mIsTouchDown = true;


        }

        if (Input.GetMouseButtonUp(0))
        {
            mIsTouchDown = false;
            mLastTouch = 0;
            // Debug.Log("========Mouse up===========");
        }

    }
}
