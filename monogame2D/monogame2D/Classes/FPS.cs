using Microsoft.Xna.Framework;
using monogame2D.ATHLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace monogame2D
{
	public class FPS
	{
		private ATHFeuilleText show;
		public FPS(Game1 game)
		{
			ATHComposite n = new ATHComposite(game, Vector2.Zero, Vector2.Zero);
			show = ATHFeuilleText.Create(n, Vector2.Zero, new Vector2(50, 50), "0", Color.Black, 1, Anchor.bottom);
		}

		public long TotalFrames { get; private set; }
		public float TotalSeconds { get; private set; }
		public float AverageFramesPerSecond { get; private set; }
		public float CurrentFramesPerSecond { get; private set; }

		public const int MAXIMUM_SAMPLES = 20;

		private Queue<float> _sampleBuffer = new Queue<float>();


		public bool Update(float deltaTime)
		{	
			CurrentFramesPerSecond = 1.0f / deltaTime;

			_sampleBuffer.Enqueue(CurrentFramesPerSecond);

			if (_sampleBuffer.Count > MAXIMUM_SAMPLES)
			{
				_sampleBuffer.Dequeue();
				AverageFramesPerSecond = _sampleBuffer.Average(i => i);
			}
			else
			{
				AverageFramesPerSecond = CurrentFramesPerSecond;
			}

			TotalFrames++;
			TotalSeconds += deltaTime;
			show.Text = (Math.Floor(AverageFramesPerSecond * 10)/10).ToString();
			return true;
		}
	}
}
