using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameLoot : MonoBehaviour
{
    public GameObject leftColumn;
    Player player;
    Game game;
    public Text remainingText;
    public GameManager gameManager;

    void Start()
    {
        player = FindObjectOfType<Player>();
        game = FindObjectOfType<Game>();
        gameManager = FindObjectOfType<GameManager>();
        StartCoroutine(AddLoot());
    }

    private IEnumerator AddLoot()
    {
        //yield return InsertLoot();
        yield return GetAllInfo();
    }

    private IEnumerator InsertLoot()
    {
        GameSerializable gameSerializable = new GameSerializable();
        gameSerializable.Id = player.Id;
        gameSerializable.IdPlayer = player.Id;
        gameSerializable.RemainingLoot = 1000;
     
        using (UnityWebRequest httpClient = new UnityWebRequest(player.HttpServerAddress + "api/Game/InsertNewGame", "POST"))
        {
            string gameData = JsonUtility.ToJson(gameSerializable);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(gameData);
            httpClient.uploadHandler = new UploadHandlerRaw(bodyRaw);
            httpClient.SetRequestHeader("Content-type", "application/json");
            httpClient.SetRequestHeader("Authorization", "bearer " + player.Token);

            httpClient.certificateHandler = new BypassCertificate();
            yield return httpClient.SendWebRequest();

            if (httpClient.isNetworkError || httpClient.isHttpError)
            {
                throw new Exception("InsertLoot: " + httpClient.error);
            }
            else
            {
                Debug.Log("InsertLoot: " + httpClient.responseCode);
            }
        }

    }

    public IEnumerator GetAllInfo()
    { 
        UnityWebRequest httpClient = new UnityWebRequest(player.HttpServerAddress + "api/Game/InfoAllGame", "GET");
        
        httpClient.SetRequestHeader("Authorization", "bearer " + player.Token);
        httpClient.SetRequestHeader("Accept", "application/json");

        httpClient.downloadHandler = new DownloadHandlerBuffer();

        httpClient.certificateHandler = new BypassCertificate();
        yield return httpClient.SendWebRequest();

        if (httpClient.isNetworkError || httpClient.isHttpError)
        {
            throw new Exception("Helper > GetPlayerInfo: " + httpClient.error);
        }
        else
        {
            Debug.Log(httpClient.downloadHandler.text);
            GameSerializable gameSerializable = JsonUtility.FromJson<GameSerializable>(httpClient.downloadHandler.text);
            game.Id = gameSerializable.Id;
            game.IdPlayer = gameSerializable.IdPlayer;
            game.RemainingLoot = gameSerializable.RemainingLoot;
            game.LastLoots = gameSerializable.LastLoots;
            game.TopLoots = gameSerializable.TopLoots;

            AddText(game.RemainingLoot);
        }

        httpClient.Dispose();
    }

    public void AddText(int num)
    {
        remainingText.text = (num - gameManager.score).ToString();
    }


    public class BypassCertificate : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            //Simply return true no matter what 
            return true;
        }
    }
}
