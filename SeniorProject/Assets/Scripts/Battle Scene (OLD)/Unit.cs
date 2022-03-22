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

    public int damage;
    public int KIDamage;
    public int KICost;

    public Sprite characterPortrait;
    public GameObject projectile;

    /*public AttackInformation attack1;
    public AttackInformation attack2;
    public AttackInformation attack3;
    public AttackInformation attack4;*/
}
