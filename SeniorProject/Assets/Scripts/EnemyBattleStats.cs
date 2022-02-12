using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyBattleStats : ScriptableObject
{
    public new string name;
    public Sprite artwork;
    
    public int health;
    public int mana;
    public int attack;
    public int magic;
    public int defense;
    public int speed;
    public int nextActTurn;

}
