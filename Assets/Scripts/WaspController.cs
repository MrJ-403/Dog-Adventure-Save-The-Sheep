using UnityEngine;
using UnityEngine.Assertions.Must;

struct Point
{
	public Vector2 position;
};

public class WaspController : MonoBehaviour
{
	private Point a, b;
	private Vector2 currentPos;
	private int invertDir = 0;
	public float pointThreshold = 0.1f;

	private Animator animator;
	private int direction; //Right,Left,Up,Down
	private Vector2 dir;
	private RaycastHit2D hit;
	public GameObject wasp;
	public GameObject shot;
	public float shotSpeed;
	public float speed;
	public float range;
	public int orientation ; //Horizontal, Vertical
	public float shotTimer;
	public float shotRate;
	public float distance;
	//public Transform parent;
	public LayerMask layerMask;

   void Start()
	{
		currentPos = wasp.transform.position;
		a.position.x = transform.position.x; a.position.y = transform.position.y;
		b.position.x = transform.position.x - (orientation == 0 ? distance : 0);
		b.position.y = transform.position.y - (orientation == 1 ? distance : 0);
		//Debug.Log(transform.name + "at position (" + transform.position.x + ", " + transform.position.y + "): \nStartPos: (" + a.position.x + ", " + a.position.y + ")\nEndPos: (" + b.position.x + ", " + b.position.y + ')');
		animator = GetComponentInChildren<Animator>();
		direction = distance >= 0 ? 1 : -1;
		//parent = gameObject.GetComponentInParent<Transform>();
		dir = (orientation == 0 ? Vector2.right : Vector2.up) * direction;
	}
	// Update is called once per frame
	void Update()
	{
		//wasp.GetComponent<SpriteRenderer>().flipX = dir.x < 0 || dir.y < 0;

		shotTimer += Time.deltaTime;
		if (shotTimer >= shotRate)
		{
			hit = Physics2D.Raycast(wasp.transform.position, wasp.transform.right, range, layerMask);
			if (hit.rigidbody != null)
			{
				if (hit.rigidbody.CompareTag("Player"))
				{
					animator.SetTrigger("isShooting");
					Shoot();
					shotTimer = 0;
				}
			}
		}
	}
    private void FixedUpdate()
    {
		if (invertDir == 0 && Vector2.Distance((Vector2)wasp.transform.position, b.position) < pointThreshold) 
		{ 
			invertDir = 1; 
			if(orientation == 0) wasp.transform.Rotate(0, 180, 0);
		}
		if (invertDir == 1 && Vector2.Distance((Vector2)wasp.transform.position, a.position) < pointThreshold)
		{
			invertDir = 0;
            if (orientation == 0) wasp.transform.Rotate(0, 180, 0);
        }
		currentPos = wasp.transform.position;
		currentPos = Vector2.MoveTowards(currentPos, invertDir == 0 ? b.position : a.position, speed * Time.deltaTime);
		wasp.transform.position = currentPos;
	}

	private void Shoot()
	{
		GetComponent<AudioSource>().Play();
		Transform sp = wasp.GetComponentInChildren<Transform>();
		var stinger = Instantiate(shot,sp.transform.position,sp.transform.rotation);
		stinger.GetComponent<Rigidbody2D>().velocity = (Mathf.Round(sp.transform.rotation.y)%360==0 ? 1 : -1) * shotSpeed * new Vector2(1,0);
	}
    private void OnDrawGizmosSelected()
	{
		int direction = distance >= 0 ? 1 : -1;
		Vector2 dir = (orientation == 0 ? Vector2.right : Vector2.up) * direction; 
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, range);
		Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, -1 * Mathf.Abs(distance) * dir);
	}
}
