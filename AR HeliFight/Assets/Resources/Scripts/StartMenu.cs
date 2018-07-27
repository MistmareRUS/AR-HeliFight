using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.SceneManagement;
using GooglePlayGames.BasicApi;
using Com.Google.Android.Gms.Games;

public class StartMenu : MonoBehaviour
{
    public Text infoBlock;
    public GameObject listener;

    const int minOpponents = 1;
    const int maxOpponents = 3;
    const int GameVariant = 0;
    List<Participant> participants;
    RealTimeMultiplayerListener MPlistener;

    // Use this for initialization
    void Start()
    {
        MPlistener = listener.GetComponent<MultiPlayerHelper>() as RealTimeMultiplayerListener;
        infoBlock.text += "Привет!";
    }

    // Update is called once per frame
    void Update()
    {

    }
    //подключение к аккаунту
    public void LogIn()
    {
        Social.localUser.Authenticate((bool succes) =>
        {
            if (succes)
            {
                infoBlock.text += "Подключено.";
            }
            else
            {
                infoBlock.text += "Не удалось  подключиться, попробуйте снова";
                return;
            }
        });
    } // Добавить подтверждение, что подключение успешно
    public void QuickGame()
    {
        PlayGamesPlatform.Instance.RealTime.CreateQuickGame(minOpponents, maxOpponents, GameVariant, MPlistener);
        infoBlock.text += "Начинаю быструю игру";
    }
    public void InviteToGameGooglePlus()
    {
        PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen(minOpponents, maxOpponents, GameVariant, MPlistener);
    }
    public void LookAtInvites()
    {
        PlayGamesPlatform.Instance.RealTime.AcceptFromInbox(MPlistener);
    }
    public void SortPlayers()
    {
        participants = PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();        
        AppController.PlayerCount = PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants().Count;
        AppController.PlayerA = participants[0].ParticipantId;
        AppController.PlayerB = participants[1].ParticipantId;
        if (AppController.PlayerCount > 2)
        {
            AppController.PlayerC = participants[2].ParticipantId;
        }
        if (AppController.PlayerCount > 3)
        {
            AppController.PlayerC = participants[3].ParticipantId;
        }
        //Определеям номер игрока в стат. классе
        string myPlayer = PlayGamesPlatform.Instance.RealTime.GetSelf().ParticipantId;
        if (myPlayer == AppController.PlayerA) AppController.currentPlayer = 0;
        else if (myPlayer == AppController.PlayerB) AppController.currentPlayer = 1;
        else if (myPlayer == AppController.PlayerC) AppController.currentPlayer = 2;
        else if (myPlayer == AppController.PlayerD) AppController.currentPlayer = 3;

        byte[] message = AppController.MessageCodingReliable("Model&1model&1color&");//ID отправляется автоматом, проверить
        PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, message);
        SceneManager.LoadScene("Main");
    }




    //public void ShowSpesialLeads(){((PlayGamesPlatform) Social.Active).ShowLeaderboardUI(GPGSIds.leaderboard_champions);}
    //public void Out(){((PlayGamesPlatform) Social.Active).SignOut();}

}
