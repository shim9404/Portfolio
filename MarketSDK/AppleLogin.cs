using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using AppleAuth.Interfaces;
using AppleAuth;
using AppleAuth.Native;
using AppleAuth.Enums;
using AppleAuth.Extensions;

namespace MarketSDK
{
    public class AppleLogin : IMarketSDKLogin
    {
        public static string IdToken { get; private set; }
        public static string AuthCode { get; private set; }
        public static string RawNonce { get; private set; }

        public static string Nonce { get; private set; }

        private IAppleAuthManager appleAuthManager;

        public string appleUserId { get; private set; }

        public void Init(Action<bool> _callback)
        {
#if UNITY_IOS
        try
        {
            var deserializer = new PayloadDeserializer();
            appleAuthManager = new AppleAuthManager(deserializer);

            RawNonce = System.Guid.NewGuid().ToString();
            Nonce = GenerateNonce(RawNonce);

            _callback?.Invoke(true);
        }
        catch (Exception e)
        {
            Debug.LogError("애플 초기화 중 에러 발생: " + e);
            _callback?.Invoke(false);
        }
#endif
        }

        /// <summary>
        /// 키체인으로 로그인 시도 후 실패하면 수동 로그인을 시도 함. 처리 결과는 <paramref name="_callback"/>으로 전달. 
        /// </summary>
        /// <param name="_callback">로그인 후 실행될 함수</param>
        public void Login(Action<bool, string> _callback)
        {
#if UNITY_IOS

        // 키체인 로그인은 이전 로그인 기록이 없다면 실패 처리됨
        var quickLoginArgs = new AppleAuthQuickLoginArgs(Nonce);

        appleAuthManager.QuickLogin(
            quickLoginArgs,
            credential =>   // 키체인 로그인 성공
            {
                try
                {
                    var appleIdCredential = credential as IAppleIDCredential;
                    if (appleIdCredential != null)
                    {
                        appleUserId = appleIdCredential.User.ToString();
                    }

                    _callback?.Invoke(true, null);
                    LoginSuccess();
                }
                catch (Exception e)
                {
                    _callback?.Invoke(false, e.ToString());
                }
            },
            error => // 키체인 로그인 실패
            {
                RawNonce = Guid.NewGuid().ToString();
                Nonce = GenerateNonce(RawNonce);

                // 키체인 로그인이 실패하면 수동 로그인 진행
                var loginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeEmail, Nonce);
                appleAuthManager.LoginWithAppleId(
                    loginArgs,
                    credential => // 로그인 성공 
                    {
                        try
                        {
                            var appleIdCredential = credential as IAppleIDCredential;
                            appleUserId = appleIdCredential.User;
                            AuthCode = Encoding.UTF8.GetString(appleIdCredential.AuthorizationCode);
                            IdToken = Encoding.UTF8.GetString(appleIdCredential.IdentityToken);
                            _callback?.Invoke(true, null);
                            LoginSuccess();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                            _callback?.Invoke(false, e.ToString());
                        }
                    },
                    error => // 로그인 실패
                    {
                        var authorizationErrorCode = error.GetAuthorizationErrorCode();

                        string errorMsg = String.Format("Sign in with Apple failed Code : {0}, Msg: {1}", authorizationErrorCode, error);
                        Debug.LogWarning(errorMsg);
                        _callback?.Invoke(false, errorMsg);
                    });

            });
#endif

        }


        /// <summary>
        /// 애플 ID 로그인 성공 시 호출
        /// </summary>
        private void LoginSuccess()
        {
        }

        public void Logout(Action<bool> _callback)
        {
            // iOS 13 이상 Apple에서 프로그래밍적으로 로그아웃을 미지원
        }

        /// <summary>
        /// <paramref name="appleUserId"/>를 사용하여 자동 로그인 시도 후 처리 결과를 <paramref name="_callback"/>에 전달
        /// </summary>
        /// <param name="_callback"></param>
        /// <param name="_appleId">게임에 처음 로그인 시 저장해둔 애플 사용자 id</param>
        public void QuickLogin(Action<bool> _callback, string _appleId = "")
        {
#if UNITY_IOS

        if (string.IsNullOrEmpty(_appleId) == false) // ID 값이 비어있지 않으면
        {
            CheckCredentialStatusForUserId(_appleId, _callback);           
            return;
        }
        else
        {
            _callback?.Invoke(false); // 자동 로그인 실패
        }
#endif
        }

        /// <summary>
        /// <paramref name="_appleUserId"/>의 인증 상태를 확인 후 처리 결과를 <paramref name="_callback"/>에 전달
        /// </summary>
        /// <param name="_appleUserId">게임에 처음 로그인 시 저장해둔 애플 사용자 id</param>
        /// <param name="_callback">로그인 후 실행될 함수</param>
        private void CheckCredentialStatusForUserId(string _appleUserId, Action<bool> _callback)
        {
#if UNITY_IOS
        // 전달받은 사용자 ID로 인증 상태를 확인한다.
        appleAuthManager.GetCredentialState(
            _appleUserId,
            state =>
            {
                switch (state)
                {
                    case CredentialState.Authorized:
                        _callback?.Invoke(true);
                        LoginSuccess();
                        return;
                    case CredentialState.Revoked:
                    case CredentialState.NotFound:
                        _callback?.Invoke(false);
                        this.appleUserId = "";
                        return;
                }
            },
            error =>
            {
                _callback?.Invoke(false);
                var authorizationErrorCode = error.GetAuthorizationErrorCode();
                Debug.LogWarning("Error while trying to get credential state " + authorizationErrorCode.ToString() + " " + error.ToString());

            });
#endif
        }


        private void Update()
        {
            appleAuthManager?.Update();
        }


        private static string GenerateNonce(string _rawNonce)
        {
            SHA256 sha = new SHA256Managed();
            var sb = new StringBuilder();

            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(_rawNonce));

            // ToString에서 "x2"로 소문자 변환해야 함. 대문자면 실패함.
            foreach (var b in hash) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

    }
}