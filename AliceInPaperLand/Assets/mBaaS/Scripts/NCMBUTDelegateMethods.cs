using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NCMB;

public delegate void ListCallback(List<NCMBObject> list, NCMBException error);

public delegate void IntCallback(int num, NCMBException error);

public delegate void StringCallback(string text);

public delegate void ErrorCallBack(NCMBException error);

