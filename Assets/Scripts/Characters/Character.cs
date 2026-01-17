using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected int health = 100;
    protected float moveSpeed = 5f;

    public int Health
    {
        get { return health; }
    }

    public abstract void Move();
    public abstract void TakeDamage(int damage);
}
