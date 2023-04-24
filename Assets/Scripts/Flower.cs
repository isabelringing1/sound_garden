using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flower : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _spriteRenderer;
	[SerializeField] private Sprite _openSprite;
	[SerializeField] private Sprite _semiSprite;
	[SerializeField] private Sprite _closeSprite;

	private bool isOpen = false;
	private Note _note;
	private Instrument _instrument;

	private const float DEFAULT_SPEED = 0.1f;
	
	// Start is called before the first frame update
	public void Initialize(Note note, Instrument instrument)
	{
		_note = note;
		_instrument = instrument;
	}

	public void Open()
	{
		isOpen = true;
		StartCoroutine(AnimateOpen());
		_instrument.PlayNote(_note);
	}
	
	public void Close()
	{
		isOpen = false;
		StartCoroutine(AnimateClose());
		_instrument.StopNote();
	}

	public void Play(float duration)
	{
		isOpen = true;
		StartCoroutine(AnimatePlay(duration));
	}

	private IEnumerator AnimateOpen(float speed = DEFAULT_SPEED)
	{
		_spriteRenderer.sprite = _semiSprite;
		yield return new WaitForSeconds(speed);
		_spriteRenderer.sprite = _openSprite;
		
	}
	
	private IEnumerator AnimateClose(float speed = DEFAULT_SPEED)
	{
		_spriteRenderer.sprite = _semiSprite;
		yield return new WaitForSeconds(speed);
		_spriteRenderer.sprite = _closeSprite;
	}
	
	private IEnumerator AnimatePlay(float duration, float speed = DEFAULT_SPEED)
	{
		_spriteRenderer.sprite = _semiSprite;
		yield return new WaitForSeconds(speed);
		_spriteRenderer.sprite = _openSprite;
		yield return new WaitForSeconds(duration);
		_spriteRenderer.sprite = _semiSprite;
		yield return new WaitForSeconds(speed);
		_spriteRenderer.sprite = _closeSprite;
	}
	

}