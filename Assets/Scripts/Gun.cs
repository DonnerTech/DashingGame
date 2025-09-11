using System;
using UnityEngine;
using UnityEngine.Pool;

public class Gun : MonoBehaviour
{
    [SerializeField] private BulletData bulletData;
    [SerializeField] private int maxPoolSize;
    [SerializeField] private int cooldownTime = 10;
    private int cooldown = 0;
    private ObjectPool<GameObject> bullets;

    public GameObject gunParentColliderRoot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bullets = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, maxPoolSize, maxPoolSize);
    }

    void FixedUpdate()
    {
        cooldown--;
    }

    public void Fire()
    {

        if (bullets.CountActive != maxPoolSize && cooldown <= 0)
        {
            bullets.Get();
            cooldown = cooldownTime;
        }
    }

    private GameObject CreatePooledItem()
    {
        GameObject go = Instantiate(bulletData.prefab);
        Bullet bullet = go.GetComponent<Bullet>();
        bullet.parentObject = gunParentColliderRoot;
        bullet.parentPool = bullets;
        bullet.bulletData = bulletData;

        go.SetActive(false);
        return go;
    }

    private void OnTakeFromPool(GameObject bulletObject)
    {
        bulletObject.SetActive(true);

        Vector3 pos;
        Quaternion rot;
        transform.GetPositionAndRotation(out pos, out rot);
        bulletObject.transform.SetPositionAndRotation(pos, rot);
    }

        private void OnReturnedToPool(GameObject bulletObject)
    {
        bulletObject.SetActive(false);
    }

        private void OnDestroyPoolObject(GameObject bulletObject)
    {
        Destroy(bulletObject);
    }
}
