using GooglePlayGames;
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
    public GameObject RocketPrefab;

    public float HP;
    public int Target = -1;
    public int PlayerIndex=-1;
    [SerializeField]
    public float AttackSpeed = 10f;

    public GameController gameController;

    void Start()
    {
        HP = 100f;
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void Update()
    {

    }
    //атака минигана
    public void StartFire()
    {
        CancelInvoke("MinigunFire");
        InvokeRepeating("MinigunFire", 0, 2);
    }
    void MinigunFire()
    {
        GameObject bullet = Instantiate(BulletPrefab, Minigun.transform.position, Quaternion.identity, this.transform);
    }
    //Распознание входящих коллайдеров
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            HP -= 1f;
#if !UNITY_EDITOR
            if (other.GetComponent<Bullet>().ParentHeli == AppController.currentPlayer)
            {
                byte[] message = AppController.MessageCodingReliable("Bullet&" + AppController.currentPlayer);
                PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, message);
            }
#endif           
        }
        if (other.gameObject.CompareTag("Rocket"))
        {
            Destroy(other.gameObject);
            HP -= 20f;
#if !UNITY_EDITOR
            if (other.GetComponent<Rocket>().ParentHeli == AppController.currentPlayer)
            {
                byte[] message = AppController.MessageCodingReliable("Rocket&" + AppController.currentPlayer);
                PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, message);
            }
#endif

        }
    }
        public void BonusAniation()
    {
        //shield/boost/heal/LR/RR...
    }
    public void EndRound()
    {
        //if win/lose
    }
    
    public void LRocket()
    {
        GameObject rocket = Instantiate(RocketPrefab, LeftWeapon.transform.position, Quaternion.identity, this.transform);
    }
    public void RRocket()
    {
        GameObject rocket = Instantiate(RocketPrefab, RightWeapon.transform.position, Quaternion.identity, this.transform);
    }


    public void Shield()
    {
        //активировать таймер и щит
    }

    public void Boost()
    {

    }
}
