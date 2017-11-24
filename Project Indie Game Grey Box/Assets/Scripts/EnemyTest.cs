using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyTypes {CHICKEN, CHARGER, FLYING, RANGED, HASDEADPARENTS, LEAPER, DEFAULT}

public class EnemyTest : MonoBehaviour {

    public GameObject item;
    public GameObject player;
    public float jumpDistance = 1.0f;
    public float jumpHeight = 1.0f;

    private GameObject p;

	private void Start () {
        p = Instantiate(item, this.transform);
        p.transform.position = new Vector3(this.gameObject.transform.position.x - 10, this.gameObject.transform.position.y, 0);
        p.GetComponent<Rigidbody2D>().gravityScale = 0;
	}

    private void jumpAttack(Vector3 pTarget, float pXVelocity = 1.0f, float pYVelocity = 1.0f)
    {
        Vector3 normalized = new Vector3(new Vector3(pTarget.x - gameObject.transform.position.x, 0, 0).normalized.x, 1).normalized;
        Debug.Log(normalized);
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(normalized.x * pXVelocity, normalized.y * pYVelocity));

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
    public void flingItem(GameObject pObject, Vector2 pTarget, float pVelocity, bool arch = false)
    {
        if (arch)
        {
            GameObject projectile = Instantiate(pObject);
            projectile.transform.position = this.gameObject.transform.position;
            projectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(this.gameObject.transform.position.x + pTarget.x, (this.gameObject.transform.position.x - pTarget.x) - (this.gameObject.transform.position.y - pTarget.y)).normalized * 10.0f, ForceMode2D.Impulse);
        }
        else
        {
            GameObject projectile = Instantiate(pObject);
            projectile.transform.position = this.gameObject.transform.position;
            projectile.GetComponent<Rigidbody2D>().AddForce((pTarget).normalized * pVelocity,ForceMode2D.Impulse);
        }
    }

	private void Update () {
        if (Input.GetKeyDown(KeyCode.O)) flingItem(item, new Vector2(-9, 0), 5.0f);
        if (Input.GetKeyDown(KeyCode.P)) flingItem(item, p.transform.position, 5.0f, true);
        if (Input.GetKeyDown(KeyCode.J)) jumpAttack(player.transform.position, jumpDistance, jumpHeight);


        p.transform.position = new Vector3(this.gameObject.transform.position.x - 10, this.gameObject.transform.position.y, 0);
    }
}
