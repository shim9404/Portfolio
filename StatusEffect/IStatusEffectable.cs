using System;

namespace StatusEffect
{ 
    [Flags]
    public enum Type
    {
        /// <summary> 배고픔 </summary>
        Hungry = 1 << 0,
        /// <summary> 수면 </summary>
        Sleep = 1 << 1,   
        /// <summary> 쇠약 </summary>
        Weakness = 1 << 2  
    }

    /// <summary>
    /// 상태 이상이 적용될 수 있는 객체에 필요한 인터페이스
    /// ex> 플레이어, NPC, 몬스터
    /// </summary>
    public interface IStatusEffectable
    {
        public event Action<Type, bool> OnChangedStatus;

        /// <summary>
        /// 플레이어의 상태에 변화가 있을 때 이벤트로 받을 함수 연결
        /// </summary>
        public void BindChangedStatus(Action<Type, bool> _changeCallback);

        /// <summary>
        /// 플레이어의 상태에 변화가 있을 때 이벤트로 받을 함수 연결 해제
        /// </summary>
        public void UnbindChangedStatus(Action<Type, bool> _changeCallback);

        /// <summary>
        /// 상태 이상 활성화 시 실행될 사항 정의
        /// </summary>
        public void Activate(Type _statusEffectType);

        /// <summary>
        /// 상태 이상 비활성화 시 실행될 사항 정의
        /// </summary>
        public void Deactivate(Type _statusEffectType);

    }
}
