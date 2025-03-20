using Unity.Netcode;
using UnityEngine;

public class ServerUI : MonoBehaviour
{
 
    public void HostGameLocally()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void LeaveGameLocally()
    {
        NetworkManager.Singleton.Shutdown();
    }
}
