using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{

    public void FireEffect()
    {
        
        
    }
    public IEnumerator Cooltime(float cooldown)
    {
        
        yield return new WaitForSeconds(cooldown);
        // ��ٿ� ���� �� ����
    }

}
