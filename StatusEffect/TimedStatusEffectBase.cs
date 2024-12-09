using System;

namespace StatusEffect
{
    /// <summary>
    /// 지속 시간이 있는 상태 이상에서 필요한 기본 로직 구현
    /// </summary>
    public class TimedStatusEffectBase : StatusEffectBase, ITimedStatusEffect
    {
        // float - 활성화 후 경과된 시간, float - 총 지속 시간
        public event Action<float, float> OnRemainTime;
    
        public Timer timer;
        public float duration = 5.0f;


        /// <summary>
        /// 지속 시간 설정
        /// </summary>
        public void SetDuration(float _duration)
        {
            duration = _duration;
        }

        public override void Activate()
        {
            // 해당 상태 이상 활성화 시 타이머 실행 후 완료 시 상태 이상 종료하도록 callback 등록
            StartTimer(() => iStatusEffectable.Deactivate(type));
        }


        /// <summary>
        /// 상태가 활성화 되었을 때 지속 시간 계산용 타이머 실행 구현
        /// </summary>
        public void StartTimer(Action _doneCallback = null)
        {
            if (timer == null) timer = TimeManager.instance.GetTimer();
            else timer.StopTimer();

            timer.StartTimer(duration, SendRemainTimeEvent, () =>
            {
                _doneCallback?.Invoke();
                StopTimer();
            });
        }

        public void StopTimer()
        {
            timer.StopTimer();
            TimeManager.instance.RestoreTimer(timer);
            timer = null;
        }

        public void SendRemainTimeEvent(float _currTime)
        {
            OnRemainTime?.Invoke(_currTime, duration);
        }

        #region 이벤트 연결

        public void BindRemainTime(Action<float, float> _timerCallback)
        {
            OnRemainTime += _timerCallback;
        }

        public void UnbindRemainTime(Action<float, float> _timerCallback)
        {
            OnRemainTime -= _timerCallback;
        }

        #endregion
    }
}
