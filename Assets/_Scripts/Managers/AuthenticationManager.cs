using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using DatabaseModels.Requests;
using DatabaseModels.Responses;
using UnityEngine;

public class AuthenticationManager : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;

    private void Start()
    {
        if (SessionStore.UserData == null)
        {
            _uiManager._authenticationMenu.SetActive(true);
        }
    }
    
    public async void Login()
    {
        var credentials = _uiManager.GetAuthenticationCredentials();

        var authResponse = await HttpClient.Post<AuthenticationResponse>(
            $"{SessionStore.ApiUrl}/auth/login",
            new AuthenticationRequest
            {
                Username = credentials.username, Password = credentials.password
            });

        if (authResponse == null)
        {
            _uiManager.SetAuthenticationErrorMessage("Server is unavailable.");
            return;
        }
        
        if (authResponse.IsError)
        {
            _uiManager.SetAuthenticationErrorMessage(authResponse.ErrorMessage);
            return;
        }

        SessionStore.Jwt = authResponse.Token;
        
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(authResponse.Token);

        SessionStore.UserData = new UserData
        {
            Id = Int32.Parse(token.Claims.First(c => c.Type == "id").Value),
            Username = credentials.username,
            Password = credentials.password
        };
        
        _uiManager._authenticationMenu.SetActive(false);
    }

    public async void Register()
    {
        var credentials = _uiManager.GetAuthenticationCredentials();

        var authResponse = await HttpClient.Post<AuthenticationResponse>(
            $"{SessionStore.ApiUrl}/auth/register",
            new AuthenticationRequest
            {
                Username = credentials.username, Password = credentials.password
            });

        if (authResponse == null)
        {
            _uiManager.SetAuthenticationErrorMessage("Server is unavailable.");
            return;
        }
        
        if (authResponse.IsError)
        {
            _uiManager.SetAuthenticationErrorMessage(authResponse.ErrorMessage);
            return;
        }

        SessionStore.Jwt = authResponse.Token;
        
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(authResponse.Token);

        SessionStore.UserData = new UserData
        {
            Id = Int32.Parse(token.Claims.First(c => c.Type == "id").Value),
            Username = credentials.username,
            Password = credentials.password
        };
        
        _uiManager._authenticationMenu.SetActive(false);
    }
}
