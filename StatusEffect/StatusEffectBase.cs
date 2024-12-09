
namespace StatusEffect
{
    /// <summary>
    /// 상태 이상에서 공통적으로 필요한 기능 구현
    /// </summary>
    public class StatusEffectBase 
    {
        protected IStatusEffectable iStatusEffectable;
        protected Type type;

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="_type"></param>
        public virtual void Init(Type _type)
        {
            type = _type;
        }

        /// <summary>
        /// 이벤트로 받은 상태에 따라 실행될 함수 지정
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_isActivate"></param>
        private void ReceiveStatus(Type _type, bool _isActivate)
        {
            // 나에게 등록된 상태가 활성화된 것이 아닐 경우 return
            if (_type != type) return;

            if (_isActivate) Activate();
            else Deactivate();
        }


        /// <summary>
        /// 상태 활성화 시 실행될 내용 정의
        /// </summary>
        public virtual void Activate()
        {       
        }

        /// <summary>
        /// 상태 비활성화 시 실행될 내용 정의
        /// </summary>
        public virtual void Deactivate()
        {
        }


        #region 이벤트 연결

        /// <summary>
        /// 상태 활성화 여부 이벤트 수신 등록
        /// </summary>
        public void BindChangedStatus(IStatusEffectable _iStatusEffectable)
        {
            iStatusEffectable = _iStatusEffectable;
            _iStatusEffectable.BindChangedStatus(ReceiveStatus); 
        }

        /// <summary>
        /// 상태 활성화 여부 이벤트 수신 해제
        /// </summary>
        public void UnbindChangedStatus(IStatusEffectable _iStatusEffectable)
        {
            iStatusEffectable = null;
            _iStatusEffectable.UnbindChangedStatus(ReceiveStatus); 
        }

        #endregion
    }
}
