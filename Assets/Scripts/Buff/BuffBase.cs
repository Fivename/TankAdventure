using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffBase
{
    /// <summary>
    /// buff的种类
    /// </summary>
    public enum BuffKind
    {
        None,
        Freeze,
        Dizziness,
        Burns,
        Bleed,
        Heal,
        Curse,
    }
    /// <summary>
    /// Buff的叠加结算
    /// </summary>
    public enum BuffOverlap
    {
        None,
        /// <summary>
        /// 增加时间
        /// </summary>
        StackedTime,
        /// <summary>
        /// 堆叠层数
        /// </summary>
        StackedLayer,
        /// <summary>
        /// 重置时间
        /// </summary>
        ResterTime,
    }

    /// <summary>
    /// Buff关闭类型
    /// </summary>
    public enum BuffShutDownType
    {
        None,
        /// <summary>
        /// 关闭所有
        /// </summary>
        All,
        /// <summary>
        /// 单层关闭
        /// </summary>
        Layer,
    }

    /// <summary>
    /// Buff执行次数
    /// </summary>
    public enum BuffCalculateType
    {
        None,
        /// <summary>
        /// 一次
        /// </summary>
        Once,
        /// <summary>
        /// 每次
        /// </summary>
        Loop,
    }

    //注：buff的很多都没做完，后续补充
    /// <summary>
    /// buff类别
    /// </summary>
    public BuffKind m_BuffKind;
    public BuffOverlap m_BuffOverlap;
    public BuffShutDownType m_BuffShutDownType;
    public BuffCalculateType m_BuffCalculateType;

    /// <summary>
    /// buff的影响数值
    /// </summary>
    public float m_Num;

    /// <summary>
    /// Buff的层数
    /// </summary>
    public int m_Layer;

    /// <summary>
    /// buff最大层数
    /// </summary>
    public int layerLimit;

    /// <summary>
    /// Buff持续时间,我们约定,Buff时间为0,则为瞬时Buff,只执行OnAdd
    /// </summary>
    public float m_Length;

    /// <summary>
    /// buff最长时间
    /// </summary>
    public float timeLimit;


    /// <summary>
    /// /计时器
    /// </summary>
    public float timer;

    /// <summary>
    /// 所归属的实体
    /// </summary>
    //public CostumEntityLogic m_CostumEntityLogic;

    public BuffBase(BuffKind buffKind, BuffOverlap buffOverlap, BuffShutDownType buffShutDownType, 
        BuffCalculateType buffCalculateType, float length,float num)
    {
        //m_CostumEntityLogic = costumEntityLogic;
        m_BuffKind = buffKind;
        m_BuffOverlap = buffOverlap;
        m_BuffShutDownType = buffShutDownType;
        m_BuffCalculateType = buffCalculateType;
        m_Length = length;
        m_Num = num;
        m_Layer = 1;
        timer = 0;
    }

    public bool EqualTo(BuffBase buffBase)
    {
        if (buffBase.m_BuffKind != m_BuffKind||
            buffBase.m_BuffOverlap != m_BuffOverlap||
            buffBase.m_BuffShutDownType != m_BuffShutDownType||
            buffBase.m_BuffCalculateType != m_BuffCalculateType||
            buffBase.m_Length != m_Length||
            buffBase.m_Num != m_Num)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    /// <summary>
    /// 当添加到实体时执行逻辑
    /// </summary>
    public virtual void OnAdd(CostumEntityLogic costumEntityLogic)
    {
    }

    /// <summary>
    /// 跟随实体每帧更新
    /// </summary>
    public virtual void OnUpdate(CostumEntityLogic costumEntityLogic)
    {
    }

    /// <summary>
    /// 当从实体移除时
    /// </summary>
    public virtual void OnRemove(CostumEntityLogic costumEntityLogic)
    {
    }

}
