using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace NCMBUT
{
	/// <summary>
	/// フィールドの型チェックを行うUtilクラス
	/// </summary>
	public class NCMBUTTypeUtil
	{
		/// <summary>
		/// 設定した値と、入力した値の型が一致するかチェックする
		/// </summary>
		/// <returns>一致していれば<c>true</c>を返し、一致していなければ<c>false</c>を返す</returns>
		/// <param name="fieldData">フィールドの設定項目</param>
		/// <param name="value">入力した値</param>
		public static bool CheckVariableType(NCMBUTFieldData fieldData, object value)
		{
			switch (fieldData.Type)
			{
				case NCMBUTDataType.STRING:
					if (value.GetType() == typeof(string))
					{
						return true;
					}
					break;
				case NCMBUTDataType.INT:
					if (value.GetType() == typeof(int))
					{
						return true;
					}
					break;
				case NCMBUTDataType.LONG:
					if (value.GetType() == typeof(long))
					{
						return true;
					}
					break;
				case NCMBUTDataType.FLOAT:
					if (value.GetType() == typeof(float))
					{
						return true;
					}
					break;
				case NCMBUTDataType.DOUBLE:
					if (value.GetType() == typeof(double))
					{
						return true;
					}
					break;
				case NCMBUTDataType.DATE_TIME:
					if (value.GetType() == typeof(DateTime))
					{
						return true;
					}
					break;
				case NCMBUTDataType.BOOL:
					if (value.GetType() == typeof(bool))
					{
						return true;
					}
					break;
				case NCMBUTDataType.ARRAY:
					if (value.GetType().IsArray)
					{
						return true;
					}
					break;
				case NCMBUTDataType.LIST:
					if (value is IList && !(value is ArrayList))
					{
						return true;
					}
					break;
				case NCMBUTDataType.DICTIONARY:
					if (value is IDictionary && !(value is Hashtable))
					{
						return true;
					}
					break;
				case NCMBUTDataType.OBJECT:
					return true;
			}
		
			throw new ArgumentException(NCMBUTErrorMessage.NO_DATA_TYPE_MATCH, fieldData.Key);
		}
	}
}