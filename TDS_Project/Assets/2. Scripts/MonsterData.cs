using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Monster/MonsterData")]
public class MonsterData : ScriptableObject
{
    [SerializeField]
    private int damage;

    public int Damage // 몬스터가 주는 데미지는 고정값으로 지정하며, 모든 몬스터의 데미지값은 동일하다.
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