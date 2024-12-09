using System;

namespace MarketSDK
{
    public interface IMarketSDKSocial
    {
        /// <summary>
        /// <paramref name="_id"/>에 해당하는 리더보드를 화면에 띄움. 생략 시 리더보드 목록을 화면에 띄움
        /// </summary>
        /// <param name="_id">화면에 띄울 리더보드 id 생략 시 리더보드 목록을 화면에 띄움</param>
        /// <param name="_callback">리더 보드 호출 후 실행될 함수</param>
        void ShowLeaderboard(string _id = "", Action<bool> _callback = null);

        /// <summary>
        /// <paramref name="_id"에 해당하는 리더보드에 점수 <paramref name="_value"/>점을 업로드 함. 처리 결과는 <paramref name="_callback"/>으로 전달
        /// </summary>
        /// <param name="_value">업로드할 점수</param>
        /// <param name="_id">업로드할 랭킹의 id 값</param>
        /// <param name="_callback">점수 업로드 호출 후 실행될 함수</param>
        void ReportScore(int _value, string _id, Action<bool> _callback = null);

    }
}