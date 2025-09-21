using System;
using UnityEngine;

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;

#endif

namespace MarketSDK
{
    // GPGS V2 대응 업데이트(v2.1.0)
    public class GPGSLogin : MonoBehaviour, IMarketSDKLogin
    {
        public void Init(Action<bool> _callback)
        {
#if UNITY_ANDROID
            try
            {
                PlayGamesPlatform.DebugLogEnabled = false;
                PlayGamesPlatform.Activate();
                _callback?.Invoke(true);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                _callback?.Invoke(false);
                return;
            }
#endif
        }
#if UNITY_ANDROID
        internal void ProcessAuthentication(SignInStatus status, Action<bool, string> _callback) 
        {
            if (status == SignInStatus.Success) 
            {
                _callback?.Invoke(true, "");
            } 
            else 
            {   
                // 로그인 실패 시 수동 로그인 호출
                PlayGamesPlatform.Instance.ManuallyAuthenticate((SignInStatus _status) =>  
                {
                    _callback?.Invoke(_status == SignInStatus.Success, "");
                });
            }
        }
#endif

        #region  로그인

    
        /// <summary>
        /// 호출 전 Init 필수
        /// </summary>
        public void Login(Action<bool, string> _callback)
        {
#if UNITY_ANDROID
            PlayGamesPlatform.Instance.Authenticate((SignInStatus status) =>  
            {
                ProcessAuthentication(status, _callback);
            });
#endif
        }

        public void Logout(Action<bool> _callback)
        {  
            // GPGS 로그아웃 미 지원
#if UNITY_ANDROID
            
            _callback?.Invoke(true);
#endif
        }

        [Obsolete("GPGS V2 대응으로 미 사용")]
        public void QuickLogin(Action<bool> _callback, string _id = "")
        {
            _callback?.Invoke(false);
        }
        
        #endregion

    }
}