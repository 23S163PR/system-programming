﻿namespace TaskManager.Model
{
	public class SystemProcessModel
	{
		public string Name { set; get; }
		public int Threads { set; get; }
		public float Memory { set; get; }
		public float Cpu { set; get; }
		public int ProcessId { set; get; }
	}
}