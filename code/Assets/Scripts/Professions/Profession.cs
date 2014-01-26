using UnityEngine;
using System.Collections;

public class Profession : AreaObject {
	protected Animator _animator;
	protected Player _player;
	protected AnimatedCharacter _animatedCharacter;

	protected virtual void Start() {
		_animator = GetComponent<Animator>();
		_player = GetComponent <Player> ();
		_animatedCharacter = GetComponent<AnimatedCharacter>();
	}
}
