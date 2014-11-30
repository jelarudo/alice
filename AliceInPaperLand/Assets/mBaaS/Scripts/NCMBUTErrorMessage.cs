using UnityEngine;
using System.Collections;

namespace NCMBUT
{
	public class NCMBUTErrorMessage
	{
#region Connection Base Errors
		public static readonly string NO_GIVEN_KEY = "The given key was not present in the dictionary.";
		public static readonly string REQUIRE_KEY = "There is no value in the required field.";
#endregion

#region Connection Errors
		public static readonly string NOT_LOGIN_ERROR = "You are not logged in.";
		public static readonly string NO_MATCH_CLASS_NAME = "Object Class Name does not match.";
		public static readonly string EMPTY_MESSAGE = "Message can not be empty.";
		public static readonly string EMPTY_USER_NAME = "UserName can not be empty.";
		public static readonly string EMPTY_ID_PASS = "UserName or Password can not be empty.";
		public static readonly string USER_NAME_CONDITIONS = "UserName does not meet the conditions.";
		public static readonly string PASSWORD_CONDITIONS = "Password does not meet the conditions.";
		public static readonly string ALREADY_LOGIN = "You have already logged in.";
#endregion

#region Util Errors
		public static readonly string NO_DATA_TYPE_MATCH = "Data type does not match.";
#endregion
	}
}