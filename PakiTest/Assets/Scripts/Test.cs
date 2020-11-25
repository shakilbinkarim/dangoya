using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Text money;


    // Use this for initialization
    private void Start()
    {
        money.text = GamerPrefs.GetMoney().ToString();
    }

    public void AddMoney()
    {
        int money = GamerPrefs.GetMoney();
        money += 1;
        GamerPrefs.SetMoney(money);
        this.money.text = GamerPrefs.GetMoney().ToString();
    }

}
