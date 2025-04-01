using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Monster/MonsterData")]
public class MonsterData : ScriptableObject
{
    [SerializeField]
    private int damage;

    public int Damage // ���Ͱ� �ִ� �������� ���������� �����ϸ�, ��� ������ ���������� �����ϴ�.
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