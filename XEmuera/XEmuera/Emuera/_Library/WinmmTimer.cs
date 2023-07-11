using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MinorShift._Library
{
	/// <summary>
	/// wrapされたtimer。外からは、このTickCountだけを呼び出す。
	/// </summary>
	//internal sealed class WinmmTimer
	//{
	//	static WinmmTimer()
	//	{
	//		instance = new WinmmTimer();
	//	}

	//	private WinmmTimer()
	//	{
	//		mm_BeginPeriod(1);
	//	}
	//	~WinmmTimer()
	//	{
	//		mm_EndPeriod(1);
	//	}

	//	/// <summary>
	//	/// 起動時にBeginPeriod、終了時にEndPeriodを呼び出すためだけのインスタンス。
	//	/// staticなデストラクタがあればいらないんだけど
	//	/// </summary>
	//	private static volatile WinmmTimer instance;

	//	/// <summary>
	//	/// timeGetTime()。Windows が起動してから経過した時間(ms)。一周して0になる可能性に注意。
	//	/// </summary>
	//	public static uint TickCount { get { return mm_GetTime(); } }

	//	/// <summary>
	//	/// 現在のフレームの描画に使うためのミリ秒数
	//	/// </summary>
	//	public static uint CurrentFrameTime;
	//	/// <summary>
	//	/// フレーム描画開始合図の時点でのミリ秒を固定するための数値
	//	/// </summary>
	//	public static void FrameStart() { CurrentFrameTime = mm_GetTime(); }

	//	[DllImport("winmm.dll", EntryPoint = "timeGetTime")]
	//	private static extern uint mm_GetTime();
	//	[DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
	//	private static extern uint mm_BeginPeriod(uint uMilliseconds);
	//	[DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
	//	private static extern uint mm_EndPeriod(uint uMilliseconds);
	//}

	internal sealed class WinmmTimer
	{
		/// <summary>
		/// 起動時にBeginPeriod、終了時にEndPeriodを呼び出すためだけのインスタンス。
		/// staticなデストラクタがあればいらないんだけど
		/// </summary>
		private static readonly Stopwatch instance = new Stopwatch();

		/// <summary>
		/// timeGetTime()。Windows が起動してから経過した時間(ms)。一周して0になる可能性に注意。
		/// </summary>
		public static uint TickCount
		{
			get
			{
				if (!instance.IsRunning)
				{
					instance.Start();
				}

				return (uint)instance.ElapsedMilliseconds;
			}
		}

		public static void Reset()
		{
			instance.Reset();
		}

		/// <summary>
		/// 現在のフレームの描画に使うためのミリ秒数
		/// </summary>
		public static uint CurrentFrameTime;
		/// <summary>
		/// フレーム描画開始合図の時点でのミリ秒を固定するための数値
		/// </summary>
		public static void FrameStart()
		{
			CurrentFrameTime = TickCount;
		}
	}
}
