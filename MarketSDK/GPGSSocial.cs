using System;

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

namespace MarketSDK
{
    // GPGS V2 대응 업데이트(v2.1.0)
    public class GPGSSocial : IMarketSDKSocial
    {

        public void Init()
        {
        }

        public void ReportScore(int _value, string _id, Action<bool> _callback = null)
        {
#if UNITY_ANDROID
            PlayGamesPlatform.Instance.ReportScore(_value, _id, (isSuccess) =>
            {
                if (isSuccess)
                {
                    _callback?.Invoke(true);
                }
                else
                {
                    _callback?.Invoke(false);
                }
            });
#endif
        }

        public void ShowLeaderboard(string _id = null, Action<bool> _callback = null)
        {
#if UNITY_ANDROID
            if (string.IsNullOrEmpty(_id))
            {
                PlayGamesPlatform.Instance.ShowLeaderboardUI();
                _callback?.Invoke(true);
            }
            else PlayGamesPlatform.Instance.ShowLeaderboardUI(_id, staus => CheckLeaderboardUIStatus(staus, _callback));
#endif
        }

#if UNITY_ANDROID
        /// <summary>
        /// <paramref name="_uiStatus"/>에 따라 <paramref name="_callback"/>에 리더보드 호출 성공 여부를 전달 
        /// </summary>
        /// <param name="_uiStatus">Leaderboard UI 호출 시 반환 코드</param>
        /// <param name="_callback">리더 보드 호출 후 실행될 함수</param>
        public void CheckLeaderboardUIStatus(UIStatus _uiStatus, Action<bool> _callback)
        {
            if (_uiStatus == UIStatus.Valid) _callback?.Invoke(true);
            else _callback?.Invoke(false);
        }
#endif

    }
}