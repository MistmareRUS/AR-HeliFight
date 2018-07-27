using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    int target;
    PlayerHelper playerHelper;
    public int ParentHeli;

    // Use this for initialization
    void Start()
    {
        ParentHeli = transform.parent.GetComponent<PlayerHelper>().PlayerIndex;
        playerHelper = transform.GetComponentInParent<PlayerHelper>();
        transform.LookAt(playerHelper.gameController.Controllers[playerHelper.Target].transform.position);
        target = playerHelper.Target;
        transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position=Vector3.Lerp(transform.position, playerHelper.gameController.Controllers[target].transform.position,0.2f);
        transform.LookAt(playerHelper.gameController.Controllers[target].transform);
    }

}
