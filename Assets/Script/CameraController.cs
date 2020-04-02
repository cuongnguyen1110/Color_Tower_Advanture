using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    // Use this for initialization
    public GameObject mPlayer;
    private Vector3 mInititalPossition;

    private bool mIsFollowing;

    private Vector3 mTarget;
    public float mSpeed;

    

	void Start ()
    {
        Debug.unityLogger.logEnabled = false;

        mInititalPossition = new Vector3(0, -0.48f, -4.15f);
        mTarget = transform.position;
        // mSpeed = -1.9f;
        float playerVelocity = mPlayer.GetComponent<Rigidbody>().velocity.y;
        if (playerVelocity < 0)
            mSpeed = playerVelocity;

    }

    public void Reset()
    {
        transform.position = mInititalPossition;
        mTarget = transform.position;

    }

    public void FollowMe(Vector3 possition)
    {
        
        //if(!mIsFollowing)
        {

            mIsFollowing = true;
            mTarget.y -= 1.4f;
        }
        Debug.Log("===========FollowMe=========== " + mTarget.y); 
    }

    //public void StopFollowMe()
    //{
    //    mIsFollowing = false;
    //    mPlayerDelta = Vector3.zero;
    //}
	
	// Update is called once per frame
	void Update ()
    {
        if (mIsFollowing)
        {
            if (transform.position.y > mTarget.y)
            {
                float playerVelocity = mPlayer.GetComponent<Rigidbody>().velocity.y;
                if (playerVelocity < 0)
                    mSpeed = playerVelocity;
                Vector3 pos = transform.position;
                pos.y += mSpeed * Time.deltaTime;
                if(pos.y < mTarget.y)
                {
                    pos.y = mTarget.y;
                }
                transform.position = pos;
               
            }
            else
            {
                Debug.Log("===========FollowMe - false =========== " + mTarget.y);
                transform.position = mTarget;
                mSpeed = -3.3f;
                mIsFollowing = false;
            }
        }

    }

    void LateUpdate()
    {
        
    }
}
