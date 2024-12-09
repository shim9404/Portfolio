using UnityEngine;

namespace StatusEffect
{
    public class Weakness : TimedStatusEffectBase
    {
        public override void Init(Type _statusEffectType)
        {
            base.Init(_statusEffectType);
            SetDuration(10.0f);
        }

        public override void Activate()
        {
            base.Activate();
            Debug.Log("쇠약 상태 활성화!");
        }

        public override void Deactivate()
        {
            base.Deactivate();
            Debug.Log("쇠약 상태 해제!");
        }
    }
}