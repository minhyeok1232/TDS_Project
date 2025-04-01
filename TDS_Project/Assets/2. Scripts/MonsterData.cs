using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Monster/MonsterData")]
public class MonsterData : ScriptableObject
{
    [SerializeField]
    private float damage;

    public float Damage // ���Ͱ� �ִ� �������� ���������� �����ϸ�, ��� ������ ���������� �����ϴ�.
    {
        get
        {
            return damage;
        }
    }

    [SerializeField]
    private float moveSpeed;

    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
    }
}