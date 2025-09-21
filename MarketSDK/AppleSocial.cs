using System;
using UnityEngine;

namespace MarketSDK
{
    public class AppleSocial : IMarketSDKSocial
    {
        /// <summary>
        /// 애플 게임 센터 로그인
        /// </summary>
        [Obsolete("Social api 미지원으로 수정 예정")]
        private void GameCenterLogin(Action<bool> _callback = null)
        {
            _callback?.Invoke(false);
            return;

            // TODO : Social api 미지원으로 수정 필요
            Social.localUser.Authenticate(result => _callback?.Invoke(result));
        }

        [Obsolete("Social api 미지원으로 수정 예정")]
        public void ReportScore(int _value, string _id, Action<bool> _callback = null)
        {
            _callback?.Invoke(false);
            return;
         
            // TODO : Social api 미지원으로 수정 필요
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

        [Obsolete("Social api 미지원으로 수정 예정")]
        public void ShowLeaderboard(string _id = null, Action<bool> _callback = null)
        {
            _callback?.Invoke(false);
            return;
            
            // TODO : Social api 미지원으로 수정 필요
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