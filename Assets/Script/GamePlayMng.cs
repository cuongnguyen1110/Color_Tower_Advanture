using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class GamePlayMng : MonoBehaviour, IUnityAdsListener
{


    public UnityEngine.UI.Text mTxtCurPoint;
    public UnityEngine.UI.Text mTxtCurLevel;
    public UnityEngine.UI.Text mTxtNextLevel;
    public UnityEngine.UI.Text mTxtBestPoint;

    public UnityEngine.UI.Text mTxtProcessComplete;
    public UnityEngine.UI.Text mTxtTapToStart;
    public UnityEngine.UI.Text mTxtWatchingAds;
    public UnityEngine.UI.Image mEndGameImg;
    public UnityEngine.UI.Image mLoading;
    public UnityEngine.UI.Image mLoading_backGround;

    public UnityEngine.UI.Slider mProcessBar;

    public Button mVideoBtn;
    public Slider mVideoSlider;

    private bool mIsVideoSliderActive;

    private int mCurrentLevel;
    private int mBestPoint;
    private int mCurrentPoint;

    private MapGenerater mMapGen;
    private PlayerController mPlayer;
    public MainObjectContrller mainObject;

    public GameObject mMainCam;

    private int mNumBlockDie;

    private bool mIsGameDie;

    private bool mIsPlayerDie;

    private bool mIsGameStarted;

    private int mGameSeason;

    private bool mShowInterstitialAd;

    //============ Menu======================

    public Canvas mBottomMenu;
    public Canvas mQuitGamePopup;
    public Button mSoundActive;
    public Button mSoundDeactive;

    public Button mNoAdsBtn;

    //===========================================

    private InterstitialAd interstitial;
    private RewardBasedVideoAd rewardBasedVideo;
    private BannerView mBanerAds;

    private List<Color> mListBGTop;
    private List<Color> mListBGBottom;

    private bool mLoadingDone;
    private int mIsAdsActive;
    // Use this for initialization


    //===================Implement Unity Ads=================================================
    private string UnityAdsGameId = "2697088";
    private string UnityAdsBannerID = "baner";
    private string UnityAdsRerawdVideoID =  "colorTowerVideoReward";
    private string UnityInstitialVidID =    "colortowernonreward";
    private bool UnityAdsTestMode = true;

    private bool mIsRewardVidReady = false;

    private void InitUnityAds()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(UnityAdsGameId, UnityAdsTestMode);
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        BannerOptions bannerOption = new BannerOptions();
        
        Advertisement.Banner.Load(UnityAdsBannerID);
        Advertisement.Load(UnityAdsRerawdVideoID);
        Advertisement.Load(UnityInstitialVidID);
        StartCoroutine(ShowBannerWhenReady());
    }

    IEnumerator ShowBannerWhenReady()
    {
        while (!Advertisement.IsReady(UnityAdsBannerID) )
        {
            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("-----------------Show Unity Banner---------->>>");
        Advertisement.Banner.Show(UnityAdsBannerID);
        Debug.Log("-----Baner loaded------");
        Vector3 pos = mBottomMenu.GetComponent<RectTransform>().position; //.y = 75 + 320 / 2;
        pos.y = 115 + 320 / 2;
        mBottomMenu.GetComponent<RectTransform>().position = pos;
    }

   public void OnUnityAdsReady(string placementId)
    {
        Debug.Log("Unity Ads OnUnityAdsReady: " + placementId);
        if(UnityAdsRerawdVideoID == placementId)
        {
            mIsRewardVidReady = true;
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log("Unity Ads OnUnityAdsDidError: " + message);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("Unity Ads OnUnityAdsDidStart: " + placementId);

       if(UnityAdsBannerID == placementId)
        {
            //Debug.Log("-----Baner loaded------");
            //Vector3 pos = mBottomMenu.GetComponent<RectTransform>().position; //.y = 75 + 320 / 2;
            //pos.y = 115 + 320 / 2;
            //mBottomMenu.GetComponent<RectTransform>().position = pos;
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        Debug.Log("Unity Ads OnUnityAdsDidFinish: " + placementId + " . Result = "+ showResult);
        if(UnityAdsRerawdVideoID == placementId)
        {
            Debug.Log("Result for showing Reward Video: " + showResult);
            mIsRewardVidReady = false;
            Advertisement.Load(UnityAdsRerawdVideoID);

            Debug.Log("HandleRewardBasedVideoClosed event received");
            mGameSeason++;
            mVideoBtn.gameObject.SetActive(false);
            mVideoSlider.gameObject.SetActive(false);
            mIsVideoSliderActive = false;
            mVideoSlider.value = 0;
            int sound = PlayerPrefs.GetInt("SoundSetting", 1);
            mPlayer.UPdateSound(sound);

            if(showResult == ShowResult.Finished)
            {
                mShowInterstitialAd = false;
                SecondChance();
                mTxtWatchingAds.gameObject.SetActive(false);
            }
        }
        else if(UnityInstitialVidID == placementId)
        {
            Advertisement.Load(UnityInstitialVidID);
        }
    }

    public void ShowRewardVideo()
    {
        Advertisement.Show(UnityAdsRerawdVideoID);

    }

    //===================Unity Ads=================================================
    void Start ()
    {
        Debug.unityLogger.logEnabled = false;
        mLoadingDone = false;
        mShowInterstitialAd = true;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        mIsAdsActive = PlayerPrefs.GetInt("ADS_ACTIVE", 1);
        Debug.Log("mIsAdsActive =  " + mIsAdsActive);

        //  InitAds();
        InitUnityAds();
        mGameSeason = 0;
        // PlayerPrefs.SetInt("Level", 1);
        // PlayerPrefs.SetInt("BestPoint", 0);
        mIsGameStarted = false;
        mCurrentPoint = 0;
        mTxtCurPoint.text = "" + mCurrentPoint;

        //===============menu=======================

        mBottomMenu.gameObject.SetActive(false);
        //mSettingMenu.gameObject.SetActive(false);
        //mBallMenu.gameObject.SetActive(false);
        mVideoBtn.gameObject.SetActive(false);
        mVideoSlider.gameObject.SetActive(false);
        mQuitGamePopup.gameObject.SetActive(false);
        mVideoSlider.value = 0;
        mIsVideoSliderActive = false;

        //=================================================

        //mCurrentLevel = 1;
        
        mCurrentLevel =  PlayerPrefs.GetInt("Level", 1);
        mBestPoint = PlayerPrefs.GetInt("BestPoint", 0);
        mTxtCurLevel.text = "" + mCurrentLevel;
        mTxtNextLevel.text = "" + (mCurrentLevel + 1);

        mProcessBar.value = 1.0f;

        GameObject gObject = GameObject.Find("MapGenerator");
        if(gObject)
        {
            mMapGen = gObject.GetComponent<MapGenerater>();
            if(null == mMapGen)
            {
                Debug.Log("WARNING!!!!!!!!! can not find MapGen object");
            }
        }

        gObject = GameObject.Find("Player");
        if (gObject)
        {
            mPlayer = gObject.GetComponent<PlayerController>();
            if (null == mMapGen)
            {
                Debug.Log("WARNING!!!!!!!!! can not find PlayerController object");
            }
        }

        gObject = GameObject.Find("GameObject");
        if (gObject)
        {
            mainObject = gObject.GetComponent<MainObjectContrller>();
            if (null == mMapGen)
            {
                Debug.Log("WARNING!!!!!!!!! can not find MainObjectContrller object");
            }
        }
        UpdateSoundMenu();
        ShowLoading();
        mNumBlockDie = 0;
        mMapGen.RandomColor();
        mPlayer.RandomDef();
        mBottomMenu.gameObject.SetActive(true);
        if (mIsAdsActive == 0)
        {
            mNoAdsBtn.gameObject.SetActive(false);
        }
        StartGameLevel(mCurrentLevel);
        

       // 
    }

    private void ShowLoading()
    {
        mLoading.gameObject.SetActive(true);
        mLoading_backGround.gameObject.SetActive(true);
        mPlayer.UPdateSound(0);
        Invoke("HideLoading", 3.0f);

    }

    private void HideLoading()
    {
        mLoading.gameObject.SetActive(false);
        mLoading_backGround.gameObject.SetActive(false);
        int sound = PlayerPrefs.GetInt("SoundSetting", 1);
        mPlayer.UPdateSound(sound);

        mLoadingDone = true;
    }

    public void EndGame( bool isDie)
    {
        if (mIsGameDie)
            return;
      //  Time.timeScale = 0.0f;
        if(isDie)
        {
            //Time.timeScale = 0.0f;
            mTxtTapToStart.text = "Tap To Restart.";
            float process = ((float)(mNumBlockDie) / mMapGen.GetTotalBlock()) * 100;
            mTxtProcessComplete.text = "Process Complete: " + process.ToString("0.0") + "%";
            mTxtProcessComplete.gameObject.SetActive(true);

          //  if(mIsAdsActive == 1)
            {
                if (mIsRewardVidReady && mGameSeason == 0)
                {
                    mVideoBtn.gameObject.SetActive(true);
                    mVideoSlider.gameObject.SetActive(true);
                    mIsVideoSliderActive = true;
                    Invoke("HideVideoButton", 3.5f);
                }
            }
            

            
        }
        else
        {
            mMapGen.RandomColor();
            
            PlayerPrefs.SetInt("Level", mCurrentLevel+1);
            //PlayerPrefs.SetInt("Level", 30);
            mTxtTapToStart.text = "Level Completed.";
            // RestartGame();
             Invoke("RestartGame", 1.5f);
        }
        mainObject.SetActive(false);
        mTxtTapToStart.gameObject.SetActive(true);
        mEndGameImg.gameObject.SetActive(true);
        mIsGameDie = true;

        PlayerPrefs.SetInt("BestPoint", mBestPoint);
        mIsPlayerDie = isDie;

    }

    private void HideVideoButton()
    {
        mVideoBtn.gameObject.SetActive(false);
        mVideoSlider.gameObject.SetActive(false);
        mIsVideoSliderActive = false;
    }

    public void RestartGame()
    {

        if (!mIsGameDie)
            return;

        mGameSeason = 0;
             
        if (mIsPlayerDie)
        {
            mCurrentLevel = PlayerPrefs.GetInt("Level", 1);
            mCurrentPoint = 0;
            mTxtCurPoint.text = "" + mCurrentPoint;
            if (mIsAdsActive == 1)
            {
                int rate = UnityEngine.Random.Range(0, 10);
                if (rate <= 5 && mShowInterstitialAd)
                {
                    if( Advertisement.IsReady(UnityInstitialVidID) )
                    {
                        Advertisement.Show(UnityInstitialVidID);
                    }
                }
            }
            
            
        }
        else
        {
            mCurrentLevel ++;
            mPlayer.RandomDef();
        }
        
        Debug.Log("------------RestartGame---------------");
        mVideoBtn.gameObject.SetActive(false);
        mVideoSlider.gameObject.SetActive(false);
        mIsVideoSliderActive = false;
        mVideoSlider.value = 0;
        mTxtBestPoint.text = "Best: " + mBestPoint;
        mTxtCurLevel.text = "" + mCurrentLevel;
        mTxtNextLevel.text = "" + (mCurrentLevel + 1);
        mProcessBar.value = 1.0f;
        mNumBlockDie = 0;
        mIsGameDie = false;
        Time.timeScale = 1.0f;
        mTxtTapToStart.gameObject.SetActive(false);
        mTxtWatchingAds.gameObject.SetActive(false);
        mEndGameImg.gameObject.SetActive(false);
        mIsPlayerDie = false;
        mTxtProcessComplete.gameObject.SetActive(false);

        mainObject.Restart();
        CameraController camCtrl = mMainCam.GetComponent<CameraController>();
        if(camCtrl)
        {
            camCtrl.Reset();
        }

        mPlayer.Reset();

        GameObject[] gamePlayObjList = GameObject.FindGameObjectsWithTag("Game_Item");
        for (int i = 0; i < gamePlayObjList.Length; i++)
        {
            Destroy(gamePlayObjList[i]);
        }

        gamePlayObjList = GameObject.FindGameObjectsWithTag("Finish");
        for (int i = 0; i < gamePlayObjList.Length; i++)
        {
            Destroy(gamePlayObjList[i]);
        }

        StartGameLevel(mCurrentLevel);
    }

    public void StartGameLevel(int level)
    {
        //RequestBanner();
        //LoadBanerAds();

        //RequestInterstitial();
        //RequestRewardBasedVideo();

        mShowInterstitialAd = true;
       

        mEndGameImg.gameObject.SetActive(false);
        mIsGameDie = false;
        mIsPlayerDie = false;
        mCurrentLevel = level;
        mTxtCurLevel.text = "" + mCurrentLevel;
        mTxtNextLevel.text = "" + (mCurrentLevel + 1);
        mTxtBestPoint.text = "Best: " + mBestPoint;
      //  mProcessBar.value = 1.0f;
        mTxtTapToStart.gameObject.SetActive(false);
        mTxtWatchingAds.gameObject.SetActive(false);
        mTxtProcessComplete.gameObject.SetActive(false);
        mMapGen.GenMap(level);
        ShowMenu(0);
        mainObject.SetActive(false);
        
    }

    public void AddPoint(int point, bool isBonus)
    {
        mCurrentPoint += point;
        if(!isBonus)
        {
            mTxtCurPoint.text = "" + mCurrentPoint;
            mNumBlockDie++;
            Debug.Log("AddPoint " + mNumBlockDie + " . " + mMapGen.GetTotalBlock());
            float process = ((float)(mNumBlockDie) / mMapGen.GetTotalBlock()) * 100;
            mProcessBar.value = process;
        }

        if(mCurrentPoint > mBestPoint)
        {
            mBestPoint = mCurrentPoint;
          //  mTxtBestPoint.text = "" + mBestPoint;
        }
       
    }

    public void IncreaseProcess( float value)
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        //if(mIsGameDie)
        //{
        //   // Debug.Log("=========== GAME DIE===========");
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        RestartGame();
        //    }
        //}

        if(mIsVideoSliderActive)
        {
            float value = mVideoSlider.value;
            value = value +  (Time.deltaTime / 3.5f);
            mVideoSlider.value = value;
        }

        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if(mLoadingDone)
            {
                OnAndroidBack();
            }
        }
    }

    public void ShowMenu(int menuId)
    {
        Debug.Log("-------------Show Menu: " + menuId);
        HideAllMenu();

        switch (menuId)
        {
            case 0: // BottomMenu
                {
                    mBottomMenu.gameObject.SetActive(true);
                    if (mIsAdsActive == 0)
                    {
                        mNoAdsBtn.gameObject.SetActive(false);
                    }
                    break;
                }
            case 1: //setting menu
                {
                  //  mSettingMenu.gameObject.SetActive(true);
                    UpdateSoundMenu();
                    break;
                }
            case 2: // ballmenu
                {
                   // mBallMenu.gameObject.SetActive(true);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    private void UpdateSoundMenu()
    {
        int sound = PlayerPrefs.GetInt("SoundSetting", 1);
        if(sound == 1)
        {
            mSoundActive.gameObject.SetActive(true);
            mSoundDeactive.gameObject.SetActive(false);
        }
        else
        {
            mSoundActive.gameObject.SetActive(false);
            mSoundDeactive.gameObject.SetActive(true);
        }
    }

    public void HideAllMenu()
    {
        mBottomMenu.gameObject.SetActive(false);
        //mSettingMenu.gameObject.SetActive(false);
        //mBallMenu.gameObject.SetActive(false);
    }

    public void ChangeSound()
    {
        int sound = PlayerPrefs.GetInt("SoundSetting", 1);
        sound = sound == 1 ? 0 : 1;
        PlayerPrefs.SetInt("SoundSetting", sound);
        mPlayer.UPdateSound(sound);
        UpdateSoundMenu();
        //if)
    }

    public void StartGame()
    {
        mIsGameStarted = true;
        mainObject.SetActive(true);
        HideAllMenu();
        mTxtWatchingAds.gameObject.SetActive(false);
    }
    //    private List<Color> mListBGTop;
    //private List<Color> mListBGBottom;

    public void SecondChance()
    {
        mGameSeason++;
        mVideoBtn.gameObject.SetActive(false);
        mVideoSlider.gameObject.SetActive(false);
        mIsVideoSliderActive = false;
        mVideoSlider.value = 0;
        mTxtBestPoint.text = "Best: " + mBestPoint;
        mTxtCurLevel.text = "" + mCurrentLevel;
        mTxtNextLevel.text = "" + (mCurrentLevel + 1);
        
       // mNumBlockDie = 0;
        mIsGameDie = false;
        mTxtTapToStart.gameObject.SetActive(false);
        mEndGameImg.gameObject.SetActive(false);
        mIsPlayerDie = false;
        mTxtProcessComplete.gameObject.SetActive(false);
        mainObject.Restart();
        mPlayer.ContinueGame();
    }




    //============== Call back for video ads=============

        /*

        
    public void InitAds()
    {
#if UNITY_ANDROID
        string appId = "ca-app-pub-6969113737753671~9877936434";
#elif UNITY_IPHONE
            string appId = "ca-app-pub-3940256099942544~1458002511";
#else
            string appId = "unexpected_platform";
#endif
        // Initialize the Google Mobile Ads SDK.
        GoogleMobileAds.Api.MobileAds.Initialize(appId);
        //GoogleMobileAds.Api.MobileAds.

        InitVideoAds();


        if (mIsAdsActive == 1)
        {
            InitBanner();
            InitInterstitialAds();
        }
        else
        {
            mNoAdsBtn.gameObject.SetActive(false);
            //mSoundActive.
            Vector3 pos = mSoundActive.GetComponent<RectTransform>().position; //.y = 75 + 320 / 2;
            pos.x = Screen.width / 2;
            mSoundActive.GetComponent<RectTransform>().position = pos;
            mSoundDeactive.GetComponent<RectTransform>().position = pos;
        }

    }

    private void RequestRewardBasedVideo()
    {
        //if (mIsAdsActive != 1)
        //{
        //    return;
        //}

        if (this.rewardBasedVideo.IsLoaded() == false)
        {
#if UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/5224354917"; //testId
            //string adUnitId = "ca-app-pub-6969113737753671/5377129233";
#elif UNITY_IPHONE
            string adUnitId = "/112517806/628511536130824";
#else
            string adUnitId = "unexpected_platform";
#endif
            // AdRequest request = new AdRequest.Builder().AddTestDevice("060353F6DB480B2C9AC89B7EAB229A9F").Build();
            AdRequest request = new AdRequest.Builder().Build();
            this.rewardBasedVideo.LoadAd(request, adUnitId);
            Debug.Log("ADS---reward video---- send request");
        }

    }


    private void InitBanner()
    {
        if (mIsAdsActive != 1)
        {
            return;
        }
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-6969113737753671/3050348792";
        //string adUnitId = "ca-app-pub-3940256099942544/6300978111"; //test id
#elif UNITY_IPHONE
            string adUnitId = "/112517806/128511533567181";
#else
            string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the Bottom of the screen.
        mBanerAds = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        mBanerAds.OnAdLoaded += BanerLoaded;

        AdRequest request = new AdRequest.Builder().Build();
        // Load the banner with the request.
        mBanerAds.LoadAd(request);
    }

    private void BanerLoaded(object sender, System.EventArgs args)
    {
        Debug.Log("-----Baner loaded------");
        Vector3 pos = mBottomMenu.GetComponent<RectTransform>().position; //.y = 75 + 320 / 2;
        pos.y = 115 + 320 / 2;
        mBottomMenu.GetComponent<RectTransform>().position = pos;

    }

    private void LoadBanerAds()
    {
        if (mIsAdsActive != 1)
        {
            return;
        }

        AdRequest request = new AdRequest.Builder().Build();
        // Load the banner with the request.
        mBanerAds.LoadAd(request);
        Debug.Log("ADS---Baner---- send request");
    }

    private void InitInterstitialAds()
    {
        if (mIsAdsActive != 1)
        {
            return;
        }
#if UNITY_ANDROID
        //string adUnitId = "ca-app-pub-3940256099942544/1033173712"; //testId
        string adUnitId = "ca-app-pub-6969113737753671/8510559033";
#elif UNITY_IPHONE
            string adUnitId = "/112517806/328511533567181";
#else
            string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.

        interstitial.OnAdLoaded += HandleOnAdLoaded;
        interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when an ad is shown.
        interstitial.OnAdOpening += HandleInterstitialOpened;
    }
    private void RequestInterstitial()
    {

        if (mIsAdsActive != 1)
        {
            return;
        }

        if (interstitial.IsLoaded() == false)
        {
            // Load the interstitial with the request.
            AdRequest request = new AdRequest.Builder().Build();
            Debug.Log("ADS---Interstitial send request");
            interstitial.LoadAd(request);
        }


    }

    public void HandleInterstitialOpened(object sender, System.EventArgs args)
    {
        Debug.Log("HandleInterstitialOpened event received");
        mTxtWatchingAds.gameObject.SetActive(true);

    }

    public void HandleOnAdLoaded(object sender, System.EventArgs args)
    {
        Debug.Log("HandleAdLoaded event received");

    }

    public void HandleOnAdClosed(object sender, System.EventArgs args)
    {
        mainObject.Restart();
        int sound = PlayerPrefs.GetInt("SoundSetting", 1);
        mPlayer.UPdateSound(sound);

    }

    private void InitVideoAds()
    {
        rewardBasedVideo = RewardBasedVideoAd.Instance;
        // Called when an ad request has successfully loaded.
        rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
        // Called when an ad request failed to load.
        rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        // Called when an ad is shown.
        rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
        // Called when the ad starts to play.
        rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
        // Called when the user should be rewarded for watching a video.
        rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        // Called when the ad is closed.
        rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        // Called when the ad click caused the user to leave the application.
        rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;

        RequestRewardBasedVideo();
    }

   

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoLoaded event received");
        
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log(
            "HandleRewardBasedVideoFailedToLoad event received with message: "
                             + args.Message);
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoOpened event received");
        mTxtWatchingAds.gameObject.SetActive(true);
        mShowInterstitialAd = false;
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoStarted event received");
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoClosed event received");
        mGameSeason++;
        mVideoBtn.gameObject.SetActive(false);
        mVideoSlider.gameObject.SetActive(false);
        mIsVideoSliderActive = false;
        mVideoSlider.value = 0;
        int sound = PlayerPrefs.GetInt("SoundSetting", 1);
        mPlayer.UPdateSound(sound);
        RequestRewardBasedVideo();
       

    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        Debug.Log(
            "HandleRewardBasedVideoRewarded event received for "
                        + amount.ToString() + " " + type);
        SecondChance();
        mTxtWatchingAds.gameObject.SetActive(false);
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
    }

    */

    //====================================================


    //======================= FOR IAP==============================

    public void IapPurchaseSuccess(int itemID)
    {
        if(itemID == 0) //buy no ads
        {
            mIsAdsActive = 0;// PlayerPrefs.GetInt("ADS_ACTIVE", 1);
            PlayerPrefs.SetInt("ADS_ACTIVE", mIsAdsActive);
          //  mBanerAds.Hide();
            mNoAdsBtn.gameObject.SetActive(false);
            //mSoundActive.
            Vector3 pos = mSoundActive.GetComponent<RectTransform>().position; //.y = 75 + 320 / 2;
            pos.x = Screen.width / 2;
            mSoundActive.GetComponent<RectTransform>().position = pos;
            mSoundDeactive.GetComponent<RectTransform>().position = pos;

            pos = mBottomMenu.GetComponent<RectTransform>().position; //.y = 75 + 320 / 2;
            pos.y = 85.0f;
            mBottomMenu.GetComponent<RectTransform>().position = pos;

            Debug.Log("IapPurchaseSuccess: buy item no_ads");

        }
        else
        {
            Debug.Log("IapPurchaseSuccess WARNING IAP!!!!: buy item " + itemID);
        }
    }

    public void IapPurchaseFail(int itemID)
    {
        if (itemID == 0) //buy no ads
        {
            Debug.Log("IapPurchaseFail: buy item no_ads");
        }
        else
        {
            Debug.Log("IapPurchaseFail WARNING IAP!!!!: buy item " + itemID);
        }
    }
    //================================================================


    public void PauseGame()
    {
        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        mQuitGamePopup.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Time.timeScale = 1.0f;
        mQuitGamePopup.gameObject.SetActive(false);
        Application.Quit();
    }

    public void OnAndroidBack()
    {
        //show popup
        mQuitGamePopup.gameObject.SetActive(true);
        PauseGame();
    }
}
