using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayController : MonoBehaviour
{
    public float moveSpeed;
   Rigidbody2D playerRB;
    Vector2 moveVect;
    Animator playAnimator;
    public Vector2 lockDirection;
    public int atk;
    private bool isAtkCD;
    private bool isEnterBattle;
    public float atkSpeed;
    private float atkCDTimer;
    private GameObject battleOppo;
    public GameObject atkCDImage;
    

    // Start is called before the first frame update
    void Start()
    {
        moveVect = new Vector2();
        playerRB = GetComponent<Rigidbody2D>();
        playAnimator = GetComponent<Animator>();
        lockDirection = new Vector2 { x = 0, y = -1 };
        atkCDTimer = 0;
        isAtkCD = false;
        isEnterBattle = false;
        battleOppo = null;
        atkCDImage.transform.parent.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        moveVect.x = Input.GetAxisRaw("Horizontal");
        moveVect.y = Input.GetAxisRaw("Vertical");
        if (moveVect.x!=0||moveVect.y!=0)
        {
            lockDirection = moveVect;
        }

        #region 战斗处理相关
        if (isEnterBattle)
        {
            if (atkCDTimer<=0)
            {
                //playAnimator.SetTrigger("Attack");
                EnterBattle();
            }
            else
            {
                atkCDTimer -= Time.deltaTime;
                UpdataPlayerAtkCDUi(atkCDTimer);
            }
        }
        #endregion

        #region 获取敌人对象并进入战斗
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit2D playerHit = Physics2D.Raycast(playerRB.position, lockDirection, 0.5f, LayerMask.GetMask("Enemy"));
            if (playerHit.collider != null)
            {
                battleOppo = playerHit.transform.gameObject;
                isEnterBattle = true;
                atkCDTimer = 0;
            }
        }
        #endregion
    }

    void FixedUpdate()
    {
        if (isEnterBattle)
        {
            playerRB.MovePosition(playerRB.position);
            playAnimator.SetFloat("LockX", lockDirection.x);
            playAnimator.SetFloat("LockY", lockDirection.y);
        }
        else
        {
            playerRB.MovePosition(playerRB.position + moveVect * moveSpeed * Time.fixedDeltaTime);
            //playAnimator.SetFloat("Speed", moveVect.magnitude);
            playAnimator.SetFloat("LockX", lockDirection.x);
            playAnimator.SetFloat("LockY", lockDirection.y);
        }
    }

    /// <summary>
    /// 战斗相关函数
    /// </summary>
    /// <param name="enemy"></param>
    void EnterBattle()
    {
        atkCDImage.transform.parent.gameObject.SetActive(false);
        battleOppo.GetComponent<EnemyUIManager>().UpdataEnemyHpBar(atk);
        atkCDTimer = atkSpeed;
        atkCDImage.transform.parent.gameObject.SetActive(true);
        Debug.Log($"攻击敌人，造成{atk}点伤害");
        if (battleOppo.GetComponent<EnemyUIManager>().EnemyCurrentHP <= 0)
        {
            GameObject.DestroyImmediate(battleOppo);
            isEnterBattle = false;
            atkCDTimer = atkSpeed;
            atkCDImage.transform.parent.gameObject.SetActive(false);
        }
    }

    void UpdataPlayerAtkCDUi(float timer)
    {
        atkCDImage.GetComponent<Image>().fillAmount = timer / atkSpeed;
        Debug.Log(atkCDImage.GetComponent<Image>().fillAmount.ToString());
    }
}
