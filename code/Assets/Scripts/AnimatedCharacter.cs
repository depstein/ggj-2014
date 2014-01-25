using UnityEngine;
using System.Collections;

public class AnimatedCharacter : MonoBehaviour {
	private Animator _animator; 
	private GameObject _back;
	private GameObject _front;
	public bool facingForward = true;

	void Awake()
	{
		_animator = GetComponent<Animator>();

		_back = transform.Find("body/back").gameObject;
		_front = transform.Find("body/front").gameObject;

		ShowFront();
	}

	void Update()
	{
		Vector3 velocity = rigidbody2D.velocity;
		_animator.SetFloat("SpeedSqr", velocity.sqrMagnitude);

		if (velocity.y > 0)
		{
			ShowBack();
		}
		if (velocity.y < 0)
		{
			ShowFront();
		}
	}

	private void ShowBack()
	{
		_back.SetActive(true);
		_front.SetActive(false);
		facingForward = false;
	}

	private void ShowFront()
	{
		_front.SetActive(true);
		_back.SetActive(false);
		facingForward = true;
	}
}