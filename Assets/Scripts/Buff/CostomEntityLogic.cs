using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 为了添加Buff设计的自定义EntityLogic
/// </summary>
public class CostumEntityLogic : MonoBehaviour
{
    //buff列表
    public List<BuffBase> m_Buffs = new List<BuffBase>();

    /// <summary>
    /// 被冰冻
    /// </summary>
    public bool IsFreeze { get; set; }

    /// <summary>
    /// 是否处于无敌状态
    /// </summary>
    public bool IsGodDefend { get; set; }
    
    /// <summary>
    /// 是否处于流血状态
    /// </summary>
    public bool IsBleed { get; set; }

    /// <summary>
    /// 是否处于烧伤状态
    /// </summary>
    public bool IsBurned { get; set; }

    /// <summary>
    /// 是否在回血
    /// </summary>
    public bool IsHealing { get; set; }

    /// <summary>
    /// 是否被诅咒   
    /// </summary>
    public bool IsCurved { get; set; }

    /// <summary>
    /// 得到无敌状态
    /// </summary>
    public virtual void GetGodDefend()
    {
    }

    /// <summary>
    /// 失去无敌状态
    /// </summary>
    public virtual void LoseGodDefend()
    {
    }
    /// <summary>
    /// 获得相同buff
    /// </summary>
    /// <param name="buff"></param>
    /// <returns></returns>
    public BuffBase GetBuff(BuffBase buff)
    {
        int length = m_Buffs.Count;
        for(int i = 0; i < length; i++)
        {
            if (buff.EqualTo(m_Buffs[i]))
            {
                return m_Buffs[i];
            }
        }
        return null;
    }

    public void AddBuff(BuffBase buffNeed2Add)
    {
        BuffBase oldBuff = GetBuff(buffNeed2Add);

        if (oldBuff != null)
        {
            switch (buffNeed2Add.m_BuffOverlap)
            {
                case BuffBase.BuffOverlap.ResterTime:
                    oldBuff.timer = 0;
                    break;
                case BuffBase.BuffOverlap.StackedLayer:
                    oldBuff.m_Layer = Mathf.Min(buffNeed2Add.m_Layer+oldBuff.m_Layer,oldBuff.layerLimit);
                    break;
                case BuffBase.BuffOverlap.StackedTime:
                    oldBuff.m_Length = Mathf.Min(buffNeed2Add.m_Length + oldBuff.m_Length, oldBuff.timeLimit);
                    break;
            }
        }
        else
        {
            m_Buffs.Add(buffNeed2Add);
            buffNeed2Add.OnAdd(this);
            //buffNeed2Add.OnAdd();
        }
    }

    public void Update()
    {
        ReFreshBuff();
    }

    public void ReFreshBuff()
    {
        for (int i = m_Buffs.Count - 1; i >= 0; i--)
        {
            m_Buffs[i].OnUpdate(this);
            //m_Buffs[i].OnUpdate();
        }
    }

    public void RemoveBuff(BuffBase buffNeed2Remove)
    {
        m_Buffs.Remove(buffNeed2Remove);
        buffNeed2Remove.OnRemove(this);
        //buffNeed2Remove.OnRemove();
    }
}