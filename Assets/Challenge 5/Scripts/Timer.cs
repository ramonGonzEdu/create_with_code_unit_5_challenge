using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
	public float timeLeft = 60.99f;
	public bool running = false;
	private GameManagerX gameManager;
	private TMPro.TextMeshProUGUI text;
	private void Start()
	{
		text = GetComponent<TMPro.TextMeshProUGUI>();
		gameManager = GameObject.Find("Game Manager").GetComponent<GameManagerX>();
		gameManager.listenStart(this, "enable");
		gameManager.listenEnd(this, "disable");
	}

	private void enable()
	{
		running = true;
	}

	private void disable()
	{
		running = false;
	}

	private void Update()
	{
		if (running)
		{
			timeLeft -= Time.deltaTime;

			if (timeLeft < 1)
			{
				timeLeft = 0;
				gameManager.GameOver();
				// running = false;
			}

			text.text = "Time Left: " + (int)Math.Floor(timeLeft);
		}



	}
}