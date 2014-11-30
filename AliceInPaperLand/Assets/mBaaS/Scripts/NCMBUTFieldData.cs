using UnityEngine;
using System;
using System.Collections.Generic;

namespace NCMBUT
{
	/// <summary>
	/// フィールドの設定項目を管理するクラス
	/// </summary>
	[Serializable]
	public class NCMBUTFieldData
	{
		public bool IsRequire;
		public string Key;
		public NCMBUTDataType Type;
		public object Value;
	}
}
