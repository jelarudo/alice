using UnityEngine;
using System.Collections;

namespace NCMBUT.EditorTools
{
	public class NCMBUTEditorErrorMessage
	{
#region Input Key Errors
		public static readonly string EMPTY_KEY = "Key can not be empty.";
		public static readonly string DUPLICATION_KEY = "Key is a duplicate.";
		public static readonly string VALIDATION_KEY = "Key can not be used a-z, A-Z, 0-9, _ only.";
		public static readonly string TYPE_KEY = "Key is you can not start from the numerical.";
		public static readonly string RESERVED_KEY = "'_id' Can not be used in key.";
#endregion

#region Ranking Worning
		public static readonly string FORCE_UPDATE = "When ForceUpdate is true, will be updated forced score.";
#endregion

#region ACL Worning
		public static readonly string ACL_IGNORED = "ACL that does not set the permissions will be ignored.";
		public static readonly string ACL_EMPTY= "TargetID can not be empty.";
#endregion
	}
}