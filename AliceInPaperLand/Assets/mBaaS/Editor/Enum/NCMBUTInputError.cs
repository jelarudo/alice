using UnityEngine;
using System.Collections;

namespace NCMBUT.EditorTools
{
	/// <summary>
	/// FreeFieldのキーの入力エラー
	/// </summary>
	public enum NCMBUTInputError
	{
		None,
		Empty,
		Duplicate,
		Validation,
		Type,
		Resorve,
		AclEmpty
	}
}