using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;
using Microsoft.Maui.Graphics;

namespace Clock
{
	public class ClockView:GraphicsView, IDrawable
    {
        private TimeSpan? _offset;

        public TimeSpan? Offset
        {
            get => _offset;            
            set
            {
                _offset = value;
                Invalidate();            
            }
        }
        private static Dictionary<int, string> Segments = new()
		{
			{ 0, "ABCDEF"},
			{ 1, "BC"},
			{ 2, "ABDEG"},
			{ 3, "ABCDG"},
			{ 4, "BCFG"},
			{ 5, "ACDFG"},
			{ 6, "ACDEFG"},
			{ 7, "ABC"},
			{ 8, "ABCDEFG"},
			{ 9, "ABCDFG"}
		};
		private IDispatcherTimer _timer;
		private int _lastsecond = -1;
		public ClockView()
		{
            Drawable = this;
            _timer = Dispatcher.CreateTimer();
			_timer.Interval = TimeSpan.FromMilliseconds(500);
			_timer.Tick += OnTick;
			_timer.Start();
		}

        private void OnTick(object? sender, EventArgs e)
        {
            var second = DateTime.Now.Second;
			if (second != _lastsecond)
			{
				_lastsecond = second;
				Invalidate();
			}
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
		{
            var background = Colors.Black;
            var off = Color.FromArgb("#013220");
            var on = Color.FromArgb("#37FD12");

            canvas.FillColor = background;
            canvas.FillRectangle(dirtyRect);

            float slot = dirtyRect.Width / 8;
            float w = slot * 0.8f;
            float h = Math.Min(dirtyRect.Height * 0.6f, 2 * w);
            float y = (dirtyRect.Height - h) / 2;

            DateTime now;
            if (Offset == null)
                now = DateTime.Now;               
            else
                now = (DateTime)(DateTime.UtcNow + Offset);   
            int[] digits = { now.Hour / 10, now.Hour % 10, now.Minute / 10, now.Minute % 10, now.Second / 10, now.Second % 10, };
            int[] slots = { 0, 1, 3, 4, 6, 7 };
            canvas.FillColor = on;
            canvas.FillCircle(2 * slot + slot/2, y + h / 2 - h / 4, w / 10);
            canvas.FillCircle(2 * slot + slot / 2, y + h / 2 + h / 4, w / 10);
            canvas.FillCircle(5 * slot + slot / 2, y + h / 2 - h / 4, w / 10);
            canvas.FillCircle(5 * slot + slot / 2, y + h / 2 + h / 4, w / 10);
            for (int i = 0; i < 6; i++) 
            {
                float x = slots[i] * slot + (slot - w) / 2;

                DrawSegments(canvas, "ABCDEFG", off, x, y, w, h);
                DrawSegments(canvas, Segments[digits[i]], on, x, y, w, h);
            }
        }

        private void DrawSegments(ICanvas canvas, string segments, Color color,
                              float x, float y, float w, float h)
        {
            float t = w / 6;          
            float g = (float)(t * 1.3);             

            canvas.StrokeColor = color;
            canvas.StrokeSize = t;
            canvas.StrokeLineCap = LineCap.Round;

            foreach (var s in segments)
            {
                switch (s)
                {
                    case 'A':  
                        canvas.DrawLine(x + g, y + (t / 2), x + w - g, y + (t / 2));
                        break;
                    case 'B':
                        canvas.DrawLine(x + w - t/2, y + g, x + w - t / 2, y + h / 2 - g);
                        break;
                    case 'C':
                        canvas.DrawLine(x+w - t/2, y + h / 2 + g, x + w - t / 2, y + h - g);
                        break;
                    case 'D':
                        canvas.DrawLine(x + g, y + h - (t / 2), x + w - g, y + h - (t / 2));
                        break;
                    case 'F':
                        canvas.DrawLine( x + t/2, y + g, x + t / 2, y + h / 2 - g);
                        break;
                    case 'E':
                        canvas.DrawLine(x + t/2, y + h / 2 + g, x + t/2, y + h - g);
                        break;
                    case 'G':
                        canvas.DrawLine(x + g, y + (h / 2), x + w - g, y + (h / 2));
                        break;

                }
            }
        }
    }
}

