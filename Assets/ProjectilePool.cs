using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePool : MonoBehaviour
{
    public GameObject projectilePrefab;
    public int maxPoolSize = 20;

    private IObjectPool<GameObject> projectilePool;

    void Awake()
    {
        projectilePool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, false, 10, maxPoolSize);
    }

    private GameObject CreatePooledItem()
    {
        GameObject projectile = Instantiate(projectilePrefab);
        projectile.SetActive(false);
        return projectile;
    }

    private void OnTakeFromPool(GameObject projectile)
    {
        projectile.SetActive(true);
    }

    private void OnReturnedToPool(GameObject projectile)
    {
        projectile.SetActive(false);
    }

    private void OnDestroyPoolObject(GameObject projectile)
    {
        Destroy(projectile);
    }

    public GameObject GetProjectile()
    {
        return projectilePool.Get();
    }

    public void ReturnProjectile(GameObject projectile)
    {
        projectilePool.Release(projectile);
    }
}