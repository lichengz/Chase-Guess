using System.Collections.Generic;
using System; 
using UnityEngine;
using Fusion;

namespace FusionExamples.Tanknarok
{
	public class PlayerManager : MonoBehaviour
	{		
		private static List<Player> _allPlayers = new List<Player>();
		public static List<Player> allPlayers => _allPlayers;
        private static List<Player> _playersAlive = new List<Player>();
        public static List<Player> playersAlive => _playersAlive;
		private static Queue<Player> _playerQueue = new Queue<Player>();

		// static private CameraStrategy _cameraStrategy;
		// static private CameraStrategy CameraStrategy
		// {
		// 	get
		// 	{
		// 		if (_cameraStrategy == null)
		// 			_cameraStrategy = FindObjectOfType<CameraStrategy>(true);
		// 		return _cameraStrategy;
		// 	}
		// }

		public static void HandleNewPlayers()
		{
			if (_playerQueue.Count > 0)
			{
				Player player = _playerQueue.Dequeue();

				// CameraStrategy.AddTarget(player.gameObject);
				
				player.Respawn(0);
			}
		}

		public static List<Player> GetPlayersAlive()
		{
            _playersAlive.Clear();
			for (int i = 0; i < _allPlayers.Count; i++)
			{
				if (_allPlayers[i].isActivated || _allPlayers[i].lives > 0)
                {
                    _playersAlive.Add(_allPlayers[i]);
                }
			}
			return _playersAlive;
		}

		public static Player GetFirstAlivePlayer()
		{
			for (int i = 0; i < _allPlayers.Count; i++)
			{
				if (_allPlayers[i].isActivated)
					return _allPlayers[i];
			}

			return null;
		}

		public static void AddPlayer(Player player)
		{
			Debug.Log("Player Added");

			int insertIndex = _allPlayers.Count;
			// Sort the player list when adding players
			for (int i = 0; i < _allPlayers.Count; i++)
			{
				if (_allPlayers[i].playerID > player.playerID)
				{
					insertIndex = i;
					break;
				}
			}

			_allPlayers.Insert(insertIndex, player);
			_playerQueue.Enqueue(player);
		}

		public static void RemovePlayer(Player player)
		{
			if (player==null || !_allPlayers.Contains(player))
				return;

			Debug.Log("Player Removed " + player.playerID);

			_allPlayers.Remove(player);
			// if(CameraStrategy) // FindObject May return null on shutdown, so let's avoid that NPE
			// 	CameraStrategy.RemoveTarget(player.gameObject);
		}

		public static void ResetPlayerManager()
		{
			Debug.Log("Clearing Player Manager");
			allPlayers.Clear();
			// if(CameraStrategy) // FindObject May return null on shutdown, so let's avoid that NPE
			// 	CameraStrategy.RemoveAll();
			Player.local = null;
		}

		public static Player GetPlayerFromID(int id)
		{
			foreach (Player player in _allPlayers)
			{
				if (player.playerID == id)
					return player;
			}

			return null;
		}

		public static Player Get(PlayerRef playerRef)
		{
			for (int i = _allPlayers.Count - 1; i >= 0; i--)
			{
				if (_allPlayers[i] == null || _allPlayers[i].Object == null)
				{
					_allPlayers.RemoveAt(i);
					Debug.Log("Removing null player");
				}
				else if (_allPlayers[i].Object.InputAuthority == playerRef)
					return _allPlayers[i];
			}

			return null;
		}

        public static void AssignBattleIDs()
        {
            var rand = new System.Random();
            var playersAlive = GetPlayersAlive();
            int[] randomBattleIDs = new int[playersAlive.Count];

            for (int i = 0; i < playersAlive.Count; i++)
            {
                // random numbers between 1 and 3 (rock, paper, scissors. 0 is null)
                int randomBattleID = rand.Next(1, 4);

                // first 3 numbers generated must be unique 
                if (i > 0 && i < 3) 
                {
                    // generate again if randomBattleIDs already contains the number
                    while (Array.IndexOf(randomBattleIDs, randomBattleID) > -1)
                    {
                        randomBattleID = rand.Next(1, 4);
                    }
                }

                randomBattleIDs.SetValue(randomBattleID, i);
                playersAlive[i].battleID = (Player.BattleID)randomBattleIDs[i];

                Debug.Log($"assigned battle id {(Player.BattleID)randomBattleIDs[i]} to player {playersAlive[i].playerID}");
            }
        }
	}
}