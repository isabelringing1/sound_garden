using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Flower : MonoBehaviour
{
	public bool IsOpen = false;
	
	[SerializeField] private SpriteRenderer _spriteRenderer;
	[SerializeField] private Sprite _openSprite;
	[SerializeField] private Sprite _semiSprite;
	[SerializeField] private Sprite _closeSprite;

	private Note _note;
	private Instrument _instrument;

	private const float DEFAULT_SPEED = 0.1f;
	
	// Start is called before the first frame update
	public void Initialize(Note note, Instrument instrument)
	{
		_note = note;
		_instrument = instrument;
		_instrument.Initialize();
	}

	public void Open(bool playNote = true)
	{
		IsOpen = true;
		StartCoroutine(AnimateOpen());
		if (playNote)
		{
			_instrument.PlayNote(_note);
		}
	}
	
	public void Close(bool stopNote = true)
	{
		if (!IsOpen)
		{
			return;
		}
		IsOpen = false;
		StartCoroutine(AnimateClose());
		if (stopNote)
		{
			_instrument.StopNote(_note);
		}
	}

	public void Play(float duration)
	{
		IsOpen = true;
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