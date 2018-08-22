using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextObject")]
public class TextBaseClass : ScriptableObject
{
	[TextArea]
	string text;
}
