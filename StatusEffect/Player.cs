using System;
using UnityEngine;
using System.Collections.Generic;
using StatusEffect;

/// <summary>
/// <para> 게임에서 플레이어가 가질 수 있는 상태 이상 정의 </para>
/// <para> 추가 시 아래 작업이 필요함. </para>
/// <list type="number">
/// <item> 상태 이상 해제 조건에 따라 StatusEffectBase 혹은 TimedStatusEffectBase class를 상속받은 class 생성 </item>
/// <item> SetUpStatusEffect()에서 인스턴스 생성 추가 </item>
/// </list>
/// </summary>
public class Player : MonoBehaviour, IStatusEffectable
{
    // 플레이어의 상태
    private int currStatus;

    // 각 상태 이상 인스턴스 Dictionary
    public Dictionary<StatusEffect.Type, StatusEffectBase> statusEffectDic;

    public event Action<StatusEffect.Type, bool> OnChangedStatus;
    
    public void Init()
    {
        currStatus = 0;
        SetUpStatusEffect();
    }

    /// <summary>
    /// 각 상태 이상 인스턴스 생성 및 초기화 작업시 호출
    /// </summary>
    public void SetUpStatusEffect()
    {
        statusEffectDic = new Dictionary<StatusEffect.Type, StatusEffectBase>();

        // 안정성을 위해 하드 코딩하는 방식으로 처리
        statusEffectDic.Add(StatusEffect.Type.Weakness, new Weakness());
        statusEffectDic.Add(StatusEffect.Type.Hungry, new Hungry());
        statusEffectDic.Add(StatusEffect.Type.Sleep, new Sleep());     

        // 상태 변경 이벤트 연결 및 초기화
        foreach (var item in statusEffectDic)
        {
            item.Value.Init(item.Key);
            item.Value.BindChangedStatus(this);
        }
    }

    /// <summary>
    /// 상태 이상 이벤트 연결 해제 등 메모리 초기화 작업
    /// 게임 종료 시 호출 필요
    /// </summary>
    public void ClearStatusEffect()
    {
        currStatus = 0;

        foreach (var item in statusEffectDic)
        {
            item.Value.UnbindChangedStatus(this);
        }
        
       statusEffectDic.Clear();
    }

    /// <summary>
    /// <paramref name="_statusEffectType"/> 상태를 활성화
    /// </summary>
    public void Activate(StatusEffect.Type _statusEffectType)
    {
        currStatus = FlagUtility.OnFlag(currStatus, Convert.ToInt32(_statusEffectType));

        OnChangedStatus?.Invoke(_statusEffectType, true);
    }

    /// <summary>
    /// <paramref name="_statusEffectType"/> 상태를 비활성화
    /// </summary>
    public void Deactivate(StatusEffect.Type _statusEffectType)
    {
        currStatus = FlagUtility.OffFlag(currStatus, Convert.ToInt32(_statusEffectType));

        OnChangedStatus?.Invoke(_statusEffectType, false);
    }

    /// <summary>
    /// <paramref name="_statusEffectType"/> 상태가 활성화되어있는지 반환
    /// </summary>
    public bool GetIsActivatedStatusEffect(StatusEffect.Type _statusEffectType)
    {
        return FlagUtility.IsFlag(currStatus, Convert.ToInt32(_statusEffectType));
    }


    #region 이벤트 연결 

    /// <summary>
    /// 플레이어의 상태에 변화가 있을 때 이벤트로 받을 수 있도록 등록
    /// </summary>
    public void BindChangedStatus(Action<StatusEffect.Type, bool> _changeCallback)
    {
        OnChangedStatus += _changeCallback;
    }

    /// <summary>
    /// 플레이어의 상태에 변화가 있을 때 이벤트로 받지 않도록 등록을 해제
    /// </summary>
    public void UnbindChangedStatus(Action<StatusEffect.Type, bool> _changeCallback)
    {
        OnChangedStatus -= _changeCallback;
    }

    /// <summary>
    /// <paramref name="_statusEffectType"/> 상태의 남은 시간을 이벤트로 받을 수 있도록 등록
    /// UI 업데이트 등에 사용
    /// </summary>
    public void BindRemainTime(StatusEffect.Type _statusEffectType, Action<float, float> _callback)
    {
        if (statusEffectDic.TryGetValue(_statusEffectType, out StatusEffectBase _statusEffect))
        {
            // 시간 제한이 있는 상태이상일 경우 구독
            ITimedStatusEffect timedStatusEffect = _statusEffect as ITimedStatusEffect;
            timedStatusEffect?.BindRemainTime(_callback);
        }
    }

    /// <summary>
    /// <paramref name="_statusEffectType"/> 상태의 남은 시간을 이벤트로 받지 않도록 등록을 해제
    /// </summary>
    public void UnbindRemainTime(StatusEffect.Type _statusEffectType, Action<float, float> _callback)
    {
        if (statusEffectDic.TryGetValue(_statusEffectType, out StatusEffectBase _statusEffect))
        {
            // 시간 제한이 있는 상태이상일 경우 구독 해제
            ITimedStatusEffect timedStatusEffect = _statusEffect as ITimedStatusEffect;
            timedStatusEffect?.UnbindRemainTime(_callback);
        }
    }
    
    #endregion

    
}
