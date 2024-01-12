using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] int hp;
    [SerializeField] int coins;
    [SerializeField] TMP_Text hp_txt;
    [SerializeField] TMP_Text coins_txt;

    void Start()
    {
        
    }
    private void Update()
    {
        DisplayUpdate();
    }

    void DisplayUpdate()
    {
        hp_txt.text = hp.ToString();
        coins_txt.text = "$" + coins.ToString();
    }
    //===========================
    //       PUBLIC METHODS
    //===========================
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
    }


    //========================
    //       COLLISIONS
    //========================


}
