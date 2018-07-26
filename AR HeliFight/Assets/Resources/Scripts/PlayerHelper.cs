using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerHelper : MonoBehaviour
{
    public GameObject LeftWeapon;
    public GameObject RightWeapon;
    public GameObject Minigun;
    public GameObject BulletPrefab;

    public float HP;
    public int Target=-1;
    [SerializeField]
    public float AttackSpeed=10f;

     public GameController gameController;

    void Start()
    {
        HP = 100f;
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void Update()
    {

    }
    public void StartFire()
    {
        CancelInvoke("MinigunFire");
        InvokeRepeating("MinigunFire", 0, 2);
    }
    void  MinigunFire()
    {
        GameObject bullet = Instantiate(BulletPrefab, Minigun.transform.position, Quaternion.identity,this.transform);
    }
   
    public void BonusAniation()
    {
        //shield/boost/heal/LR/RR...
    }
    public void EndRound()
    {
        //if win/lose
    }
    public void TakeDamage(float damage)
    {
        
    }

    public void TakeHeal(float heal)
    {
        
    }

    public void Shield()
    {
       //активировать таймер и щит
    }
        
    public void Boost()
    {
        
    } 
    public void LeftRocket()
    {

    }
    public void RightRocket()
    {

    }
}
