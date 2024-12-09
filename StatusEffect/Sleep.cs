using UnityEngine;

namespace StatusEffect
{
    public class Sleep : TimedStatusEffectBase
    {
        public override void Init(Type _statusEffectType)
        {
            base.Init(_statusEffectType);
            SetDuration(7.0f);
        }

        public override void Activate()
        {
            base.Activate();
            Debug.Log("수면 상태 활성화!");
        }

        public override void Deactivate()
        {
            base.Deactivate();
            Debug.Log("수면 상태 해제!");
        }

    }
}