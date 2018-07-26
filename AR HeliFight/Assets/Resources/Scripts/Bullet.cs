using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed;
    Vector3 direction;
    float move;
    float startTime;
    PlayerHelper playerHelper;


    // Use this for initialization
    void Start()
    {
        startTime = Time.time;
        playerHelper = transform.GetComponentInParent<PlayerHelper>();
        speed = playerHelper.AttackSpeed;
        transform.LookAt(playerHelper.gameController.Controllers[playerHelper.Target].transform.position);
        transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime > 5f) Destroy(gameObject);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
