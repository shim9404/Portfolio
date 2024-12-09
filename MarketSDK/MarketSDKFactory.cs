
namespace MarketSDK
{
    public class MarketSDKFactory
    {
        /// <summary>
        /// <paramref name="_type"/> 타입의 로그인 인스턴스를 생성해서 반환
        /// </summary>
        /// <param name="_type"></param>
        public static IMarketSDKLogin GetLoginInstance(MarketType _type)
        {
            switch (_type)
            {
                case MarketType.GPGS:
                    return new GPGSLogin();
                case MarketType.Apple:
                    return new AppleLogin();
                default:
                    return null;
            }
        }        
        
        /// <summary>
        /// <paramref name="_type"/> 타입의 소셜 인스턴스를 생성해서 반환
        /// </summary>
        /// <param name="_type"></param>
        public static IMarketSDKSocial GetSocialInstance(MarketType _type)
        {
            switch (_type)
            {
                case MarketType.GPGS:
                    return new GPGSSocial();
                case MarketType.Apple:
                    return new AppleSocial();
                default:
                    return null;
            }
        }
    }
}