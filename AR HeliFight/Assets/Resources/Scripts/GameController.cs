using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject[] Respowns;//здесь появляются все участники
    public GameObject Center;
   
    public GameObject[] HelicopterModels;//сюда закинуть варианты моделей
    public VirtualJoystick joystick;
    public Slider CurrentPlayer;//здоровье игрока
    public Slider[] PlayersSlider;//здоровье остальных участников  
    RealTimeMultiplayerListener MPlistener;
    MultiPlayerHelper multiPlayer;

    public Text infotext_pos;
    public Text infotext_in;
    public Text infotext_out;

    private float moveSpeed;
    public GameObject[] PlayersPrefabs;
    public PlayerHelper[] Players;//информация о всех игроках    // AppController.PlayerCount  индекс текущего игрока
    public CharacterController[] Controllers;//управление всеми моделями на поле
    List<int> targets;

    //bool isHost = false;


    // Use this for initialization
    void Start()
    {

#if UNITY_EDITOR
        AppController.currentPlayer = 0;
        AppController.PlayerA = "a";
        AppController.PlayerB = "b";
        AppController.PlayerC = "c";
        AppController.PlayerD = "d";
        AppController.PlayerCount = 4;
#endif
#if !UNITY_EDITOR
        //связь текущего контроллера и мультиплеера
        multiPlayer = GameObject.Find("MultiPlayerHelper").GetComponent<MultiPlayerHelper>();
        multiPlayer.gameController = this;
       
#endif


        PlayersPrefabs = new GameObject[AppController.PlayerCount];//Инициализируем массив игроков из статического класса
        Players = new PlayerHelper[AppController.PlayerCount];//инициализируем массив хелперов для получения инфо о состоянии
        Controllers = new CharacterController[AppController.PlayerCount];//инициализируем массив контроллеров для управленияна моделями поле

        SetPlayers();
        for (int i = 0; i < Players.Length; i++)
        {
            Players[i] = PlayersPrefabs[i].GetComponent<PlayerHelper>();
        }
        moveSpeed = 5;
        CurrentPlayer.maxValue = 100;
        CurrentPlayer.value = Players[AppController.currentPlayer].HP;
        if (AppController.PlayerCount < 4) PlayersSlider[2].gameObject.SetActive(false);
        if (AppController.PlayerCount < 3) PlayersSlider[1].gameObject.SetActive(false);
        foreach (var item in PlayersSlider)
        {
            item.maxValue = 100;
            item.value = 100;
        }
        SetControler();
        //набор возможных целей
        targets = new List<int>();
        for (int i = 0; i < AppController.PlayerCount; i++)
        {
            if (i != AppController.currentPlayer)
            {
                targets.Add(i);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        MessageUpdate();
        print(Players[AppController.currentPlayer].Target);
        if (!(Players[AppController.currentPlayer].Target == -1)) Debug.DrawLine(Players[AppController.currentPlayer].Minigun.transform.position, Players[Players[AppController.currentPlayer].Target].transform.position, Color.red);
    }

    private void FixedUpdate()
    {
        //считать движениеж
        MovePlayer();//слать сообщение хосту
#if !UNITY_EDITOR
        SetSliders();
#endif
    }
    //движение контроллера по индексу текущего игрока
    void MovePlayer()
    {
        if (joystick.Horizontal() == 0 && joystick.Vertical() == 0) return;
        Vector3 dir = Vector3.zero;
        dir.x = joystick.Horizontal() * moveSpeed;
        dir.z = joystick.Vertical() * moveSpeed;

        dir = Camera.main.transform.TransformDirection(dir);
        Controllers[AppController.currentPlayer].SimpleMove(dir);

        Vector3 lookDir = dir + Controllers[AppController.currentPlayer].transform.position;
        Vector3 rot = new Vector3(lookDir.x, Controllers[AppController.currentPlayer].transform.position.y, lookDir.z);
        Controllers[AppController.currentPlayer].transform.LookAt(rot);

        byte[] message = AppController.MessageCodingNonReliable("Move&" + AppController.currentPlayer + "&"
                                                                + PlayersPrefabs[AppController.currentPlayer].transform.localPosition + "&"
                                                                + PlayersPrefabs[AppController.currentPlayer].transform.eulerAngles + "&"
                                                                + Players[AppController.currentPlayer].HP + "&"
                                                                + Players[AppController.currentPlayer].Target + "&"
                                                                );
        infotext_out.text = AppController.currentPlayer + "&"
                        + Controllers[AppController.currentPlayer].transform.position + "&"//отсюда
                        + dir + "&"//сюда
                        + Controllers[AppController.currentPlayer].transform.localEulerAngles + "&"
                        + rot;
#if !UNITY_EDITOR
        PlayGamesPlatform.Instance.RealTime.SendMessageToAll(false, message);
#endif
        #region //проверка отправляемго сообщения        
        string backText = AppController.MessageDecodingNonReliable(message);
        infotext_out.text = backText;
        #endregion
    }
    public void xMove()
    {
        Controllers[0].transform.localPosition = Vector3.Lerp(Controllers[0].transform.localPosition, Vector3.zero, 0.5f);
    }
    public void xRotate()
    {
        Controllers[0].transform.eulerAngles = Vector3.Lerp(Controllers[0].transform.eulerAngles, new Vector3(0f, 45f, 0f), 0.5f);// new Vector3(0f, 45f, 0f);// Vector3.Lerp(Controllers[0].transform.localPosition, Vector3.zero, 0.5f);//(new Vector3(0f, 45f, 0f), 0.1f));(new Quaternion(0.6f,0.6f,0.6f,0.6f));// = Vector3.Lerp(Controllers[0].transform.localRotation.eulerAngles, new Vector3(0f, 45f, 0f), 0.1f);
    }
    //временный метод для отслеживания сообщений
    void MessageUpdate()
    {
        infotext_pos.text = "0- pos:" + Controllers[0].transform.localPosition.ToString()
                            + " rot:" + Controllers[0].transform.localEulerAngles.ToString() +
                            "\n1- pos:" + Controllers[1].transform.localPosition.ToString()
                            + " rot:" + Controllers[1].transform.localEulerAngles.ToString();
    }
    //передвижение тел на карте


    /*public void StartRocket(string leftRight) //отправить:   Players[AppController.currentPlayer]    №/атака...  //добавить номер чаилда!!!
    {
        Transform startPosition;
        if (leftRight == "l")
        {
            startPosition = Players[AppController.currentPlayer].transform.GetChild(0).transform;
        }
        else if (leftRight == "r")
        {
            startPosition = Players[AppController.currentPlayer].transform.GetChild(0).transform;
        }

        print("Пиу-пиу ракетой");
        if (Players[AppController.currentPlayer].CurrentTarget != null)
        {
            Players[AppController.currentPlayer].CurrentTarget.TakeDamage(10f);
        }
        else
        {
            //сообщить об отсутствии цели
        }
        //отрисовать путь ракет из точки transform!!!!

    }*/

   
    

    //всех игроков по своим местам
    void SetPlayers()
    {
        //расположение игроков(префабов)   модели должны считываться из первой сцены, временно все 1!!!!!!!
        //можно переписать без установки родителей
        PlayersPrefabs[0] = Instantiate(HelicopterModels[0]);
        PlayersPrefabs[0].transform.SetParent(Respowns[0].transform);       
        PlayersPrefabs[0].name = "Player_0";
        PlayersPrefabs[1] = Instantiate(HelicopterModels[0]);
        PlayersPrefabs[1].transform.SetParent(Respowns[1].transform);        
        PlayersPrefabs[1].name = "Player_1";
        //если игроков 4-ро, то эти респауны
        if (AppController.PlayerC != string.Empty && AppController.PlayerD != string.Empty)
        {
            PlayersPrefabs[2] = Instantiate(HelicopterModels[0]);
            PlayersPrefabs[3] = Instantiate(HelicopterModels[0]);
            PlayersPrefabs[2].transform.SetParent(Respowns[2].transform);           
            PlayersPrefabs[3].transform.SetParent(Respowns[3].transform);           
            PlayersPrefabs[2].name = "Player_2";
            PlayersPrefabs[3].name = "Player_3";
        }
        //если 3-е, +этот
        else if (AppController.PlayerC != string.Empty && AppController.PlayerD == string.Empty)
        {
            //добавить смещение центра сцены !!!
            PlayersPrefabs[2] = Instantiate(HelicopterModels[0]);
            PlayersPrefabs[2].transform.SetParent(Respowns[4].transform);            
            PlayersPrefabs[2].name = "Player_2";
        }
        foreach (var item in PlayersPrefabs)
        {
            item.transform.localPosition = Vector3.zero;
            item.transform.SetParent(null);
            item.transform.LookAt(Center.transform.position);
        }
    }
    //установка ХарактерКонтроллеров и ПлеерХелперов
    void SetControler()
    {
        for (int i = 0; i < PlayersPrefabs.Length; i++)
        {
            Players[i] = PlayersPrefabs[i].GetComponent<PlayerHelper>() as PlayerHelper;
            Controllers[i] = PlayersPrefabs[i].GetComponent<CharacterController>();
        }
    }
    //Регулярное обновление полосох ХП на основе currentHP массива
    void SetSliders()
    {
        int count = 0;
        for (int i = 0; i < AppController.PlayerCount; i++)
        {
            if (i == AppController.currentPlayer)
            {
                CurrentPlayer.value = Players[i].HP;
            }
            else
            {
                PlayersSlider[count].value = Players[i].HP;
                count++;
            }
        }
    }
    //Установка цели из списка целей
    public void TargetSet(int index)
    {
        int ct = Players[AppController.currentPlayer].Target;
        if (index == 1)
        {
            ct++;
            if (!targets.Contains(ct)) ct++;
            if (ct >= AppController.PlayerCount) ct = 0;
            if (!targets.Contains(ct)) ct++;
        }
        else if (index == 0)
        {
            ct--;
            if (!targets.Contains(ct)) ct--;
            if (ct < 0) ct = AppController.PlayerCount - 1;
            if (!targets.Contains(ct)) ct--;
        }
        Players[AppController.currentPlayer].Target = ct;
        Players[AppController.currentPlayer].StartFire();
    }


    //Окончание игры
    public void EndGame()
    {
        //начислить баллы, окно геймовера
        //очистить переменные игровых моделей
        //Очистить инфо текст GPS?
    }




    //выполняется хостом
    public void AsHostPlayer()
    {

    }

    //выполняется клиентом хоста
    public void AsClientPlayer()
    {

    }
    void GameUpdate(bool isHost)
    {
        if (isHost) AsHostPlayer();
        else AsClientPlayer();
    }
}
