using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "Scriptable Objects/Bullet")]
public class Bullet : ScriptableObject
{
    public GameObject prefab;
    public Vector3 size = Vector3.one;
    public float speed = 1f;
    public float lifetime = 1f;

}
