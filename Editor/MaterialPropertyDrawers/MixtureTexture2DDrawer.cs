﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MixtureTexture2DDrawer : MaterialPropertyDrawer
{
    public override void OnGUI (Rect position, MaterialProperty prop, string label, MaterialEditor editor)
    {
        if (!(prop.textureValue is Texture2D) && prop.textureValue != null)
            prop.textureValue = null;
        
        Texture2D value = (Texture2D)EditorGUI.ObjectField(position, prop.displayName, prop.textureValue, typeof(Texture2D),false);

        if (GUI.changed)
            prop.textureValue = value;
    }
}
