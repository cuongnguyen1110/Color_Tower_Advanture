using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerater : MonoBehaviour {

    public GameObject mMainObject;
    public GameObject mFinish;
    public List<GameObject> mListResourceObject;
    public GameObject mCubDie;
    public GameObject mObtascle_01;
    public GameObject mObtascle_02;

    public GameObject mCotTru;
    // Use this for initialization

    private Vector3 mInitPos;
    private int mTotalBlock;

    private Color mBLUE_COLOR;
    private Color mRED_COLOR;

    public Material mRedMaterial;
    public Material mBlueMaterial;

    private PlayerController mPlayer;

    private List<Color> listColor;
    private Color mEmissionColor;

    MapGenerater()
    {
        mTotalBlock = 5;
        mInitPos = new Vector3(0, -2, 0);
        mBLUE_COLOR = Color.blue;
        mRED_COLOR = Color.yellow;
    }
    void Start ()
    {
        Debug.unityLogger.logEnabled = false;
        GameObject player = GameObject.Find("Player");
        if (player)
        {
            mPlayer = player.GetComponent<PlayerController>();
        }

        listColor = new List<Color>();
        Color c1 = new Color();
        ColorUtility.TryParseHtmlString("#2DF3FF", out c1);

        Color c2 = new Color();
        ColorUtility.TryParseHtmlString("#FC2DFF", out c2);

        Color c3 = new Color();
        ColorUtility.TryParseHtmlString("#2DFFC2", out c3);

        Color c4 = new Color();
        ColorUtility.TryParseHtmlString("#2DFF63", out c4);

        Color c5 = new Color();
        ColorUtility.TryParseHtmlString("#BFFF2D", out c5);

        Color c6 = new Color();
        ColorUtility.TryParseHtmlString("#FFCE2D", out c6);

        Color c7 = new Color();
        ColorUtility.TryParseHtmlString("#FF6E2D", out c7);

        Color c8 = new Color();
        ColorUtility.TryParseHtmlString("#2D7DFF", out c8);

        listColor.Insert(0, c1);
        listColor.Insert(0, c2);
        listColor.Insert(0, c3);
        listColor.Insert(0, c4);
        listColor.Insert(0, c5);
        listColor.Insert(0, c6);
        listColor.Insert(0, c7);
        listColor.Insert(0, c8);

    }

    public int GetTotalBlock()
    {
        return mTotalBlock;
    }
	
    private List<int> GetFirstBlock()
    {
        List<int> listData = new List<int>();

        int ran = Random.Range(0, 5);
        switch(ran)
        {
            case 0:
                {
                    listData.Clear();
                    listData.Add(-2);
                    listData.Add(-3);
                    listData.Add(3);
                    listData.Add(2);
                    listData.Add(-2);
                    break;
                }
            case 1:
                {
                    listData.Clear();
                    listData.Add(-2);
                    listData.Add(3);
                    listData.Add(3);
                    listData.Add(2);
                    listData.Add(-2);
                    break;
                }
            case 2:
                {
                    listData.Clear();
                    listData.Add(-2);
                    listData.Add(3);
                    listData.Add(3);
                    listData.Add(-2);
                    listData.Add(2);
                    break;
                }
            case 3:
                {
                    listData.Clear();
                    listData.Add(-2);
                    listData.Add(3);
                    listData.Add(-1);
                    listData.Add(2);
                    listData.Add(-2);
                    listData.Add(2);
                    break;
                }
            case 4:
                {
                    listData.Clear();
                    listData.Add(-1);
                    listData.Add(2);
                    listData.Add(2);
                    listData.Add(2);
                    listData.Add(-2);
                    listData.Add(1);
                    listData.Add(2);
                    break;
                }
            default:
                {
                    listData.Clear();
                    listData.Add(-1);
                    listData.Add(2);
                    listData.Add(2);
                    listData.Add(2);
                    listData.Add(-2);
                    listData.Add(1);
                    listData.Add(2);
                    break;
                }
        }
       
        return listData;
    }

    private bool GenCubeDieObject(int level, GameObject obj, Vector3 possition, bool activeAnim)
    {

        bool returnCubeDie = false;
       //if (numberOfRedCube == 0)
        {
            int obsObjectId = 0;
            int rateObj = Random.Range(0, 10);
            if(rateObj <= 5)
            {
                obsObjectId = 0;
            }
            else if(rateObj > 5 && rateObj <=7)
            {
                obsObjectId = 1;
            }
            else
            {
                obsObjectId = 2;
            }


            switch (obsObjectId)
            {
                case 0:
                    {
                        int ran = Random.Range(0, 99);
                        int rate = 2 + (int)(level * 0.85f);
                        rate = Mathf.Clamp(rate, 2, 80);
                        if (ran < rate)
                        {
                            Vector3 cubePos = possition;
                            cubePos.y -= 0.1f;
                            GameObject cube = Instantiate(mCubDie, cubePos, mCubDie.transform.rotation);
                            ItemController cubeItem = cube.gameObject.GetComponent<ItemController>();
                            cubeItem.Setup(eItemType.E_ITEM_RED);
                            cubeItem.SetColor(mRED_COLOR);
                            cubeItem.ActiceDeathMat();
                            cubeItem.SetEmmissionColor(mEmissionColor);
                            cubeItem.SetObsObject(true);
                            if(activeAnim)
                            {
                                cubeItem.ActiveAnim(true);
                            }
                            cube.transform.parent = obj.transform;
                            returnCubeDie = true;
                        }
                        break;
                    }
                case 1:
                    {
                        int ran = Random.Range(0, 99);
                        int rate = 2 + (int)(level * 0.95f);
                        rate = Mathf.Clamp(rate, 2, 90);
                        if (ran < rate)
                        {
                            Vector3 cubePos = possition;
                            cubePos.y += 0.1f;
                            GameObject cube = Instantiate(mObtascle_01, cubePos, mObtascle_01.transform.rotation);
                            ItemController cubeItem = cube.gameObject.GetComponent<ItemController>();
                            cubeItem.Setup(eItemType.E_ITEM_RED);
                            cubeItem.SetColor(mRED_COLOR);
                            cubeItem.ActiceDeathMat();
                            cubeItem.SetEmmissionColor(mEmissionColor);
                            cubeItem.SetObsObject(true);
                            //if (activeAnimation)
                            {
                                cubeItem.ActiveAnim(true);
                            }
                            cube.transform.parent = obj.transform;
                        }
                        break;
                    }
                case 2:
                    {
                        int ran = Random.Range(0, 99);
                        int rate = 2 + (int)(level * 1.05f);
                        rate = Mathf.Clamp(rate, 2, 90);
                        if (ran < rate)
                        {
                            Vector3 cubePos = possition;
                            cubePos.y += 0.1f;
                            GameObject cube = Instantiate(mObtascle_02, cubePos, mObtascle_02.transform.rotation);
                            ItemController cubeItem = cube.gameObject.GetComponent<ItemController>();
                            cubeItem.Setup(eItemType.E_ITEM_RED);
                            cubeItem.SetColor(mRED_COLOR);
                            cubeItem.ActiceDeathMat();
                            cubeItem.SetEmmissionColor(mEmissionColor);
                            cubeItem.SetObsObject(true);
                            //if (activeAnimation)
                            {
                                cubeItem.ActiveAnim(true);
                            }
                            cube.transform.parent = obj.transform;
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

        }

        return returnCubeDie;

    }

    private void GenBlock(Vector3 possition, int level, bool isAllBlue)
    {
        List<int> listData;
        GameObject obj;
        listData = GenListData(level);

        //// ===============test======================
        //listData.Clear();
        //listData.Add(1);
        //listData.Add(2);
        //listData.Add(3);
        //listData.Add(4);
        //listData.Add(1);
        //listData.Add(1);
        //// ===============test end======================

        // Debug.Log("Round: " + round + ". possition = " + possition);
        int totalValue = 0;
        int numberOfRedObject = 0;
        int numberOfRedCube = 0;

        bool hasCubeDieIsBlock = false;
        
        int maxOfRedCube = 1 + (int)(level * 0.12f);
        if (isAllBlue)
        {
            listData = GetFirstBlock();
        }

        int rotate = 0;

        for (int i = 0; i < listData.Count; i++)
        {
            
            if(i > 0)
            {
                //rotate += (Mathf.Abs(listData[i]) * (360 / 12));
                rotate = rotate + (Mathf.Abs(listData[i ]) * (360 / 12)) / 2 + (Mathf.Abs(listData[i - 1]) * (360 / 12)) / 2;
            }
            if (listData[i] > 0)
            {
                // Create object, 
                obj = Instantiate(mListResourceObject[listData[i] - 1], possition, mListResourceObject[listData[i] - 1].transform.rotation);
                ItemController item = obj.gameObject.GetComponent<ItemController>();
                if (item)
                {
                    if( (listData[i] == 1 || listData[i] == 2) && numberOfRedObject < 3)
                    {
                        int ran = Random.Range(0, 10);
                        if ((ran <= 7 && numberOfRedObject > 0)
                            || (isAllBlue) // Round 0 is all BLUE
                        )
                        {
                            item.Setup(eItemType.e_ITEM_BLUE);
                            item.SetColor(mBLUE_COLOR);

                            //=====Gen Cube die======================
                            if(!isAllBlue)
                            {
                                if (numberOfRedCube < maxOfRedCube)
                                {
                                    bool activeAnim = hasCubeDieIsBlock;
                                    if (!hasCubeDieIsBlock)
                                    {
                                        hasCubeDieIsBlock = GenCubeDieObject(level, obj, possition, activeAnim);
                                    }
                                    else
                                    {
                                        GenCubeDieObject(level, obj, possition, activeAnim);
                                    }

                                    numberOfRedCube++;
                                }
                            }
                            
                        }
                        else
                        {
                            numberOfRedObject++;
                            item.Setup(eItemType.E_ITEM_RED);
                            item.SetColor(mRED_COLOR);
                            item.ActiceDeathMat();
                            item.SetEmmissionColor(mEmissionColor);
                            Vector3 scale = item.transform.localScale;

                            scale.x *= 1.05f;
                            scale.y *= 1.002f;
                            scale.z *= 1.1f;
                            item.transform.localScale = scale;

                            // set rotate
                            if(level >= 10)
                            {
                                int rotationRate = 1 + (int)(level * 1.2f);
                                if(rotationRate > 90)
                                {
                                    rotationRate = 90;
                                }
                                int rate = Random.Range(0, 100);
                                if(rate < rotationRate)
                                {
                                    int ranSpeed = Random.Range(5, 25);
                                    float rotateSpd = 20.0f + ranSpeed;
                                    int ranDegree = Random.Range(5, 35);
                                    float rotateDegree = 45.0f + ranDegree;

                                    item.SetRotation(rotateDegree, rotateSpd);
                                }
                                
                            }
                           

                          
                        }
                        // setDeath
                    }
                    else
                    {
                        item.Setup(eItemType.e_ITEM_BLUE);
                        item.SetColor(mBLUE_COLOR);

                        //=====Gen Cube die======================
                        //numberOfRedCube = 0;
                        if(numberOfRedCube < maxOfRedCube)
                        {
                            bool activeAnim = hasCubeDieIsBlock;
                            if (!hasCubeDieIsBlock)
                            {
                                hasCubeDieIsBlock = GenCubeDieObject(level, obj, possition, activeAnim);
                            }
                            else
                            {
                                GenCubeDieObject(level, obj, possition, activeAnim);
                            }
                            numberOfRedCube++;
                        }
                        

                        //=======================================
                    }
                }
                // rotate object
                obj.transform.Rotate(Vector3.forward * rotate, Space.Self);

                // mMainObject.addChild
                obj.transform.parent = mMainObject.transform;
                //  mMainObject.gameObject.
            }

            totalValue += Mathf.Abs(listData[i]);
        }
    }


    public void RandomColor()
    {
        bool done = false;
        do
        {
            int ran = Random.Range(0, listColor.Count);

            if (mEmissionColor != listColor[ran])
            {
                mEmissionColor = listColor[ran];
                done = true;
            }

        }
        while (!done);
        



        //int ranFirstColor = Random.Range(0, 255);
        //int ranSecondColor = Random.Range(0, 255);

        //mBLUE_COLOR.r = 0.0f;
        //mBLUE_COLOR.g = ((float)ranFirstColor) / 255.0f;
        //mBLUE_COLOR.g = Mathf.Clamp(mBLUE_COLOR.g, 100.0f/255.0f, 1.0f);
        //mBLUE_COLOR.b = ((float)ranSecondColor) / 255.0f;
        //mBLUE_COLOR.a = 1.0f;

        //mRED_COLOR.r = 1.0f;
        //mRED_COLOR.g = ((float)ranFirstColor) / 255.0f; ;
        //mRED_COLOR.b = ((float)ranSecondColor) / 255.0f;
        //mRED_COLOR.a = 1.0f;

        //Color playerColor = new Color();
        //playerColor.r = ((float)ranFirstColor) / 255.0f;
        //playerColor.g = ((float)ranSecondColor) / 255.0f;
        //playerColor.b = 0.0f;
        //playerColor.a = 1.0f;

        //mPlayer.SetColor(playerColor);

        //Debug.Log("mBLUE_COLOR = " + mBLUE_COLOR);
        //Debug.Log("mRED_COLOR = " + mRED_COLOR);
    }

    public Color GetColor(int type)
    {
        if(type == 0) // BLUE
        {
            return mBLUE_COLOR;
        }
        else
        {
            return mRED_COLOR;
        }
    }

    public void GenMap(int level)
    {

        level += 10;
        mInitPos = new Vector3(0, -2, 0);
        Vector3 possition = mInitPos;
        mTotalBlock = 13;
        mTotalBlock = mTotalBlock + (int)(level *1.1f);
        if(mTotalBlock > 120)
        {
            mTotalBlock = 120; // linmit at 80 block
        }
        //mTotalBlock = 20;
        Debug.Log("===========Gen Map: round = " + mTotalBlock);

        for (int round = 0; round < mTotalBlock; round++)
        {
            bool isGenAllBlue = (round == 0);
            GenBlock(possition, level, isGenAllBlue);
            possition.y -= 1.4f;

        }

        // add finish object

        GameObject finisObj = Instantiate(mFinish, possition, mFinish.transform.rotation);
        finisObj.transform.parent = mMainObject.transform;

        float totalLong = -1.0f * finisObj.gameObject.transform.position.y;
        float scale = totalLong / 0.999f;

        Vector3 vScale = mCotTru.gameObject.transform.localScale;
        vScale.z = scale;
        mCotTru.gameObject.transform.localScale = vScale;
        //===========


    }

    List<int> GenListData(int level)
    {
        List<int> data = new List<int>();

        int rand = Random.Range(0, 10);
        int numberOfNonObject;
        if(rand <=4)
        {
            numberOfNonObject = 1;
        }
        else if(rand >4 && rand <=8)
        {
            numberOfNonObject = 2;
        }
        else
        {
            numberOfNonObject = 3;
        }

        for(int i = 0; i < numberOfNonObject; i++)
        {
            int randUnit = Random.Range(0, 10);
            int numberOfUnit;
            if(randUnit <2)
            {
                numberOfUnit = 1;
            }
            else if( randUnit >= 2 && randUnit < 8)
            {
                numberOfUnit = 2;
            }
            else
            {
                numberOfUnit = 3;
            }
           
            numberOfUnit *= -1;
            data.Insert(0,numberOfUnit);
        }

        int totalValue = 0;
        for(int i = 0; i < data.Count; i++)
        {
            totalValue += Mathf.Abs(data[i]);
        }

        int currentValue = 12 - totalValue;

        //==================  Gen daeth object ====================
        rand = Random.Range(0, 10);
        int numberOfDeath;
        if(rand <= 5)
        {
            numberOfDeath = 1;
        }
        else if(rand >5 && rand <=8)
        {
            numberOfDeath = 2;
        }
        else
        {
            numberOfDeath = 3;
        }

        for(int i = 0; i < numberOfDeath; i++)
        {
            data.Insert(0,1);
        }
        //=======================================================

        totalValue = 0;
        for (int i = 0; i < data.Count; i++)
        {
            totalValue += Mathf.Abs(data[i]);
        }
        currentValue = 12 - totalValue;
        while (currentValue > 0)
        {

            rand = Random.Range(1, 6);
            if(currentValue - rand < 0)
            {
                rand = currentValue;
            }

            data.Insert(0,rand);
            currentValue -= rand;
        }


        //============= Mix data===================

        for (int i = 0; i < data.Count; i++)
        {
            int temp = data[i];
            int randomIndex = Random.Range(i, data.Count);
            data[i] = data[randomIndex];
            data[randomIndex] = temp;
        }
        //============================================


        //
        return data;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
