using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.UI;
using GooglePlayGames.BasicApi;
using System;

public class MultiPlayerHelper : MonoBehaviour, RealTimeMultiplayerListener
{
    public StartMenu startMenu;
    public GameController gameController;
    public Text InMenuInfoText;
    Text inGameInfoText;

     
    
    public void OnLeftRoom()
    {
        //throw new System.NotImplementedException();
        InMenuInfoText.text += "вышел из комнаты";
        //Обнулить стринги игроков
    }

    public void OnParticipantLeft(Participant participant)
    {
        //throw new System.NotImplementedException();
        InMenuInfoText.text += "кто-то ушел.";
        //2е осталось?
    }

    public void OnPeersConnected(string[] participantIds)
    {
        //throw new System.NotImplementedException();
        InMenuInfoText.text += "подключение";
    }

    public void OnPeersDisconnected(string[] participantIds)
    {
        //throw new System.NotImplementedException();
        InMenuInfoText.text += "отключение";
        //2е осталось?
    }
    //обработка входящего сообщения!!
    public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
    {
        gameController.infotext_in.text = "start: ";
        if (isReliable)
        {
            string mesage = AppController.MessageDecodingReliable(data);
            //игрок sender...такой-то, такой-то
            //добавить вариации моделей и цветов(через добавленный класс)
        }

        else
        {
           string backText= AppController.MessageDecodingNonReliable(data);
            gameController.infotext_in.text += backText;
            string[] array = backText.Split('&');
            
            if (array[0] == "Move")
            {                
                //преобразуем принятое  сообщение
                int playerNumber = Convert.ToInt32(array[1]);
                string tempPosintion = array[2].Remove(0,1).Remove(array[2].Length - 2);
                string[] position = tempPosintion.Split(',');
                string tempRotation = array[3].Remove(0,1).Remove(array[3].Length - 2);
                string[] rotation = tempRotation.Split(',');
                float hp = Convert.ToSingle(array[4]);
                int target = Convert.ToInt32(array[4]);
                //распределяем принятые данные
                gameController.Controllers[playerNumber].transform.localPosition=Vector3.Lerp(gameController.Controllers[playerNumber].transform.localPosition,
                                                                                            (new Vector3(Convert.ToSingle(position[0].Trim())
                                                                                            , Convert.ToSingle(position[1].Trim())
                                                                                            , Convert.ToSingle(position[2].Trim())))
                                                                                            ,0.5f);
                gameController.Controllers[playerNumber].transform.localEulerAngles=Vector3.Lerp(gameController.Controllers[playerNumber].transform.localEulerAngles,
                                                                                        (new Vector3(Convert.ToSingle(rotation[0].Trim())
                                                                                        , Convert.ToSingle(rotation[1].Trim())
                                                                                        , Convert.ToSingle(rotation[2].Trim())))
                                                                                        ,0.5f);
                gameController.Players[playerNumber].HP = hp;
                gameController.Players[playerNumber].Target = target;
                
                gameController.infotext_in.text += ". end";
            }

        }
    }

    public void OnRoomConnected(bool success)
    {
        //вызов метода для сортировки игроков
        startMenu.SortPlayers();
    }

    public void OnRoomSetupProgress(float percent)
    {
        //загрузка комнаты для сбора пати
        PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI();
        //нужна возможность отмены!!
    }

    void Start()
    {
        PlayGamesClientConfiguration configuration = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        InMenuInfoText.text += "Activated.";
        DontDestroyOnLoad(gameObject);
    }
}
