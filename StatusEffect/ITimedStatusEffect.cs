using System;

namespace StatusEffect
{
    /// <summary>
    /// 상태 이상이 해제되는 조건이 시간일 경우 필요한 인터페이스
    /// </summary>
    public interface ITimedStatusEffect 
    {
        // 플레이어 상태의 남은 시간을 알려주는 이벤트 정의 
        // float - 활성화 이후 지난 시간 / float - 상태가 지속될 총 시간
        public event Action<float, float> OnRemainTime;

        /// <summary>
        /// 상태 이상 지속 시간 설정
        /// </summary>
        public void SetDuration(float _duration);

        /// <summary>
        /// 등록된 상태의 남은 시간을 이벤트로 받을 수 있도록 등록
        /// </summary>
        /// <param name="_timerCallback"></param>
        public void BindRemainTime(Action<float, float> _timerCallback);
        /// <summary>
        /// 등록된 상태의 남은 시간을 이벤트로 받는 것을 해제
        /// </summary>
        /// <param name="_timerCallback"></param>
        public void UnbindRemainTime(Action<float, float> _timerCallback);
    }
}