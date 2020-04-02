using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eItemType
{
    E_ITEM_RED,
    e_ITEM_BLUE
}

public class ItemController : MonoBehaviour {


    private Vector3 mSpeed;
    private float mGravity;
    private bool mIsAlive;

    private GameObject mPlayer;
    public int mId;

    public eItemType mType;

    public bool mIsMoveable;

    public float M_Rotate_Dege;

    public float mRotateSpeed;

    private float mCurrentRotate;

    private Color mColor;
    private PlayerController mPlayerCtl;

    private bool mIsObsObject;
    private bool mActiveAnim;

    public Material mDeathMat;
    public Material mAliveMat;

    ItemController()
    {
        mCurrentRotate = 0.0f;
        mIsAlive = true;
        mSpeed = new Vector3(3, 1, 3);
        mGravity = -0.5f;
        mIsObsObject = false;
        mActiveAnim = false;
    }

    // Use this for initialization
    void Start ()
    {
       // mRotateSpeed = 12.0f;
       

        mPlayer = GameObject.Find("Player");
        if (null == mPlayer)
        {
            Debug.Log("WARNING!!!!!! ItemController Can not find Sphere object");
        }
        else
        {
            mPlayerCtl = mPlayer.GetComponent<PlayerController>();
        }

        if(GetComponent<Animator>())
        {
            if (mActiveAnim)
            {
                GetComponent<Animator>().enabled = true;
            }
            else
            {
                GetComponent<Animator>().enabled = false;
            }
        }
        

        if(mIsObsObject)
        {
            //ActiceDeathMat();
        }
    }
    
    public void SetObsObject(bool isObs)
    {
        mIsObsObject = isObs;
    }

    public void ActiveAnim(bool active)
    {
        mActiveAnim = active;

        if (GetComponent<Animator>())
        {
            if (mActiveAnim)
            {
                GetComponent<Animator>().enabled = true;
            }
            else
            {
                GetComponent<Animator>().enabled = false;
            }
            //Debug.Log("===== ActiveAnim : " + active);

            //Animation myAnimation = GetComponent<Animation>();

            //Animator animator = GetComponent<Animator>();
            
            //if(myAnimation)
            //{
            //    Debug.Log("----Found the anim: " + myAnimation.name);
            //    AnimationState animState = myAnimation.GetComponent<AnimationState>();
            //    if(animState)
            //    {
            //        Debug.Log(" === Found State anim : " + animState.name);
            //    }
            //}
        }
    }

    public void Setup(eItemType type)
    {
        mType = type;
        if(mType == eItemType.e_ITEM_BLUE)
        {
          //  mIsMoveable = false;
        }
    }

