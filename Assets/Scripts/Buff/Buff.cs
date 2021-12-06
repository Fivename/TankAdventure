using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : BuffBase
{
    PlayerHealth playerHealth;
    float count=0;
    public Buff(BuffKind buffKind, BuffOverlap buffOverlap, BuffShutDownType buffShutDownType, 
        BuffCalculateType buffCalculateType, float length,float num)
        : base(buffKind,buffOverlap,buffShutDownType,buffCalculateType,length,num)
    {

    }

    public override void OnAdd(CostumEntityLogic costumEntityLogic)
    {
        switch (m_BuffKind)
        {
            case BuffKind.Bleed:
                {
                    base.OnAdd(costumEntityLogic);
                    //m_CostumEntityLogic.IsBleed = true;
                    //costumEntityLogic.IsBleed = true;
                    playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
                }
                break;
            case BuffKind.Freeze:
                {
                    base.OnAdd(costumEntityLogic);
                    //m_CostumEntityLogic.IsFreeze = true;
                    costumEntityLogic.IsFreeze = true;
                }
                break;
            case BuffKind.Burns:
                {
                    base.OnAdd(costumEntityLogic);
                    costumEntityLogic.IsBurned = true;
                    //m_CostumEntityLogic.IsBurned = true;
                }
                break;
            case BuffKind.Heal:
                {
                    base.OnAdd(costumEntityLogic);
                    playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
                }
                break;
        }
    }

    public override void OnUpdate(CostumEntityLogic costumEntityLogic)
    {
        switch (m_BuffKind)
        {
            case BuffKind.Bleed:
                {
                    base.OnUpdate(costumEntityLogic);
                    if (timer >= m_Length)
                    {
                        //costumEntityLogic.IsBleed = false;
                        //m_CostumEntityLogic.IsBleed = false;
                        costumEntityLogic.RemoveBuff(this);
                        //m_CostumEntityLogic.RemoveBuff(this);
                    }
                    //每过1s造成一次伤害 可优化
                    if ((int)(timer - count) >= 1)
                    {
                        count++;
                        playerHealth.DamagePlayer((int)m_Num);
                    }
                    timer += Time.fixedDeltaTime*2;
                }
                break;
            case BuffKind.Freeze:
                {
                    base.OnUpdate(costumEntityLogic);
                    if (timer >= m_Length)
                    {
                        costumEntityLogic.IsFreeze = false;
                        //m_CostumEntityLogic.IsFreeze = false;
                        costumEntityLogic.RemoveBuff(this);
                        //m_CostumEntityLogic.RemoveBuff(this);
                    }
                    timer += Time.fixedDeltaTime * 2;
                }
                break;
            case BuffKind.Burns:
                {
                    base.OnUpdate(costumEntityLogic);
                    if(timer >= m_Length)
                    {
                        costumEntityLogic.IsBurned = false;
                        //m_CostumEntityLogic.IsBurned = false;
                        costumEntityLogic.RemoveBuff(this);
                        //m_CostumEntityLogic.RemoveBuff(this);
                    }
                    if ((int)(timer - count) >= 0.5)
                    {
                        count++;
                        playerHealth.DamagePlayer((int)m_Num);
                    }
                    timer += Time.fixedDeltaTime * 2;
                }
                break;
            case BuffKind.Heal:
                {
                    base.OnUpdate(costumEntityLogic);
                    if(timer >= m_Length)
                    {
                        costumEntityLogic.RemoveBuff(this);
                    }
                    //0.5秒回一次血
                    if ((int)(timer - count) >= 0.5)
                    {
                        count++;
                        playerHealth.DamagePlayer((int)-m_Num);
                    }
                    timer += Time.fixedDeltaTime * 2;
                }
                break;
        }
    }
    public override void OnRemove(CostumEntityLogic costumEntityLogic)
    {
        base.OnRemove(costumEntityLogic);
        GC.Collect();
    }

}
