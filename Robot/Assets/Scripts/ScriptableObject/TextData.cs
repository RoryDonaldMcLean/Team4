using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TextData")]
public class TextData : ScriptableObject {
    [TextArea]
    public string[] Text;
}
