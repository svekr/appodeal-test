using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AppodealStarter : MonoBehaviour, IRewardedVideoAdListener {
    public TextMeshProUGUI debugTF;
    public TextMeshProUGUI coinsTF;

    private int _coinsAmount = 0;
    private bool _isWaiting = false;

    void Start() {
        string applicationKey = "8e7baea7fda98925426fa05dc2d375d872634c70b8757476";
        Appodeal.setTesting(true);
        Appodeal.disableLocationPermissionCheck();
        Appodeal.initialize(applicationKey, Appodeal.INTERSTITIAL | Appodeal.BANNER_VIEW | Appodeal.REWARDED_VIDEO);
        Appodeal.setRewardedVideoCallbacks(this);
        UpdateCoinsTF();
    }

    public void ShowAd() {
        if (_isWaiting) {
            return;
        }
        if (Appodeal.isLoaded(Appodeal.REWARDED_VIDEO)) {
            ShowRewardedVideo();
        } else {
            Log("REWARDED_VIDEO is not loaded.");
            _isWaiting = true;
            Appodeal.cache(Appodeal.REWARDED_VIDEO);
        }
    }

    public void onRewardedVideoLoaded() {
        _isWaiting = false;
        Log("Rewarded video loaded.");
        if (Appodeal.canShow(Appodeal.REWARDED_VIDEO)) {
            Log("Show Appodeal REWARDED_VIDEO");
            Appodeal.show(Appodeal.REWARDED_VIDEO);
        } else {
            Log("Appodeal can not show REWARDED_VIDEO");
        }
    }

    public void onRewardedVideoFailedToLoad() {
        _isWaiting = false;
        Log("Rewarded video failed to load.");
    }

    public void onRewardedVideoShown() {
        Log("Rewarded video shown.");
    }

    public void onRewardedVideoFinished(int amount, string name) {
        Log("Rewarded video finished.");
    }

    public void onRewardedVideoClosed(bool finished) {
        Log("Rewarded video closed.");
        if (finished) {
            _coinsAmount += 10;
            UpdateCoinsTF();
        }
    }

    private void ShowRewardedVideo() {
        _isWaiting = false;
        if (Appodeal.canShow(Appodeal.REWARDED_VIDEO)) {
            Log("Show Appodeal REWARDED_VIDEO");
            Appodeal.show(Appodeal.REWARDED_VIDEO);
        } else {
            Appodeal.cache(Appodeal.REWARDED_VIDEO);
            Log("Appodeal can not show REWARDED_VIDEO");
        }
    }

    private void UpdateCoinsTF() {
        if (coinsTF != null) {
            coinsTF.text = _coinsAmount.ToString() + " COINS";
        }
    }

    private void Log(string message) {
        if (message != null && message.Length > 0) {
            Debug.Log(message);
            if (debugTF != null) {
                debugTF.text += message + "\n";
            }
        }
    }
}
