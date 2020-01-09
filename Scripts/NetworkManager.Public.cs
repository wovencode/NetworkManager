// =======================================================================================
// NetworkManager
// by Weaver (Fhiz)
// MIT licensed
//
// This part of the NetworkManager contains all public functions. That comprises all
// methods that are called on the NetworkManager from UI elements in order to check for
// an action or perform an action (like "Can we register an account with password X and
// name Y?" or "Now register an account with password X and name Y").
//
// =======================================================================================

using wovencode;
using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace wovencode
{
	
	// ===================================================================================
	// NetworkManager
	// ===================================================================================
	public partial class NetworkManager
	{
   
   		// -------------------------------------------------------------------------------
   		// CanClick
   		// can any network related button be clicked at the moment?
   		// -------------------------------------------------------------------------------
		public bool CanClick()
		{
			return (!isNetworkActive && !IsConnecting());
		}
		
	   	// -------------------------------------------------------------------------------
   		// CanInput
   		// can the user input data into any network related input field right now?
   		// -------------------------------------------------------------------------------
		public bool CanInput()
		{
			return (!isNetworkActive || !IsConnecting());
		}
   		
   		// -------------------------------------------------------------------------------
   		// CanCancel
   		// can we cancel what we are currently doing?
   		// -------------------------------------------------------------------------------
		public bool CanCancel()
		{
			return IsConnecting();
		}
   		
		// -------------------------------------------------------------------------------
   		// CanLoginAccount
   		// can we login into an existing account with the provided name and password?
   		// -------------------------------------------------------------------------------
		public bool CanLoginAccount(string _name, string _password)
		{
			return !isNetworkActive && 
				Tools.IsAllowedName(_name) && 
				Tools.IsAllowedPassword(_password) &&
				!IsConnecting();
		}

		// -------------------------------------------------------------------------------
   		// CanRegisterAccount
   		// can we register a new account with the provided name and password?
   		// -------------------------------------------------------------------------------
		public bool CanRegisterAccount(string _name, string _password)
		{
			return !isNetworkActive &&
				Tools.IsAllowedName(_name) && 
				Tools.IsAllowedPassword(_password) &&
				!IsConnecting();
		}

		// -------------------------------------------------------------------------------
   		// CanDeleteAccount
   		// can we delete an account with the provided name and password?
   		// -------------------------------------------------------------------------------
		public bool CanDeleteAccount(string _name, string _password)
		{
			return !isNetworkActive &&
				Tools.IsAllowedName(_name) && 
				Tools.IsAllowedPassword(_password) &&
				!IsConnecting();
		}
		
		// -------------------------------------------------------------------------------
   		// CanStartServer
   		// can we start a server (host only) right now?
   		// -------------------------------------------------------------------------------
		public bool CanStartServer()
		{
			return (Application.platform != RuntimePlatform.WebGLPlayer && 
					!isNetworkActive &&
					!IsConnecting());
		}

		// -------------------------------------------------------------------------------
   		// TryLoginAccount
   		// we try to login into an existing account using name and password provided
   		// -------------------------------------------------------------------------------
		public void TryLoginAccount(string _name, string _password, bool hostMode=false)
		{
			
			((wovencode.NetworkAuthenticator)authenticator).userName = _name;
			((wovencode.NetworkAuthenticator)authenticator).userPassword = _password;
			
			if (hostMode)
			{
				((wovencode.NetworkAuthenticator)authenticator).accountAction = NetworkAuthenticator.NetworkActionLoginLocal;
				StartHost();
			}
			else
			{
				((wovencode.NetworkAuthenticator)authenticator).accountAction = NetworkAuthenticator.NetworkActionLoginRemote;
				StartClient();
			}
		}
		
		// -------------------------------------------------------------------------------
   		// TryRegisterAccount
   		// we try to register a new account with name and password provided
   		// -------------------------------------------------------------------------------
		public void TryRegisterAccount(string _name, string _password, bool hostMode=false)
		{
			
			((wovencode.NetworkAuthenticator)authenticator).userName = _name;
			((wovencode.NetworkAuthenticator)authenticator).userPassword = _password;
			
			if (hostMode)
			{
				((wovencode.NetworkAuthenticator)authenticator).accountAction = NetworkAuthenticator.NetworkActionRegisterLocal;
				StartHost();
			}
			else
			{
				((wovencode.NetworkAuthenticator)authenticator).accountAction = NetworkAuthenticator.NetworkActionRegisterRemote;
				StartClient();
			}
		}
		
		// -------------------------------------------------------------------------------
   		// TryDeleteAccount
   		// we try to delete an existing account according to its name and password
   		// -------------------------------------------------------------------------------
		public void TryDeleteAccount(string _name, string _password, bool hostMode=false)
		{
			
			((wovencode.NetworkAuthenticator)authenticator).userName = _name;
			((wovencode.NetworkAuthenticator)authenticator).userPassword = _password;
			
			if (hostMode)
			{
				((wovencode.NetworkAuthenticator)authenticator).accountAction = NetworkAuthenticator.NetworkActionDeleteLocal;
				StartHost();
			}
			else
			{
				((wovencode.NetworkAuthenticator)authenticator).accountAction = NetworkAuthenticator.NetworkActionDeleteRemote;
				StartClient();
			}
		}

		// -------------------------------------------------------------------------------
   		// TryStartServer
   		// we try to start a server (host only) if possible
   		// -------------------------------------------------------------------------------
		public void TryStartServer()
		{
			if (!CanStartServer()) return;
			StartServer();
		}
	
		// -------------------------------------------------------------------------------
   		// TryCancel
   		// we try to cancel whatever we are doing right now
   		// -------------------------------------------------------------------------------
		public void TryCancel()
		{
			StopClient();
		}
	
		// -------------------------------------------------------------------------------
   		// CanHostAndPlay
   		// can we host and play on the same machine right now?
   		// -------------------------------------------------------------------------------
		public bool CanHostAndPlay(string _name, string _password)
		{
			return Application.platform != RuntimePlatform.WebGLPlayer && 
				!isNetworkActive && 
				Tools.IsAllowedName(_name) && 
				Tools.IsAllowedPassword(_password) &&
				!IsConnecting();
		}
	
		// -------------------------------------------------------------------------------
	
	}

}

// =======================================================================================