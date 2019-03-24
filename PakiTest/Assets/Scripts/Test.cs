using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Text money;


    // Use this for initialization
    private void Start()
    {
        money.text = GamerPrefs.GetOkane().ToString();
    }

    public void AddMoney()
    {
        int money = GamerPrefs.GetOkane();
        money += 1;
        GamerPrefs.SetOkane(money);
        this.money.text = GamerPrefs.GetOkane().ToString();
    }

}
