using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    Rigidbody2D m_Rigidbody2D;
    bool m_FacingRight = true;
    bool facingRight = true;
    bool isHitted = false;

    [SerializeField]
    float speed = 2f;

    [SerializeField]
    float life = 10;
    [SerializeField]
    bool isInvincible = false;

    [SerializeField]
    GameObject enemy;
    float distToPlayer;
    float distToPlayerY;
    float meleeDist = 1f;
    float rangeDist = 5f;
    bool canAttack = true;
    Transform attackCheck;
    float dmgValue = 4;

    [SerializeField]
    GameObject throwableObject;

    float randomDecision = 0;
    bool doOnceDecision = true;
    bool endDecision = false;
    Animator anim;

    [SerializeField] Transform checkSensor;
    [SerializeField] float lenghtCheckWall;
    [SerializeField] float lenghtCheckGround;
    bool isPlat;
    bool isObstacle;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
       // attackCheck = transform.Find("AttackCheck").transform;
        anim = transform.Find("Model").GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        //this.MoveToTarget(enemy.transform);
       // this.FlipWhenChangeVelX();
        this.moveWhenNotTarget();
    }

    void MoveToTarget(Transform target)
    {
        if (isHitted) return;

        distToPlayer = target.position.x - transform.position.x;
        distToPlayerY = target.position.y - transform.position.y;

        if (Mathf.Abs(distToPlayer) < .25f)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
            AnimationUltils.Instance.ChangeAnimation(anim, "Mushroom_Idle");
        }
        else if (Mathf.Abs(distToPlayer) > .25f && Mathf.Abs(distToPlayer) < meleeDist)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0f, m_Rigidbody2D.velocity.y);
            AnimationUltils.Instance.ChangeAnimation(anim, "Mushroom_Idle");

            // nguoi choi vao vung phat hien se quay mat lai
            if (((distToPlayer > 0f && transform.localScale.x < 0f) || (distToPlayer < 0f && transform.localScale.x > 0f)) && Mathf.Abs(distToPlayerY) < 2f)
                Flip();
        }
        else if (Mathf.Abs(distToPlayer) > meleeDist && Mathf.Abs(distToPlayer) < rangeDist)
        {
            // di den vi tri nguoi choi de tan cong
            m_Rigidbody2D.velocity = new Vector2(distToPlayer / Mathf.Abs(distToPlayer) * speed, m_Rigidbody2D.velocity.y);
            AnimationUltils.Instance.ChangeAnimation(anim, "Mushroom_Run");
        }
        else
        {
            AnimationUltils.Instance.ChangeAnimation(anim, "Mushroom_Idle");
            if (!endDecision)
            {
                if ((distToPlayer > 0f && transform.localScale.x < 0f) || (distToPlayer < 0f && transform.localScale.x > 0f))

                    Flip();

                //if (randomDecision < 0.4f)
                //    Run();
                //else if (randomDecision >= 0.4f && randomDecision < 0.6f)
                //    Jump();
                //else if (randomDecision >= 0.6f && randomDecision < 0.8f)
                //    StartCoroutine(Dash());
                //else if (randomDecision >= 0.8f && randomDecision < 0.95f)
                //    RangeAttack();
                //else
                //    Idle();
            }
            else
            {
                endDecision = false;
            }

        }
    }

    void moveWhenNotTarget()
    {
        Vector3 targetPosX = checkSensor.position;
        Vector3 targetPosY = checkSensor.position;
        if (!facingRight && lenghtCheckGround > 0) lenghtCheckWall *= -1;
        else if(facingRight && lenghtCheckWall < 0) lenghtCheckWall *= -1;
        targetPosX.x += lenghtCheckWall;
        targetPosY.y += -lenghtCheckGround;

        Debug.DrawLine(checkSensor.position, targetPosX, Color.blue);
        isObstacle = Physics2D.Linecast(checkSensor.position, targetPosX, 1 << LayerMask.NameToLayer("Ground"));

         
        Debug.DrawLine(checkSensor.position, targetPosY, Color.blue);
        isPlat = Physics2D.Linecast(checkSensor.position, targetPosY, 1 << LayerMask.NameToLayer("Ground"));

        if (!isHitted && life > 0 && Mathf.Abs(m_Rigidbody2D.velocity.y) < 0.5f)
        {
            if (isPlat && !isObstacle && !isHitted)
            {
                if (facingRight)
                {
                    m_Rigidbody2D.velocity = new Vector2(speed, m_Rigidbody2D.velocity.y);
                }
                else
                {
                    m_Rigidbody2D.velocity = new Vector2(-speed, m_Rigidbody2D.velocity.y);
                }
            }
            else
            {
                Flip();
            }
        }
    }

    void FlipWhenChangeVelX()
    {
        if (transform.localScale.x * m_Rigidbody2D.velocity.x > 0 && !m_FacingRight && life > 0)
        {
            // ... flip the player.

            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (transform.localScale.x * m_Rigidbody2D.velocity.x < 0 && m_FacingRight && life > 0)
        {
            // ... flip the player.
            Flip();
        }
    }

    void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void ApplyDamage(float damage)
    {
        if (isInvincible) return;

        float direcition = damage / Mathf.Abs(damage);
        damage = Mathf.Abs(damage);
        AnimationUltils.Instance.ChangeAnimation(anim, "Mushroom_Hit");
        life -= damage;
        transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(direcition * 300f, 100f));
        StartCoroutine(HitTime());
    }

    IEnumerator HitTime()
    {
        isInvincible = true;
        isHitted = true;
        yield return new WaitForSeconds(0.1f);
        isHitted = false;
        isInvincible = false;
    }

    public void MeleeAttack()
    {
        AnimationUltils.Instance.ChangeAnimation(anim, "Mushroom_Attack");
        Collider2D[] collider2DsEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 0.9f);
        for (int i = 0; i < collider2DsEnemies.Length; i++)
        {
            if (collider2DsEnemies[i].gameObject.tag == "Player")
            {
            }
        }
    }
}
