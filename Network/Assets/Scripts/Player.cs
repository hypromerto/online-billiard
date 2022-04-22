using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public ushort Id { get; private set; }
    public string Username { get; private set; }

    private void OnDestroy()
    {
        list.Remove(Id);
    }

    public static void Spawn(ushort id, string username)
    {
        foreach (Player otherPlayer in list.Values)
            otherPlayer.SendSpawned(id);

        Player player = Instantiate(GameLogic.Singleton.PlayerPrefab, new Vector3(0f, 2f, 0f), Quaternion.identity)
            .GetComponent<Player>();
        player.name = $"Player {id} ({(string.IsNullOrEmpty(username) ? "Guest" : username)})";
        player.Id = id;
        player.Username = string.IsNullOrEmpty(username) ? $"Guest {id}" : username;

        player.SendSpawned();
        list.Add(id, player);
    }

    #region Messages

    private void SendSpawned()
    {
        Message message = Message.Create(MessageSendMode.reliable, (ushort) ServerToClientId.playerSpawned);

        message = AddSpawnData(message);

        NetworkManager.Singleton.Server.SendToAll(message);
    }

    private void SendSpawned(ushort fromClientId)
    {
        Message message = Message.Create(MessageSendMode.reliable, ServerToClientId.playerSpawned);

        message = AddSpawnData(message);

        NetworkManager.Singleton.Server.Send(message, fromClientId);
    }

    private Message AddSpawnData(Message message)
    {
        message.AddUShort(Id);
        message.AddString(Username);
        message.AddVector3(transform.position);

        return message;
    }

    #endregion

    [MessageHandler((ushort) ClientToServerId.name)]
    private static void Name(ushort fromClientId, Message message)
    {
        Spawn(fromClientId, message.GetString());
    }
}