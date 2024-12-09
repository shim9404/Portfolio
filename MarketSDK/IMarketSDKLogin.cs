using System;


namespace MarketSDK
{
    public enum MarketType
    {
        None = -1,
        /// <summary> Google Play Games Services </summary>
        GPGS,
        /// <summary> Apple AppStore </summary>
        Apple
    }

    /// <summary>
    /// 마켓 SDK에서 제공하는 기능 중 사용할 부분 정의
    /// </summary>
    public interface IMarketSDKLogin
    {
        /// <summary>
        /// 초기 설정에 필요한 내용
        /// </summary>
        void Init(Action<bool> _callback);

        /// <summary>
        /// 빠른 로그인
        /// </summary>
        void QuickLogin(Action<bool> _callback, string _id = "");


        /// <summary>
        /// 로그인
        /// </summary>
        /// <param name="callback"></param>
        void Login(Action<bool, string> _callback);


        /// <summary>
        /// 로그아웃
        /// </summary>
        void Logout(Action<bool> _callBack);
    }
}
