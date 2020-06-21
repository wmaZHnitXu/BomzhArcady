using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : hpBase
{
    private bool enemyMinused;
    private void OnEnable() {
        //characterctrl.it.allEnemys.Add(this);
        enemyMinused = false;
    }
    private void EnemyCountMinus () {
        if (!enemyMinused) timeCtrl.me.enemyCount--;
        enemyMinused = true;
        //characterctrl.it.allEnemys.Remove(this);
    }
}
