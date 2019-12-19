using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIManager : MonoBehaviour
{
    public GameObject enemyHpBar;
    public int enemyHP;
    public int EnemyCurrentHP { get; set; }
    //public static EnemyUIManager Instance { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        EnemyCurrentHP = enemyHP;
        enemyHpBar.transform.parent.gameObject.SetActive(false);
        enemyHpBar.GetComponent<Image>().fillAmount = (float)EnemyCurrentHP / (float)enemyHP;
        //Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdataEnemyHpBar(int damage)
    {
        //if (enemyHpBar.activeSelf == true)
        //{
        //    enemyHpBar.SetActive(false);
        //    return;
        //}

        //enemyHpBar.SetActive(true);
        if (enemyHpBar.transform.parent.gameObject.activeSelf!=true)
        {
            enemyHpBar.transform.parent.gameObject.SetActive(true);
        }
        EnemyCurrentHP -= damage;
        enemyHpBar.GetComponent<Image>().fillAmount = (float)EnemyCurrentHP / (float)enemyHP;
    }
}
