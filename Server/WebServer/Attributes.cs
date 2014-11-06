using System;
using System.Collections.Generic;
using System.Reflection;

namespace WebServer
{
	[AttributeUsage(AttributeTargets.Method)]
	public class CallPriorityAttribute : Attribute
	{
		public int Priority { get; set; }

		public CallPriorityAttribute(int priority)
		{
			Priority = priority;
		}
	}

	public class CallPriorityComparer : IComparer<MethodInfo>
	{
		public int Compare(MethodInfo x, MethodInfo y)
		{
			if (x == null && y == null)
				return 0;

			if (x == null)
				return 1;

			if (y == null)
				return -1;

			return GetPriority(x) - GetPriority(y);
		}

		private int GetPriority(MethodInfo mi)
		{
			object[] objects = mi.GetCustomAttributes(typeof(CallPriorityAttribute), true);

			if (objects == null)
				return 0;

			if (objects.Length == 0)
				return 0;

			CallPriorityAttribute attribute = objects[0] as CallPriorityAttribute;

			if (attribute == null)
				return 0;

			return attribute.Priority;
		}
	}
}
