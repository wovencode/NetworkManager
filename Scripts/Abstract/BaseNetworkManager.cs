// =======================================================================================
// Wovencore
// by Weaver (Fhiz)
// MIT licensed
// =======================================================================================

using UnityEngine;
using System;
using Mirror;
using System.Collections.Generic;
using Wovencode;
using Wovencode.Network;
using Wovencode.Debug;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Wovencode.Network
{
	
	// ===================================================================================
	// BaseNetworkManager
	// ===================================================================================
	public abstract partial class BaseNetworkManager : Mirror.NetworkManager
	{

#if wSYNCABLE
		[Header("Spawnable Prefab Categories")]
		[Tooltip("Only prefabs of the listed categories are included")]
    	public string[] sortCategories;
#endif

		[Header("Debug Helper")]
		public DebugHelper debug;
		public NetworkType networkType;
		
#if UNITY_EDITOR
	
		// -------------------------------------------------------------------------------
		// AutoRegisterSpawnablePrefabs
		// @Editor
		// -------------------------------------------------------------------------------
		public void AutoRegisterSpawnablePrefabs()
		{

			var guids = AssetDatabase.FindAssets("t:Prefab");
			List<GameObject> toSelect = new List<GameObject>();
			spawnPrefabs.Clear();

			foreach (var guid in guids)
			{
				var path = AssetDatabase.GUIDToAssetPath(guid);
				UnityEngine.Object[] toCheck = AssetDatabase.LoadAllAssetsAtPath(path);
				
				foreach (UnityEngine.Object obj in toCheck)
				{
					var go = obj as GameObject;
					if (go == null)
					{
						continue;
					}

					NetworkIdentity comp = go.GetComponent<NetworkIdentity>();
					if (comp != null && !comp.serverOnly)
					{

#if wSYNCABLE
						EntityComponent entityComponent = go.GetComponent<EntityComponent>();

						if (entityComponent == null || sortCategories == null || sortCategories.Length == 0)
							toSelect.Add(go);
						else if (Tools.ArrayContains(sortCategories, entityComponent.archeType.sortCategory))
							toSelect.Add(go);
						
#else
						toSelect.Add(go);
#endif
					}
					
				}
			}

			spawnPrefabs.AddRange(toSelect.ToArray());
			
			debug.Log("[NetworkManager] Added [" + toSelect.Count + "] prefabs to spawnables prefabs list.");

		}
		
		// -------------------------------------------------------------------------------
		
#endif
			
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================