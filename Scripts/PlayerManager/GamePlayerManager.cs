using UnityEngine;
using InControl;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GamePlayerManager : MonoBehaviour
{
	private static GamePlayerManager instance;

	public PlayerController _playerPrefabP1;
	public PlayerController _playerPrefabP2;

	public Vector3 _P1Start;
	public Vector3 _P2Start;

	public int SetControllertoIcebert = 0;
	public int SetControllertoSpicegirl = 1;

	private CheckpointManager _CM;
		
	const int maxPlayers = 2;

	List<PlayerController> players = new List<PlayerController>( maxPlayers );

    private void Start()
	{
		_playerPrefabP1 = Instantiate(_playerPrefabP1, _P1Start, Quaternion.identity);
		_playerPrefabP2 = Instantiate(_playerPrefabP2, _P2Start, Quaternion.identity);
		InputManager.OnDeviceDetached += OnDeviceDetached;
        _CM = GameObject.FindGameObjectWithTag("CM").GetComponent<CheckpointManager>();
		_P1Start = _CM._LastCheckpointPosP1;
		_P2Start = _CM._LastCheckpointPosP2;
	}

	void Update()
	{
		var inputDevice = InputManager.ActiveDevice;

		if (JoinButtonWasPressedOnDevice( inputDevice ))
		{
			if (ThereIsNoPlayerUsingDevice( inputDevice ))
			{
				CreatePlayer( inputDevice );
			}
		}
	}

	bool JoinButtonWasPressedOnDevice( InputDevice inputDevice )
	{
		return inputDevice.IsActive;
	}

	PlayerController FindPlayerUsingDevice( InputDevice inputDevice )
	{
		var playerCount = players.Count;
		for (var i = 0; i < playerCount; i++)
		{
			var player = players[i];
			if (player.Device == inputDevice)
			{
				return player;
			}
		}
		return null;
	}

	bool ThereIsNoPlayerUsingDevice( InputDevice inputDevice )
	{
		return FindPlayerUsingDevice( inputDevice ) == null;
	}


	void OnDeviceDetached( InputDevice inputDevice )
	{
		var player = FindPlayerUsingDevice( inputDevice );
		if (player != null)
		{
			RemovePlayer( player );
		}
	}

	PlayerController CreatePlayer( InputDevice inputDevice )
	{
		// if (players.Count < maxPlayers)
		// {
		// 	// Pop a position off the list. We'll add it back if the player is removed.

        //     var player = Instantiate( playerPrefab, new Vector3(0f, 10f, 0f), Quaternion.identity );
        //     player.Device = inputDevice;
        //     players.Add( player );
        //     return player;
            
		// }

        if(players.Count == 0)
        {
			_playerPrefabP1.Device = InputManager.Devices[SetControllertoIcebert];
			players.Add(_playerPrefabP1);
			return _playerPrefabP1;
        }
        if(players.Count == 1)
        {
			_playerPrefabP2.Device = InputManager.Devices[SetControllertoSpicegirl];
			players.Add(_playerPrefabP2);
			return _playerPrefabP2;
        }
		return null;

	}

	void RemovePlayer( PlayerController player )
	{
		players.Remove( player );
		player.Device = null;
	}
}