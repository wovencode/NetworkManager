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
   		// CanLoginUser
   		// can we login into an existing user with the provided name and password?
   		// -------------------------------------------------------------------------------
		public bool CanLoginUser(string name, string password)
		{
			return !isNetworkActive && 
				Tools.IsAllowedName(name) && 
				Tools.IsAllowedPassword(password) &&
				!IsConnecting();
		}

		// -------------------------------------------------------------------------------
   		// CanRegisterUser
   		// can we register a new user with the provided name and password?
   		// -------------------------------------------------------------------------------
		public bool CanRegisterUser(string name, string password)
		{
			return !isNetworkActive &&
				Tools.IsAllowedName(name) && 
				Tools.IsAllowedPassword(password) &&
				!IsConnecting();
		}

		// -------------------------------------------------------------------------------
   		// CanDeleteUser
   		// can we delete an user with the provided name and password?
   		// -------------------------------------------------------------------------------
		public bool CanDeleteUser(string name, string password)
		{
			return !isNetworkActive &&
				Tools.IsAllowedName(name) && 
				Tools.IsAllowedPassword(password) &&
				!IsConnecting();
		}
		
		// -------------------------------------------------------------------------------
   		// CanChangePasswordUser
   		// can we change the provided users password?
   		// -------------------------------------------------------------------------------
		public bool CanChangePasswordUser(string name, string oldpassword, string newpassword)
		{
			return !isNetworkActive &&
				Tools.IsAllowedName(name) && 
				Tools.IsAllowedPassword(oldpassword) &&
				Tools.IsAllowedPassword(newpassword) &&
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
   		// TryLoginUser
   		// we try to login into an existing user using name and password provided
   		// -------------------------------------------------------------------------------
		public void TryLoginUser(string name, string password, bool hostMode=false)
		{
			
			((wovencode.NetworkAuthenticator)authenticator).userName 		= name;
			((wovencode.NetworkAuthenticator)authenticator).userPassword 	= password;
			((wovencode.NetworkAuthenticator)authenticator).userAction 		= NetworkAuthenticator.NetworkActionLoginUser;
			
			if (hostMode)
				StartHost();
			else
				StartClient();
			
		}
		
		// -------------------------------------------------------------------------------
   		// TryRegisterUser
   		// we try to register a new User with name and password provided
   		// -------------------------------------------------------------------------------
		public void TryRegisterUser(string name, string password, bool hostMode=false)
		{
			
			((wovencode.NetworkAuthenticator)authenticator).userName 		= name;
			((wovencode.NetworkAuthenticator)authenticator).userPassword 	= password;
			((wovencode.NetworkAuthenticator)authenticator).userAction 		= NetworkAuthenticator.NetworkActionRegisterUser;
			
			if (hostMode)
				StartHost();
			else
				StartClient();
			
		}
		
		// -------------------------------------------------------------------------------
   		// TryDeleteUser
   		// we try to delete an existing User according to its name and password
   		// -------------------------------------------------------------------------------
		public void TryDeleteUser(string name, string password, bool hostMode=false)
		{
			
			((wovencode.NetworkAuthenticator)authenticator).userName 		= name;
			((wovencode.NetworkAuthenticator)authenticator).userPassword 	= password;
			((wovencode.NetworkAuthenticator)authenticator).userAction 		= NetworkAuthenticator.NetworkActionDeleteUser;
			
			if (hostMode)
				StartHost();
			else
				StartClient();

		}
		
		// -------------------------------------------------------------------------------
   		// TryChangePasswordUser
   		// we try to delete an existing User according to its name and password
   		// -------------------------------------------------------------------------------
		public void TryChangePasswordUser(string name, string oldpassword, string newpassword, bool hostMode=false)
		{
			
			((wovencode.NetworkAuthenticator)authenticator).userName 		= name;
			((wovencode.NetworkAuthenticator)authenticator).userPassword 	= oldpassword;
			((wovencode.NetworkAuthenticator)authenticator).newPassword 	= newpassword;
			((wovencode.NetworkAuthenticator)authenticator).userAction 		= NetworkAuthenticator.NetworkActionChangePasswordUser;
			
			if (hostMode)
				StartHost();
			else
				StartClient();

		}
		
		// -------------------------------------------------------------------------------
   		// TryConfirmUser
   		// we try to confirm an existing User according to its name and password
   		// -------------------------------------------------------------------------------
		public void TryConfirmUser(string name, string password, bool hostMode=false)
		{
			
			((wovencode.NetworkAuthenticator)authenticator).userName 		= name;
			((wovencode.NetworkAuthenticator)authenticator).userPassword 	= password;
			((wovencode.NetworkAuthenticator)authenticator).userAction 		= NetworkAuthenticator.NetworkActionConfirmUser;
			
			if (hostMode)
				StartHost();
			else
				StartClient();

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
		public bool CanHostAndPlay(string name, string password, string newpassword="")
		{
			return Application.platform != RuntimePlatform.WebGLPlayer && 
				!isNetworkActive && 
				Tools.IsAllowedName(name) && 
				Tools.IsAllowedPassword(password) &&
				(String.IsNullOrWhiteSpace(newpassword) || Tools.IsAllowedPassword(newpassword)) &&
				!IsConnecting();
		}
	
		// -------------------------------------------------------------------------------
	
	}

}

// =======================================================================================