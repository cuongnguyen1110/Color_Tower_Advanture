using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class PlayerController : MonoBehaviour {

    // Use this for initialization

    public float SPEED_MIN;

    private const int POINT_VALUE = 12;
    private const int REWARD_POINT_VALUE = 15;

    public GameObject mMainCamera;
    private CameraController mCamController;

    private Vector3 mLowestPos;

    private int mNumberOfPerfect;

    private bool mIsPowerMode;

    public Rigidbody mRigid;
    public float FORCE;
    public GameObject mExploseEffect;
    public GameObject mTrailEffect;
    public GameObject mPower;
    public GameObject mPowerExploseEffect;


    public UnityEngine.Canvas mGui;
    public GameObject mAddPointTextObj;
    public GameObject mPerfectTextObj;

    private GamePlayMng mGame;
    private Vector3 mInitPos;

    private Color mColor;

    private bool mIsDie;

    //=========== Sound======================
    public AudioClip mSoundColider;
    public AudioClip mSoundGetPoint;
    public AudioClip mSoundEndGame;
    public AudioClip mSoundPowerUp;


    public AudioSource source;
    private float mSoundVol;
    private int SoundActive;

    private GameObject mLastColisionObj;
    //=======================================

    public List<Mesh> mListMesh;
    public List<Material> mListMaterial;

    private int mIdentifyId;

    PlayerController()
    {
        mLowestPos = Vector3.down * 2;
        mIdentifyId = -1;
    }

    public bool isDie()
    {
        return mIsDie;
    }
    public void SetColor(Color color)
    {

        //return;
        //mColor = color;


        //Renderer rend = gameObject.GetComponent<Renderer>();
        //Material mat = rend.material;
        //mat.color = color;

        //mat = mPower.gameObject.GetComponent<Renderer>().material;
        //Color alPhaColor = color; // new Color();
        //alPhaColor.a = 0.25f;
        //mat.color = alPhaColor;

        //TrailRenderer trail = mTrailEffect.gameObject.GetComponent<TrailRenderer>();

        //// Set the color of the material to tint the trail.
        //trail.startColor = color;

    }

    void Start ()
    {
        Debug.unityLogger.logEnabled = false;
        // SetColor(Color.red);
        mIsDie = false;
        mSoundVol = 1.0f;// Random.Range(volLowRange, volHighRange);
        SoundActive = PlayerPrefs.GetInt("SoundSetting", 1);
        // mRigid.gameObject.SetActive(false);
        mInitPos = new Vector3( -0.01f,-1.02f, -0.85f);
        mNumberOfPerfect = 0;
        mIsPowerMode = false;
        mLowestPos = Vector3.down * 2 ;
        mPower.gameObject.SetActive(false);
        mCamController = mMainCamera.GetComponent<CameraController>();
        if(null == mCamController)
        {
            Debug.Log("WARNING!!!!! can not get cameracontroller");
        }


        GameObject gObject = GameObject.Find("GamePlayMng");
        if (gObject)
        {
            mGame = gObject.GetComponent<GamePlayMng>();
            if (null == mGame)
            {
                Debug.Log("WARNING!!!!!!!!! can not find mGame object");
            }
        }

    }

    public void Reset()
    {
        mIsDie = false;
        mPower.gameObject.SetActive(false);
        transform.position = mInitPos;
        mRigid.velocity = Vector3.zero;
        //mTrailEffect.gameObject.SetActive(true);
        mTrailEffect.gameObject.GetComponent<TrailRenderer>().Clear();

        mIsPowerMode = false;
        mPower.gameObject.SetActive(false);
    }

    public void SetMesh(Mesh m)
    {
        GetComponent<MeshFilter>().sharedMesh = m;
    }

    public void SetMaterial(Material mat)
    {
        GetComponent<Renderer>().sharedMaterial = mat;
    }

    public void SetPlayerDef(int defId)
    { 
        Mesh mesh = mListMesh[defId];
        SetMesh(mesh);
        Material mat = mListMaterial[defId];
        SetMaterial(mat);
    }

    public void RandomDef()
    {
        bool done = false;
        do
        {
            int defId = Random.Range(0, 5);
            if(defId != mIdentifyId)
            {
                mIdentifyId = defId;
                done = true;
                Debug.Log("Random def: " + defId);
            }
            
            
        }
        while (!done);

        SetPlayerDef(mIdentifyId);

    }

    public void StartPlay()
    {
        // mRigid.gameObject.SetActive(true);
        //SoundActive = PlayerPrefs.GetInt("SoundSetting", 1);
        //gameObject.
    }


	// Update is called once per frame
	void Update ()
    {

        //if (mIsDie)
        //    return;

        if (transform.position.y < mLowestPos.y)
        {
            mLowestPos.y = mLowestPos.y - 1.4f;
            mCamController.FollowMe(transform.position);
            mNumberOfPerfect++;
            mGame.AddPoint(POINT_VALUE, false);
            // play sound
            if(SoundActive == 1)
            {
                source.PlayOneShot(mSoundGetPoint, mSoundVol * 2);
            }
            // ==============gen tex===================
            GameObject effect = Instantiate(mAddPointTextObj, mAddPointTextObj.transform.position, mAddPointTextObj.transform.rotation);
            UnityEngine.UI.Text textPoint = effect.gameObject.GetComponent<Text>();
            textPoint.text = "+" + (POINT_VALUE * mNumberOfPerfect);
            effect.transform.SetParent(mGui.transform);
            Destroy(effect, 1);
            //=================================================

            //  Debug.Log("===========FixedUpdate=========== " + mNumberOfPerfect);
            if (mNumberOfPerfect > 2)
            {
                mPower.gameObject.SetActive(true);
                mIsPowerMode = true;
            }

        }


    }

    void FixedUpdate()
    {
        if (mIsDie)
            return;
        if (mRigid.velocity.y < SPEED_MIN)
        {
            // Debug.Log(" Limite velocity :" + mRigid.velocity);
            Vector3 v = mRigid.velocity; // *= 0.95f;
            v.y = SPEED_MIN;
            mRigid.velocity = v;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (mIsDie)
            return;

        //if (other.gameObject.name != "Cube(Clone)" && mRigid.velocity.y > 0.5f)
        //    return;

        mLastColisionObj = other.gameObject;
        ItemController item = other.gameObject.GetComponent<ItemController>();
        if(other.gameObject.name == "New_cube (1)" || other.gameObject.name == "New_cube (2)")
        {
            if(!item)
            {
                item = other.gameObject.transform.parent.gameObject.GetComponent<ItemController>();
            }
        }
        if (item)
        {
             
            if (item.IsAlive() && (transform.position.y >= mLowestPos.y))
            {
                //if(mRigid.velocity.y > 0 )
                //{
                //    return;
                //}
                //============ apply power =============
                if (mIsPowerMode && other.gameObject.CompareTag("Game_Item"))
                {
                    GameObject[] gamePlayObjList = GameObject.FindGameObjectsWithTag("Game_Item");
                    float posY;
                    if(other.gameObject.name == "Cube(Clone)")
                    {
                        posY = other.gameObject.transform.parent.transform.position.y;
                        mLowestPos = other.gameObject.transform.parent.transform.position;
                    }
                    else if (other.gameObject.name == "New_cube (1)" || other.gameObject.name == "New_cube (2)")
                    {
                        posY = other.gameObject.transform.parent.gameObject.transform.parent.transform.position.y;
                        mLowestPos = other.gameObject.transform.parent.gameObject.transform.parent.transform.position;
                    }
                    else
                    {
                        posY = other.gameObject.transform.position.y;
                    }
                    for (int i = 0; i < gamePlayObjList.Length; i++)
                    {
                        if (gamePlayObjList[i].gameObject.transform.position.y == posY)
                        {
                            ItemController itemCtl = gamePlayObjList[i].gameObject.GetComponent<ItemController>();
                            if (itemCtl)
                            {
                                itemCtl.Die();
                                //mTrailEffect.gameObject.SetActive(false);
                            }
                        }
                    }

                    GameObject powEffect = Instantiate(mPowerExploseEffect, transform.position, mPowerExploseEffect.transform.rotation);
                    Destroy(powEffect.gameObject, powEffect.gameObject.GetComponent<ParticleSystem>().main.duration);

                    //Material mat = powEffect.gameObject.GetComponent<Renderer>().material;
                    //Color alPhaColor = mColor; // new Color();
                    //alPhaColor.a = 0.25f;
                    //mat.color = alPhaColor;
                    //Destroy(powEffect, 0.2f);


                    // ==============gen perfect tex===================
                    int bonus = mNumberOfPerfect * REWARD_POINT_VALUE;
                    mGame.AddPoint(bonus, true);

                    GameObject perfect = Instantiate(mPerfectTextObj, mPerfectTextObj.transform.position, mPerfectTextObj.transform.rotation);
                    UnityEngine.UI.Text textPoint = perfect.gameObject.GetComponent<Text>();
                    textPoint.text = "Perfect! +" + bonus;
                    textPoint.fontSize = 100 + (mNumberOfPerfect * 10);
                    perfect.transform.SetParent(mGui.transform);
                    Destroy(perfect, 1.1f);
                    //=================================================

                    //play sound 
                    if (SoundActive == 1)
                        source.PlayOneShot(mSoundPowerUp, mSoundVol * 0.7f);

                    mRigid.velocity = new Vector3(0, FORCE, 0);
                    

                    if (mNumberOfPerfect > 0)
                    {
                        mNumberOfPerfect = 0;
                    }
                    mIsPowerMode = false;
                    mPower.gameObject.SetActive(false);

                    return;
                }

                //======================================

                if (item.getType() == eItemType.E_ITEM_RED)
                {
                    mIsDie = true;
                    mGame.EndGame(true);
                    if (SoundActive == 1)
                        source.PlayOneShot(mSoundEndGame, mSoundVol);
                    //ShowAds();
                    //Time.timeScale = 0.0f;
                }
                else
                {
                    //play sound 
                    if (SoundActive == 1)
                        source.PlayOneShot(mSoundColider, mSoundVol);
                    if (other.gameObject.CompareTag("Finish") == true)
                    {
                        mIsDie = true;
                        mGame.EndGame(false);
                    }
                    else
                    {
                        mRigid.velocity = new Vector3(0, FORCE, 0);
                        mLowestPos = other.gameObject.transform.position;
                    }

                    if (mNumberOfPerfect > 0)
                    {
                        mNumberOfPerfect = 0;
                    }
                }

                Vector3 pos = transform.position;
                pos.y += 0.15f;
                GameObject effect = Instantiate(mExploseEffect, pos, mExploseEffect.transform.rotation);

                //ParticleSystem ps = effect.gameObject.GetComponent<ParticleSystem>();
                //var main = ps.main;
                //main.startColor = mColor;
                ////ps.main.se;// = mColor;
                Destroy(effect.gameObject, effect.gameObject.GetComponent<ParticleSystem>().main.duration);

            }

        }



    }


    private void OnTriggerEnter(Collider other)
    {
    }

    public void ContinueGame()
    {
        Debug.Log("========Player: Continue game now=-=======");
        if(mLastColisionObj)
        {
            ItemController itemCtl;
            if (mLastColisionObj.name == "Cube(Clone)")
            {
                itemCtl = mLastColisionObj.gameObject.transform.parent.gameObject.GetComponent<ItemController>();
                mLastColisionObj.gameObject.transform.parent = null;
                Destroy(mLastColisionObj.gameObject);
            }
            else if(mLastColisionObj.name == "New_cube (1)" || mLastColisionObj.name == "New_cube (2)")
            {
                Destroy(mLastColisionObj.gameObject.transform.parent.gameObject);
                itemCtl = mLastColisionObj.gameObject.transform.parent.gameObject.GetComponent<ItemController>();
            }
            else
            {
                itemCtl = mLastColisionObj.GetComponent<ItemController>();
            }
            
            if(itemCtl)
            {
                GameObject gObject = GameObject.Find("MapGenerator");
                if (gObject)
                {
                    MapGenerater mapGen = gObject.GetComponent<MapGenerater>();
                    if (null != mapGen)
                    {
                        Color blue = mapGen.GetColor(0);
                        itemCtl.Setup(eItemType.e_ITEM_BLUE);
                        //itemCtl.SetColor(blue);
                        itemCtl.ActiveAliveMat();
                    }
                }
            }
        }

        Vector3 pos = transform.position;
        pos.y += 1.0f;
        transform.position = pos;

        mIsDie = false;
        mPower.gameObject.SetActive(false);
        mRigid.velocity = Vector3.zero;
        mTrailEffect.gameObject.GetComponent<TrailRenderer>().Clear();
        mIsPowerMode = false;
        mPower.gameObject.SetActive(false);

    }

    public void UPdateSound(int value)
    {
        SoundActive = value;
    }
}
