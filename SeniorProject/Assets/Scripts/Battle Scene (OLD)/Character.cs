using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Character : ScriptableObject
{

    public string unitName;
    public int unitLevel;

    public int maxHP;
    public int currentHP;

    public Sprite characterPortrait;

    public AttackInformation attack1;
    public AttackInformation attack2;
    public AttackInformation attack3;
    public AttackInformation attack4;
}