    public void SetEmmissionColor(Color c)
    {
        Renderer rend = GetComponent<Renderer>();
        //rend.material.shader = Shader.Find("Specular");
        if(rend)
        {
            rend.material.SetColor("_EmissionColor", c);
        }
        else if (gameObject.name == "Obtascal_2_cube_01(Clone)" || gameObject.name == "Obtascal_2_cube_02(Clone)")
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                GameObject child = gameObject.transform.GetChild(i).gameObject;
                Renderer childRend = child.GetComponent<Renderer>();
                if (childRend)
                {
                    childRend.material.SetColor("_EmissionColor", c);
                }
            }
        }
    }


    public void ActiveAliveMat()
    {
        
        if (mAliveMat)
        {
            Renderer rend = GetComponent<Renderer>();
            if (rend)
            {
                rend.sharedMaterial = mAliveMat;
            }
            else if (gameObject.name == "Obtascal_2_cube_01(Clone)" || gameObject.name == "Obtascal_2_cube_02(Clone)")
            {
                for (int i = 0; i < gameObject.transform.childCount; i++)
                {
                    GameObject child = gameObject.transform.GetChild(i).gameObject;
                    Renderer childRend = child.GetComponent<Renderer>();
                    if (childRend)
                    {
                        childRend.sharedMaterial = mAliveMat;
                    }
                }
            }
        }
    }
    public void ActiceDeathMat()
    {
        if(mDeathMat)
        {
            Renderer rend = GetComponent<Renderer>();
            if(rend)
            {
                rend.sharedMaterial = mDeathMat;
            }
            else if(gameObject.name == "Obtascal_2_cube_01(Clone)" || gameObject.name == "Obtascal_2_cube_02(Clone)")
            {
                for(int i = 0; i < gameObject.transform.childCount; i++ )
                {
                    GameObject child = gameObject.transform.GetChild(i).gameObject;
                    Renderer childRend = child.GetComponent<Renderer>();
                    if(childRend)
                    {
                        childRend.sharedMaterial = mDeathMat;
                    }
                }
            }
        }
            
    }

    public void SetColor(Color color)
    {

        return;
        if(mType == eItemType.e_ITEM_BLUE)
        {
            //Debug.Log(" Color of BLUE item: " + color);
        }
        mColor = color;
        Renderer rend = gameObject.GetComponent<Renderer>();
        if(rend)
        {
            Material mat = rend.material;
            mat.color = mColor;
        }
        else
        {
            int childCount = gameObject.transform.childCount;
            for(int i = 0; i < childCount; i++)
            {
                GameObject childObj = gameObject.transform.GetChild(i).gameObject;
                Renderer objRend = childObj.GetComponent<Renderer>();
                if (objRend)
                {
                    Material mat = objRend.material;
                    mat.color = mColor;
                }
            }
        }
        
        //Renderer rend = gameObject.GetComponent<Renderer>();
        //Material mat = rend.material;
        //mat.color = mColor;

        //Set the main Color of the Material to green
        //  rend.material.shader = Shader.Find("_Color");
        // rend.material.SetColor("_Color", mColor);
    }

    public eItemType getType()
    {
        return mType;
    }

    public bool IsAlive()
    {
        return mIsAlive;
    }

    public void Die()
    {
        if(gameObject.GetComponent<BoxCollider>())
            gameObject.GetComponent<BoxCollider>().gameObject.SetActive(false);
        
        mIsAlive = false;
        int numberOfChild = transform.childCount;
        for(int i = 0; i < numberOfChild; i++)
        {
            GameObject childGO = transform.GetChild(i).gameObject;
            BoxCollider boxCol = childGO.GetComponent<BoxCollider>();
            if(boxCol)
            {
                boxCol.gameObject.SetActive(false);
            }
            ItemController item = childGO.GetComponent<ItemController>();
            if(item)
            {
                item.Die();
            }
        }
        float rotate = transform.rotation.eulerAngles.y;
        rotate = rotate - ((mId * (360 / 12)) / 2);
        //Debug.Log("ItemController:" + gameObject.name + ". Rotate = " + transform.rotation.eulerAngles);
        rotate = Mathf.Deg2Rad * rotate;
        mSpeed.x *= Mathf.Sin(rotate);
        mSpeed.z *= Mathf.Cos(rotate);
        gameObject.transform.parent = null;
    }

    public void SetRotation(float degree, float speed)
    {
        mIsMoveable = true;
        M_Rotate_Dege = degree;
        mRotateSpeed = speed;
    }

    // Update is called once per frame
    void Update ()
    {
        if(mIsAlive)
        {
            if (transform.position.y > mPlayer.transform.position.y && !mIsObsObject)
            {
                Die();
            }
        }
        
        if(!mIsAlive)
        {
            mSpeed.y += mGravity;
            Vector3 possition = transform.position;
            possition += mSpeed * Time.deltaTime;
            transform.position = possition;
            Destroy(gameObject, 1);
        }


       
		
	}

    void FixedUpdate()
    {
        if(mPlayerCtl && !mPlayerCtl.isDie())
        {
            if (mIsMoveable)
            {
                transform.Rotate(Vector3.back * mRotateSpeed * Time.deltaTime, Space.Self);
                mCurrentRotate += mRotateSpeed * Time.deltaTime;
                if (mCurrentRotate > M_Rotate_Dege || mCurrentRotate < 0)
                {
                    mRotateSpeed *= -1;
                }
            }
        }
        
    }
}
