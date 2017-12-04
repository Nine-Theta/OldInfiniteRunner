using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyTypes {CHICKEN, CHARGER, FLYING, SPINNER, RANGED, HASDEADPARENTS, LEAPER, DEFAULT}

public class EnemyTest : MonoBehaviour {

    public GameObject item;
    public Transform player;

    public EnemyTypes enemyType = EnemyTypes.LEAPER;

    public float thinkSpeed = 3.0f;
    public float detectionRange = 5.0f;
    
    public float itemThrowHeight = 1.0f;

    public float jumpDistance = 1.0f;
    public float jumpHeight = 1.0f;

    private GameObject p;
    private float _thinkTimer = 3.0f;

	private void Start () {
        p = Instantiate(item, this.transform);
        p.transform.position = new Vector3(this.gameObject.transform.position.x - 10, this.gameObject.transform.position.y, 0);
        p.GetComponent<Rigidbody2D>().gravityScale = 0;
	}
    

    private void jumpAttack(Vector3 pTarget, float pXVelocity = 1.0f, float pYVelocity = 1.0f)
    {
        Vector3 normalized = new Vector3(pTarget.x - gameObject.transform.position.x, pTarget.y - gameObject.transform.position.y, 0).normalized;
        Debug.Log(normalized);
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(normalized.x * pXVelocity, normalized.y + 1.0f * pYVelocity));

        //Vector3 t = (new Vector3(pTarget.x - gameObject.transform.position.x, Mathf.Abs(pTarget.x - gameObject.transform.position.x) + (pTarget.y - this.gameObject.transform.position.y), 0));
        //Debug.Log(t);
        //this.gameObject.GetComponent<Rigidbody2D>().AddForce((t).normalized * ((new Vector3(pTarget.x, pTarget.y, 0) - gameObject.transform.position).magnitude*0.2f + 2.0f));
    }

    /// <summary>
    /// Creates a copy of an Object and flings it at the target.
    /// </summary>
    /// <param name="pObject">The original GameObject to be copied. Requires a Rigidbody2D.</param>
    /// <param name="pTarget">The position of the target.</param>
    /// <param name="pVelocity">The velocity given to the Object.</param>
    /// <param name="arch">If the object should be thrown in an arch or not False by default.</param>
    public void flingItem(GameObject pObject, Vector3 pTarget, float pXVelocity = 1.0f, float pYVelocity = 1.0f, bool arch = false, float pGravityScale = 1.0f)
    {
        if (arch)
        {
            GameObject projectile = Instantiate(pObject);
            projectile.transform.position = this.gameObject.transform.position;
            projectile.GetComponent<Rigidbody2D>().gravityScale = pGravityScale;

            Vector3 normalized = new Vector3(new Vector3(pTarget.x - gameObject.transform.position.x, 0, 0).normalized.x, 1).normalized;
            Debug.Log(normalized);
            projectile.GetComponent<Rigidbody2D>().AddForce(new Vector3(normalized.x * (pXVelocity), normalized.y * (pYVelocity)), ForceMode2D.Impulse);

            //projectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(this.gameObject.transform.position.x + pTarget.x, (this.gameObject.transform.position.x - pTarget.x) - (this.gameObject.transform.position.y - pTarget.y)).normalized * 10.0f, ForceMode2D.Impulse);
        }
        else
        {
            GameObject projectile = Instantiate(pObject);
            projectile.transform.position = this.gameObject.transform.position;
            projectile.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            projectile.GetComponent<Rigidbody2D>().AddForce((pTarget - gameObject.transform.position).normalized * pXVelocity,ForceMode2D.Impulse);
        }
    }

    private void Step()
    {
        if ((player.position - gameObject.transform.position).magnitude < detectionRange)
        {
            jumpAttack(player.position, jumpDistance, jumpHeight);
        }


        _thinkTimer = thinkSpeed;
    }

	private void Update () {
        if (Input.GetKeyDown(KeyCode.O)) flingItem(item, player.position, 5.0f);
        if (Input.GetKeyDown(KeyCode.P)) flingItem(item, player.position, 5.0f, 5.0f, true);
        if (Input.GetKeyDown(KeyCode.J)) jumpAttack(player.position, jumpDistance, jumpHeight);

        switch (enemyType) {
            //case

            case EnemyTypes.LEAPER:
                if (_thinkTimer <= 0) Step();
                else _thinkTimer -= Time.deltaTime;
                break;
        }

        p.transform.position = new Vector3(this.gameObject.transform.position.x - 10, this.gameObject.transform.position.y, 0);
    }
}
