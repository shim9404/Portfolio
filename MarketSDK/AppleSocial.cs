using System;
using UnityEngine;

namespace MarketSDK
{
    public class AppleSocial : IMarketSDKSocial
    {
        /// <summary>
        /// 애플 게임 센터 로그인
        /// </summary>
        private void GameCenterLogin(Action<bool> _callback = null)
        {
            Social.localUser.Authenticate(result => _callback?.Invoke(result));
        }


        public void ReportScore(int _value, string _id, Action<bool> _callback = null)
        {
            Action<bool> loginCallback = (isSuccess =>
            {
                if (isSuccess) _callback?.Invoke(true);
                else _callback?.Invoke(false);
            });
      

            if (Social.localUser.authenticated == false)
            {
                GameCenterLogin(result =>
                {
                    if (result == false) _callback?.Invoke(false);
                    else Social.ReportScore(_value, _id, loginCallback);
                });
            }
            else
            {
                Social.ReportScore(_value, _id, loginCallback);
            }
        }

        public void ShowLeaderboard(string _id = null, Action<bool> _callback = null)
        {
            Action loginCallback = () =>
            {
                Social.ShowLeaderboardUI();
                _callback?.Invoke(true);
            };

            if (Social.localUser.authenticated == false)
            {
                GameCenterLogin(result =>
                {
                    if (result == false) _callback?.Invoke(false);
                    else loginCallback?.Invoke();
                });
            }
            else
            {
                loginCallback?.Invoke();
            }

        }
    }
}