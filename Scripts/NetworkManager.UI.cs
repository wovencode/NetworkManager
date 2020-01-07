// =======================================================================================
// NetworkManager UI
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using wovencode;
using System;
//using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
		public bool CanClick()
		{
			return (!isNetworkActive && !IsConnecting());
		}
		
	   	// -------------------------------------------------------------------------------
		public bool CanInput()
		{
			return (!isNetworkActive || !IsConnecting());
		}
   		
   		// -------------------------------------------------------------------------------
		public bool CanCancel()
		{
			return IsConnecting();
		}
   		
		// -------------------------------------------------------------------------------
		public bool CanLoginAccount(string _name, string _password)
		{
			return !isNetworkActive && 
				Tools.IsAllowedName(_name) && 
				Tools.IsAllowedPassword(_password) &&
				!IsConnecting();
		}

		// -------------------------------------------------------------------------------
		public bool CanRegisterAccount(string _name, string _password)
		{
			return !isNetworkActive &&
				Tools.IsAllowedName(_name) && 
				Tools.IsAllowedPassword(_password) &&
				!IsConnecting();
		}

		// -------------------------------------------------------------------------------
		public bool CanDeleteAccount(string _name, string _password)
		{
			return !isNetworkActive &&
				Tools.IsAllowedName(_name) && 
				Tools.IsAllowedPassword(_password) &&
				!IsConnecting();
		}
		
		// -------------------------------------------------------------------------------
		public bool CanStartServer()
		{
			return (Application.platform != RuntimePlatform.WebGLPlayer && 
					!isNetworkActive &&
					!IsConnecting());
		}

		// -------------------------------------------------------------------------------
		public void TryLoginAccount(string _name, string _password, bool hostMode=false)
		{
			
			((wovencode.NetworkAuthenticator)authenticator).accountName = _name;
			((wovencode.NetworkAuthenticator)authenticator).accountPassword = _password;
			
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
		public void TryRegisterAccount(string _name, string _password, bool hostMode=false)
		{
			
			((wovencode.NetworkAuthenticator)authenticator).accountName = _name;
			((wovencode.NetworkAuthenticator)authenticator).accountPassword = _password;
			
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
		public void TryDeleteAccount(string _name, string _password, bool hostMode=false)
		{
			
			((wovencode.NetworkAuthenticator)authenticator).accountName = _name;
			((wovencode.NetworkAuthenticator)authenticator).accountPassword = _password;
			
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
		public void TryStartServer()
		{
			if (!CanStartServer()) return;
			StartServer();
		}
	
		// -------------------------------------------------------------------------------
		public void TryCancel()
		{
			StopClient();
		}
	
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
		public void TryHostAndPlay(string _name, string _password)
		{
			//((wovencode.NetworkAuthenticator)authenticator).accountAction = "HOSTANDPLAY";
			//StartHost();
		}
	
		// -------------------------------------------------------------------------------
	
	}

}

// =======================================================================================