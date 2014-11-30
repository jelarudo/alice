using UnityEngine;
using System;

namespace NCMBUT
{
	/// <summary>
	/// ACLの設定項目を管理するクラス
	/// </summary>
	[Serializable]
	public class NCMBUTACLData
	{
		public NCMBUTACLType Type;
		public string ObjectId;
		public bool IsRead;
		public bool IsWrite;
	}
}