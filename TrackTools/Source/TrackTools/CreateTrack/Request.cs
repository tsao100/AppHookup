/*
 * 由SharpDevelop创建。
 * 用户： Jack
 * 日期: 2019/8/2
 * 时间: 下午 05:00
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Threading;

namespace TrackTools.CreateTrack
{
	
	public enum RequestId : int
   {
       /// <summary>
       /// None
       /// </summary>
       None = 0,
       /// <summary>
       /// "Delete" request
       /// </summary>
       Test = 1,
       BatchBuild = 2
	}
	/// <summary>
	/// Description of Request.
	/// </summary>
	public class Request
	{
		private int m_request = (int)RequestId.None;
		
		public RequestId Take()
		{
			return (RequestId)Interlocked.Exchange(ref m_request, (int)RequestId.None);
		}

		public void Make(RequestId request)
		{
			Interlocked.Exchange(ref m_request, (int)request);
		}

	}
}
