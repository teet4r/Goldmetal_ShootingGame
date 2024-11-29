using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class TagManager : MonoBehaviour
{
    /// <summary>
    /// 저장된 Tag를 const 문자열로 자동 생성
    /// </summary>
    
    private static string _creationPath = $"{Application.dataPath}/Managements/Tag";
    private const string _FILE_NAME = "Tag.cs";

    [MenuItem("Tag/Tag Const String 생성", priority = 100)]
    private static void _GenerateTagStrings()
    {
        var tagAsset = AssetDatabase.LoadAssetAtPath<Object>("ProjectSettings/TagManager.asset");
        var tags = new SerializedObject(tagAsset).FindProperty("tags");
        var tagCount = tags.arraySize;
        StringBuilder sb = new();

        sb.AppendLine("public static class Tag\n{");
        for (int i = 0; i < tagCount; ++i)
        {
            var tag = tags.GetArrayElementAtIndex(i).stringValue;
            sb.AppendLine($"\tpublic const string {tag} = \"{tag}\";");
        }
        sb.Append('}');

        if (!Directory.Exists(_creationPath))
            Directory.CreateDirectory(_creationPath);
        StreamWriter sw = new(File.Create($"{_creationPath}/{_FILE_NAME}"));

        sw.Write(sb.ToString());
        sw.Close();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Generate Tag Const Strings!");
    }
}
