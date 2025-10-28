using System;
using System.Collections;
using GameAudioScriptingEssentials;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class DialogueFX : MonoBehaviour
{
	Text _text;
	TMP_Text _tmpProText;
	string writer;

	[SerializeField] float delayBeforeStart = 0f;
	[SerializeField] float timeBtwChars = 0.1f;
	[SerializeField] string leadingChar = "";
	[SerializeField] bool leadingCharBeforeDelay = false;
	[SerializeField] bool PlayOnStart = false;

	public UnityEvent FinishedText;

	private void Start()
	{
		if (PlayOnStart) PlayText(_text.text);
	}

	// Use this for initialization
	public void PlayText(String text, bool append = false, UnityAction callback = null)
	{
		StopAllCoroutines();
		
		_text = GetComponent<Text>()!;
		_tmpProText = GetComponent<TMP_Text>()!;

		if (_text != null)
		{
			writer = text;
	
			StartCoroutine(TypeWriterText(callback, append));
		}

		if (_tmpProText != null)
		{
			writer = text;

			StartCoroutine(TypeWriterTMP(callback, append));
		}
	}
	
	IEnumerator TypeWriterText(UnityAction callback = null, bool append = false)
	{
		if (append) _text.text += " ";
		else _text.text = leadingCharBeforeDelay ? leadingChar : "";

		yield return new WaitForSeconds(delayBeforeStart);

		foreach (char c in writer)
		{
			if (_text.text.Length > 0)
			{
				_text.text = _text.text.Substring(0, _text.text.Length - leadingChar.Length);
			}
			_text.text += c;
			_text.text += leadingChar;
			
			yield return new WaitForSeconds(timeBtwChars);
		}

		if (leadingChar != "")
		{
			_text.text = _text.text.Substring(0, _text.text.Length - leadingChar.Length);
		}

		FinishedText?.Invoke();
		
		callback?.Invoke();
	}

	IEnumerator TypeWriterTMP(UnityAction callback = null, bool append = false)
	{
		if (append) _tmpProText.text += " ";
		else _tmpProText.text = leadingCharBeforeDelay ? leadingChar : "";

		yield return new WaitForSeconds(delayBeforeStart);

		foreach (char c in writer)
		{
			if (_tmpProText.text.Length > 0)
			{
				_tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
			}
			_tmpProText.text += c;
			_tmpProText.text += leadingChar;
			
			yield return new WaitForSeconds(timeBtwChars);
		}

		if (leadingChar != "")
		{
			_tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
		}
		
		FinishedText?.Invoke();
		
		callback?.Invoke();
	}
}
