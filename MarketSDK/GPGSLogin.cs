using System;
using UnityEngine;

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

namespace MarketSDK
{
    public class GPGSLogin : MonoBehaviour, IMarketSDKLogin
    {
        public void Init(Action<bool> _callback)
        {
#if UNITY_ANDROID
            try
            {
                // GPGS 플러그인 설정
                PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
                    .Builder()
                    .EnableSavedGames()
                    .Build();

                // 초기화
                PlayGamesPlatform.InitializeInstance(config);
                PlayGamesPlatform.DebugLogEnabled = true; // 디버그 로그를 보고 싶지 않다면 false로 변경
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

        #region  로그인


        public void QuickLogin(Action<bool> _callback, string _id = "")
        {
#if UNITY_ANDROID
            // 로그인 허용 UI가 뜨지 않게 옵션 설정
            PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.NoPrompt, (result) =>
            {
                if (result == SignInStatus.Success)
                {
                    _callback.Invoke(true);
                }
                else
                {
                    switch (result)
                    {
                        case SignInStatus.UiSignInRequired: // 클라이언트가 서비스에 연결을 시도했지만 사용자가 로그인되어 있지 않습니다.
                    case SignInStatus.DeveloperError:   // 	응용 프로그램이 잘못 구성되었습니다.
                    case SignInStatus.NetworkError:     // 네트워크 오류가 발생했습니다.
                    case SignInStatus.InternalError:     //	내부 오류가 발생했습니다.
                    case SignInStatus.Canceled:          // 사용자 취소
                    case SignInStatus.AlreadyInProgress: // 로그인 진행 중
                    case SignInStatus.Failed:            // 현재 계정으로 로그인 시도에 실패했습니다.
                    case SignInStatus.NotAuthenticated:
                            break;
                        default:
                            break;
                    }
                    _callback.Invoke(false);
                }
            });
#endif
        }


        public void Login(Action<bool, string> _callback)
        {
#if UNITY_ANDROID
            PlayGamesPlatform.Instance.Authenticate(_callback, false);
#endif
        }

        public void Logout(Action<bool> _callback)
        {
#if UNITY_ANDROID
            PlayGamesPlatform.Instance.SignOut();
            _callback?.Invoke(true);
#endif
        }

        #endregion

    }
}