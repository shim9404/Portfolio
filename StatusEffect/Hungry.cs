using UnityEngine;

namespace StatusEffect
{ 
    public class Hungry : StatusEffectBase
    {
        public override void Init(Type _statusEffectType)
        {
            base.Init(_statusEffectType);
        }

        public override void Activate()
        {        
            Debug.Log("배고픔 상태!");
        }

        public override void Deactivate()
        {
            Debug.Log("배고픔 상태 해제!");
        }
}
}
