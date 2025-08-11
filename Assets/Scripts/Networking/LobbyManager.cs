using TMPro;
using UnityEngine;
using System;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance;

    [Header("Lobby creation")]
    [SerializeField] private TMP_InputField createLobbyNameField;

    public string joinedLobbyId;
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    public async void CreateLobby()
    {
        Lobby createdLobby = null;
        try
        {
            createdLobby = await LobbyService.Instance.CreateLobbyAsync(createLobbyNameField.text, 2);
            joinedLobbyId = createdLobby.Id;
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}
