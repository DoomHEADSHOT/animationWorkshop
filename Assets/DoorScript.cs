using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour
{
    [SerializeField] string levelName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //SceneManager.LoadSceneAsync("Level2");

            NetworkManager.Singleton.SceneManager.LoadScene(levelName, LoadSceneMode.Single);
        }
    }
}
