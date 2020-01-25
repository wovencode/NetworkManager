// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using Wovencode;
using Wovencode.Database;
using Wovencode.Network;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Wovencode.Database
{

	// ===================================================================================
	// DatabaseManager
	// ===================================================================================
	public partial class DatabaseManager
	{

		// -------------------------------------------------------------------------------
		// GetPlayers
		// -------------------------------------------------------------------------------
		public List<PlayerPreview> GetPlayers(string username)
		{
			List<TablePlayer> results = Query<TablePlayer>("SELECT * FROM "+nameof(TablePlayer)+" WHERE username=? AND deleted=0 AND banned=0", username);
			
			List<PlayerPreview> players = new List<PlayerPreview>();
			
			foreach (TablePlayer result in results)
				players.Add(new PlayerPreview { name = result.name} );
			
			return players;
		}

		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================