using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Character character;
    public string unitName;
    public int unitLevel;

    public int maxHP;
    public int currentHP;

    public int maxKI;
    public int currentKI;

    public Sprite characterPortrait;

    public AttackInformation attack1;
    public AttackInformation attack2;
    public AttackInformation attack3;
    public AttackInformation attack4;
}
