using UnityEngine;
using System;
using MarketSDK;

/// <summary>
/// <para> Market SDK를 생성 및 기능을 호출할 때 사용 </para>
/// </summary>
public class MarketSDKManager : MonoBehaviour
    {
        private bool isInit = false;
        MarketType loggedInType;

        IMarketSDKLogin login;
        IMarketSDKSocial social;

        private static MarketSDKManager instance;
        public static MarketSDKManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject(nameof(MarketSDKManager)).AddComponent<MarketSDKManager>();
                    DontDestroyOnLoad(instance);
                }
                return instance;
            }
        }

        /// <summary>
        /// <paramref name="_type"/>으로 인스턴스 생성하고 초기화를 진행함. 결과는 <paramref name="_callback"/>으로 전달
        /// </summary>
        /// <param name="_type">로그인할 마켓 타입</param>
        /// <param name="_callback">초기화 결과 콜백 함수</param>
        public void Init(MarketType _type, Action<bool> _callback)
        {
            // 마켓 타입에 맞는 인스턴트 생성 후 캐싱
            if (login == null) login = MarketSDKFactory.GetLoginInstance(_type);
            if (social == null) social = MarketSDKFactory.GetSocialInstance(_type);

            social.Init();

            login.Init(result =>
            {
                if (result)
                {
                    
                    isInit = true;
                    login.Login((bool result, string msg) => 
                    {
                        if (result) LoginSuccess(MarketSDK.MarketType.GPGS);
                        _callback?.Invoke(result);
                    });
                }
                else
                {
                    _callback?.Invoke(false);
                }
            });
        }

        #region 로그인

        /// <summary>
        /// 현재 로그인 된 마켓 종류 반환
        /// </summary>
        /// <returns></returns>
        public MarketType GetLoggedInType()
        {
            return loggedInType;
        }

        /// <summary>
        /// 자동 로그인 진행 후 결과 전달
        /// </summary>
        public void QuickLogin(MarketType _type, Action<bool> _callback)
        {
            Action<bool> loginCallback = (result) =>
            {
                if (result) LoginSuccess(_type);
                _callback?.Invoke(result); 
            };

            if (isInit == false)
            {
                Init(_type, result =>
                {
                    if (result) login?.QuickLogin(loginCallback);
                    else _callback?.Invoke(false);
                });
            }
            else
            {
                login?.QuickLogin(loginCallback);
            }
        }

        /// <summary>
        /// 초기화 여부와 유니티 social 함수를 이용해 마켓 로그인 여부를 반환
        /// </summary>
        /// <returns>마켓 로그인 여부</returns>
        public bool CheckSocialLoggedIn()
        {
            return login != null;
        }

        /// <summary>
        /// 로그인 진행 후 결과 전달
        /// </summary>
        public void Login(MarketType _type, Action<bool, string> _callback)
        {
            Action<bool, string> loginCallback = ((result, msg) =>
            {
                if (result) LoginSuccess(_type);
                _callback?.Invoke(result, msg);
            });

            if (isInit == false)
            {
                Init(_type, result =>
                {
                    if (result) login?.Login(loginCallback);
                    else _callback?.Invoke(false, "InitFail");
                });
            }
            else
            {
                login?.Login(loginCallback);
            }

        }

        private void LoginSuccess(MarketType _type)
        {
            loggedInType = _type;
        }


        /// <summary>
        /// 로그아웃을 요청
        /// </summary>
        public void RequestLogout()
        {
            login?.Logout((result) => { });
        }

        #endregion

        #region 소셜 기능

        /// <summary>
        /// <paramref name="_leaderboardId"/>에 해당하는 리더보드를 화면에 띄움. 생략 시 리더보드 목록을 화면에 띄움.
        /// </summary>
        /// <param name="_callback">리더 보드 호출 후 실행될 함수</param>
        /// <param name="_leaderboardId">화면에 띄울 리더보드 id 생략 시 리더보드 목록을 화면에 띄움</param>
        public void ShowLeaderBoard(MarketType _type, string _leaderboardId = "", Action<bool> _callback = null)
        {
            Action<bool> leaderboardCallback = (result) =>
            {
                if (result) social?.ShowLeaderboard(_leaderboardId, _callback);
                else _callback?.Invoke(false);
            };

            if (loggedInType == MarketType.None) Login(_type, (result, msg) => { leaderboardCallback?.Invoke(result); });
            else leaderboardCallback?.Invoke(true);
        }

        /// <summary>
        /// <paramref name="_leaderboardId"에 해당하는 리더보드에 점수 <paramref name="_value"/>점을 업로드 함. 처리 결과는 <paramref name="_callback"/>으로 전달 
        /// </summary>
        /// <param name="_value">업로드 할 점수</param>
        public void ReportScore(MarketType _type, int _value, string _leaderboardId, Action<bool> _callback = null)
        {
            Action<bool> leaderboardCallback = (result) =>
            {
                if (result) social?.ReportScore(_value, _leaderboardId, _callback);
                else _callback?.Invoke(false);
            };

            if (loggedInType == MarketType.None) Login(_type, (result, msg) => { leaderboardCallback?.Invoke(result); });
            else leaderboardCallback?.Invoke(true);
        }


        #endregion
}