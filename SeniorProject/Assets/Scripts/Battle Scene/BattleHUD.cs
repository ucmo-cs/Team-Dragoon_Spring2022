using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BattleHUD : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public Slider hpSlider;
    public Slider kiSlider;
    //public Image playerImage;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "Lvl " + unit.unitLevel;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
        kiSlider.maxValue = unit.maxKI;
        kiSlider.value = unit.currentKI;

        //playerImage.GetComponent(SpriteRenderer).sprite = unit.characterPortrait;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }

    public void SetKI(int ki)
    {
        kiSlider.value = ki;
    }
}
