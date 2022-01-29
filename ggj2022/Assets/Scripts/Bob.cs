using UnityEngine;

public class Bob : MonoBehaviour {

	public float Speed = 1.0f;
	public float Distance = 1.0f;
    public float Offset = 0.0f;
	private Vector3 startPosition;

	void Start ()
    {
		this.startPosition = transform.position;
	}
	
	void Update ()
    {
		transform.position = startPosition
            + new Vector3(0, this.Distance * Mathf.Sin(Time.time * this.Speed + this.Offset), 0);
	}
}
