using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface 사용 이유 
// Player, Box, Monster 전부 피격이 가능
public interface IDamageable
{
    void TakeDamage(int amount);
}
