using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KISystem : MonoBehaviour
{
    public event EventHandler OnKIChanged;
    public event EventHandler OnDead;

    private int KIMax;
    private int KI;

    public KISystem(int KIMax)
    {
        this.KIMax = KIMax;
        KI = KIMax;
    }

    public void SetKIAmount(int KI)
    {
        this.KI = KI;
        if (OnKIChanged != null) OnKIChanged(this, EventArgs.Empty);
    }
    public float GetKIPercent()
    {
        return (float)KI / KIMax;
    }


    public int GetKIAmount()
    {
        return KI;
    }


    public void Drain(int amount)
    {
        KI -= amount;
        if (KI < 0)
        {
            KI = 0;
        }
        if (OnKIChanged != null)
            OnKIChanged(this, EventArgs.Empty);
    }

    public bool KIEmpty()
    {
        return KI <= 0;
    }

    public void Heal(int amount)
    {
        KI += amount;
        if (KI > KIMax)
        {
            KI = KIMax;
        }
        if (OnKIChanged != null)
            OnKIChanged(this, EventArgs.Empty);
    }
}
