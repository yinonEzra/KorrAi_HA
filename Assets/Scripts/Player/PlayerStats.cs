using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] int startingHP = 3;
    [SerializeField] int hp;
    [SerializeField] int coins;
    [SerializeField] TMP_Text hp_txt;
    [SerializeField] TMP_Text coins_txt;
    [SerializeField] float hurtCoolDown;

    float coolDownTimer;
    bool canHurt;
    private void Update()
    {
        DisplayUpdate();
        CoolDownTimer();
    }
    void CoolDownTimer()
    {
        if (!canHurt)
        {
            coolDownTimer += Time.deltaTime;
            canHurt = coolDownTimer > hurtCoolDown ? true : false;
        }
        else
        {
            coolDownTimer = 0;
        }
    }
    void DisplayUpdate()
    {
        hp_txt.text = hp.ToString();
        coins_txt.text = "$" + coins.ToString();
    }
    //===========================
    //       PUBLIC METHODS
    //===========================
    public void ResetStats()
    {
        hp = startingHP;
        coins = 0;
    }
    public void AddCoin(int amount)
    {
        coins += amount;
    }
    public void AddHeart(int amount)
    {
        hp += amount;
    }
    public void LoseHeart()
    {
        if (!canHurt) return;

        GetComponent<PlayerMovementSystem>().HurtAnim();
        if (hp > 1)
        {
            hp--;
        }
        else
        {
            hp = 0;
            gameManager.Gameover();
        }

        canHurt = false;
    }


    //========================
    //       COLLISIONS
    //========================


}
