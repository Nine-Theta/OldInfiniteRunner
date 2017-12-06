using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberScript : MonoBehaviour
{
    [SerializeField]
    private GameObject thrownItem;

    private Transform player;

    [SerializeField]
    private Vector2 followOffset = new Vector2(0, 0);

    [SerializeField]
    private float thinkSpeed = 3.0f;
    [SerializeField]
    private float detectionRange = 5.0f;
    [SerializeField]
    private float followSpeedMult = 1.0f;
    [SerializeField]
    private float throwSpeedMult = 1.0f;
    [SerializeField]
    private float throwHeightMult = 1.0f;
    [SerializeField]
    private float gravityScale = 1.0f;

    [SerializeField]
    private bool throwArc = true;
    [SerializeField]
    private float _idleDelay = 5.0f;
    [SerializeField]
    private AudioClip _idleSound;
    [SerializeField]
    private AudioClip _attackSound;
    [SerializeField]
    private AudioClip _damageSound;
    [SerializeField]
    private AudioClip _deathSound;

    private float _idleTimer = 5.0f;

    private enum EnemyStates { PATROL, CHASE }
    private EnemyStates behaviourState = EnemyStates.PATROL;

    private AudioSource _audio;
    private Rigidbody2D _body;
    private Rigidbody2D _playerBody;
    private Vector2 _offset;
    private HealthBarScript _healthBar;
    private Animator _animator;

    private PathfinderScript _pathFinder = null;
    private Stack<Node> _path;
    private Node _currentNode;
    private Vector3 _lastKnownPlayerPos = Vector3.zero;
    private Vector2 _mapOffset;
    private Color _initialColor;

    private float _thinkTimer = 3.0f;
    private bool _usePathfinding = false;

    private void Start()
    {
        player = MovementScript.GetPlayer().transform;

        if (this.gameObject.GetComponent<PathfinderScript>() != null)
        {
            _usePathfinding = true;
            _pathFinder = gameObject.GetComponent<PathfinderScript>();
        }

        //_mapOffset = GraphGenerator.GeneratorObject().gameObject.GetComponent<GraphGenerator>().GetMapOffset();
        //Debug.Log("mapoffset: " + _mapOffset);

        _thinkTimer = Random.Range(0.0f, thinkSpeed);
        _body = this.gameObject.GetComponent<Rigidbody2D>();
        _playerBody = player.gameObject.GetComponent<Rigidbody2D>();
        _offset = new Vector2(followOffset.x, followOffset.y);
        _healthBar = GetComponentInChildren<HealthBarScript>();
        _animator = GetComponent<Animator>();
        _initialColor = GetComponent<SpriteRenderer>().color;
        _audio = GetComponent<AudioSource>();
        GetComponent<SpriteRenderer>().color = Color.black;
        _idleTimer = Random.Range(0.0f, _idleDelay);
    }

    /// <summary> Creates a copy of an Object and flings it at the target. </summary>
    /// <param name="pObject">The original GameObject to be copied. Requires a Rigidbody2D.</param>
    /// <param name="pTarget">The position of the target.</param>
    /// <param name="arc">If the object should be thrown in an arc or not. False by default.</param>
    public void flingItem(Vector3 pTarget, float pXVelocity = 1.0f, float pYVelocity = 1.0f, bool arc = false, float pGravityScale = 1.0f)
    {
        GameObject projectile = Instantiate(thrownItem);
        projectile.transform.position = gameObject.transform.position;
        Rigidbody2D projBody = projectile.GetComponent<Rigidbody2D>();

        if (arc)
        {
            projectile.GetComponent<Rigidbody2D>().gravityScale = pGravityScale;

            float x = Mathf.Abs(pTarget.x - gameObject.transform.position.x);
            float y = Mathf.Abs(pTarget.y - gameObject.transform.position.y);

            Vector2 normalized = new Vector2(pTarget.x - gameObject.transform.position.x, y).normalized;

            projBody.AddForce(new Vector2(normalized.x * x * pXVelocity, normalized.y * (y + x) * pYVelocity), ForceMode2D.Impulse);
        }
        else
        {
            projBody.gravityScale = 0.0f;
            projBody.AddForce((pTarget - gameObject.transform.position).normalized * pXVelocity, ForceMode2D.Impulse);
        }
    }

    private void UpdatePosition()
    {
        #region pathfinding
        if (_usePathfinding)
        {
            //if (_path.Count == 0) _path = null;

            if (_path == null || _path.Count == 0)
            {
                _currentNode = null;
                if (_pathFinder.CanFindPath(gameObject.transform.position, player.position, 15))
                {
                    _path = new Stack<Node>();
                    if (_pathFinder.GetPath() != null)
                    {
                        foreach (Node node in _pathFinder.GetPath())
                            _path.Push(node);

                        _currentNode = _path.Pop();
                    }
                }
            }

            if (_currentNode != null)
            {
                Vector2 position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
                if (position.x < _currentNode.Position.x + _mapOffset.x + 0.5f && position.x > _currentNode.Position.x + _mapOffset.x - 0.5f && position.y < _currentNode.Position.y + _mapOffset.y + 0.5f && position.y > _currentNode.Position.y + _mapOffset.y - 0.5f)
                    _currentNode = _path.Pop();

                //Debug.Log("currentNode pos: " + _currentNode.Position);
                gameObject.transform.position += new Vector3(_currentNode.Position.x - position.x + _mapOffset.x, _currentNode.Position.y - position.y + _mapOffset.y).normalized * 0.1f;
                return;
            }
        }
        #endregion
        //Debug.Log("Not using path finding");
        _idleTimer -= Time.deltaTime;
        if (_idleTimer <= 0.0f)
        {
            _idleTimer = _idleDelay;
            _audio.PlayOneShot(_idleSound);
        }
        Vector2 subtracted = new Vector2(player.position.x - gameObject.transform.position.x + _offset.x, player.position.y - gameObject.transform.position.y + _offset.y);

        if (Mathf.Abs(player.position.x - gameObject.transform.position.x) < _offset.x)
            subtracted.x = 0;

        _body.AddForce(new Vector2(subtracted.normalized.x * followSpeedMult, 0), ForceMode2D.Force);
        _body.velocity = new Vector2(_body.velocity.x, subtracted.normalized.y * subtracted.magnitude);
    }

    private void Step()
    {
        if (throwArc)
            flingItem(new Vector2(player.position.x + _playerBody.velocity.x, player.position.y), throwSpeedMult, throwHeightMult, true, gravityScale);
        else
            flingItem(new Vector2(player.position.x + _playerBody.velocity.x, player.position.y), throwSpeedMult);
        _audio.PlayOneShot(_attackSound);
        _thinkTimer = thinkSpeed;
    }

    private void Update()
    {
        if (_path != null)
            Debug.Log(_path.Count);

        if ((player.position - gameObject.transform.position).magnitude < detectionRange)
        {
            if (GetComponent<SpriteRenderer>().color == Color.black)
                GetComponent<SpriteRenderer>().color = _initialColor;
            UpdatePosition();

            if (_thinkTimer <= 0) Step();
            else _thinkTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Explosion")
        {
            _audio.PlayOneShot(_damageSound);
            if (!_healthBar.TakeDamage())
            {
                _animator.SetBool("Dead", true);
                if (_audio != null)
                    _audio.PlayOneShot(_deathSound);
            }
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void SetAnimFalse()
    {
        _animator.SetBool("Dead", false);
    }
}
