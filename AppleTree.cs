using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Timers;

public class AppleTree : Form
{	Button start = new Button();
	Button pause = new Button();
	Button clear = new Button();
	Button exit = new Button();

	private Label caught_counter = new Label();
	private Label missed_counter = new Label();
	private Label success_ratio = new Label();

	private const double refresh_rate = 60.0;
	private const double time_converter = 1000.0;
	private const int delta = 15;
	private double dot_update_rate = 1.0;

	private static System.Timers.Timer animation_clock = new System.Timers.Timer();
	private static System.Timers.Timer refresh_clock = new System.Timers.Timer();

	private int radius = 20;
	private int apple_center_x;
	private int apple_center_y;
	private SolidBrush apple_brush = new SolidBrush(Color.Blue);
	Random rdm = new Random();

	private int cursor_x = 0;
	private int cursor_y = 0;

	private double caught = 0.0;
	private double missed = 0.0;
	private double ratio = 0.0;

	public AppleTree()
	{	Text = "Apple Tree";
		Size = new Size(1600, 900);

		apple_center_x = rdm.Next(100, 1500);
		apple_center_y = radius;
		
		start.Text = "start";
		start.Size = new Size(85, 25);
		start.Location = new Point(50, 820);
		pause.Text = "pause";
		pause.Size = new Size(85, 25);
		pause.Location = new Point(150, 820);
		clear.Text = "clear";
		clear.Size = new Size(85, 25);
		clear.Location = new Point(250, 820);
		exit.Text = "exit";
		exit.Size = new Size(85, 25);
		exit.Location = new Point(1500, 820);

		caught_counter.Text = "Apples caught: " + caught.ToString();
		caught_counter.Size = new Size(120, 25);
		caught_counter.Location = new Point(900, 820);
		caught_counter.ForeColor = Color.White;
		caught_counter.BackColor = Color.Black;
		missed_counter.Text = "Apples missed: " + missed.ToString();
		missed_counter.Size = new Size(120, 25);
		missed_counter.Location = new Point(1050, 820);
		missed_counter.ForeColor = Color.White;
		missed_counter.BackColor = Color.Black;
		success_ratio.Text = "Success ratio: ";
		success_ratio.Size = new Size(120, 25);
		success_ratio.Location = new Point(1200, 820);
		success_ratio.ForeColor = Color.White;
		success_ratio.BackColor = Color.Black;

		Controls.Add(start);
		Controls.Add(pause);
		Controls.Add(clear);
		Controls.Add(exit);
		Controls.Add(caught_counter);
		Controls.Add(missed_counter);
		Controls.Add(success_ratio);

		start.Click += new EventHandler(start_click);
		pause.Click += new EventHandler(pause_click);
		clear.Click += new EventHandler(clear_click);
		exit.Click += new EventHandler(exit_click);

		animation_clock.Enabled = false;
		animation_clock.Elapsed += new ElapsedEventHandler(update_apple_position);

		refresh_clock.Enabled = false;
		refresh_clock.Elapsed += new ElapsedEventHandler(update_graphics);

		DoubleBuffered = true;
	}
	
	protected override void OnPaint(PaintEventArgs a)
	{	Graphics board = a.Graphics;
		board.FillRectangle(Brushes.Blue, 0, 0, 1600, 700);
		board.FillRectangle(Brushes.SaddleBrown, 0, 700, 1600, 200);
		board.FillRectangle(Brushes.Gray, 0, 800, 1600, 100);

		board.FillEllipse(apple_brush, apple_center_x-radius, apple_center_y-radius, radius*2, radius*2); 

		base.OnPaint(a);
	}

	protected override void OnMouseDown(MouseEventArgs me)
	{	cursor_x = me.X;
		cursor_y = me.Y;
	}

	protected void start_animation_clock(double updaterate)
	{	double elapsed_time_between_coordinate_changes;
		if(updaterate < 1.0)
			updaterate = 1.0;
		elapsed_time_between_coordinate_changes = time_converter/updaterate;
		animation_clock.Interval = (int)System.Math.Round(elapsed_time_between_coordinate_changes);
		animation_clock.Enabled = true;
	}

	protected void start_refresh_clock(double refreshrate)
	{	double elapsed_time_between_tics;
		if(refreshrate < 1.0)
			refreshrate = 1.0;
		elapsed_time_between_tics = time_converter/refreshrate;
		refresh_clock.Interval = (int)System.Math.Round(elapsed_time_between_tics);
		refresh_clock.Enabled = true;
	}

	protected void update_graphics(System.Object sender, ElapsedEventArgs even)
	{	Invalidate();
	}

	protected void update_apple_position(System.Object sender, ElapsedEventArgs even)
	{	int top = apple_center_y - radius;

		double distance_between_apple_and_cursor = Math.Sqrt((apple_center_x-cursor_x)*(apple_center_x-cursor_x) + (apple_center_y-cursor_y)*(apple_center_y-cursor_y));
		if(distance_between_apple_and_cursor < 20.0)
		{	caught = caught + 1.0;
			apple_center_x = rdm.Next(100, 1500);
			apple_center_y = radius;
			Invalidate();
			caught_counter.Text = "Apples caught: " + caught.ToString();
			if(missed > 0.0)
			{	ratio = caught / missed;
				//ratio= Math.Round(ratio, 2);
				success_ratio.Text = "Success ratio: " + ratio.ToString();
			}
		}

		if(apple_center_y > 700)
		{
			apple_center_x = rdm.Next(100, 1500);
			apple_center_y = radius;

			missed = missed + 1.0;
			missed_counter.Text = "Apples missed: " + missed.ToString();
			ratio = caught / missed;
			//ratio = Math.Round(ratio, 2);
			success_ratio.Text = "Success ratio: " + ratio.ToString();
		}

		apple_center_y += delta;
	}

	protected void start_click(Object sender, EventArgs events)
	{
		animation_clock.Enabled = true;
		refresh_clock.Enabled = true;
		apple_brush.Color = Color.Red;

		System.Console.WriteLine("you've clicked on the start button.");
	}

	protected void pause_click(Object sender, EventArgs events)
	{	if(pause.Text == "pause")
		{
			animation_clock.Enabled = false;
			refresh_clock.Enabled = false;
			pause.Text = "resume";
			System.Console.WriteLine("you've clicked on the pause button.");
		}
		else
		{
			animation_clock.Enabled = true;
			refresh_clock.Enabled = true;
			pause.Text = "pause";
			System.Console.WriteLine("you've clicked on the resume button.");
		}
	}

	protected void clear_click(Object sender, EventArgs events)
	{	caught_counter.Text = "Apples caught: ";
		missed_counter.Text = "Apples missed: ";
		success_ratio.Text = "Success ratio: ";
		if(pause.Text == "resume")
			pause.Text = "pause";
		apple_brush.Color = Color.Blue;
		apple_center_x = rdm.Next(100, 1500);
		apple_center_y = radius;
		animation_clock.Enabled = false;
		refresh_clock.Enabled = false;
		Invalidate();

		System.Console.WriteLine("you've clicked on the clear button.");
	}

	protected void exit_click(Object sender, EventArgs events)
	{	System.Console.WriteLine("you've clicked on the exit button.");
		Close();
	}
}