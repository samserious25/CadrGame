/*
 * FindMissingScriptsRecursively.cs
 * Last modified: 25th Nov 2016
 * 
 * Editor script for Unity 5+. Put this in your Editor folder. 
 * You will then have in the menu "Window->Find Missing Scripts (All)". 
 * Run your scene, click it. Look in the console windows messages for a list of items found.
 * 
 * More info: https://www.gmtdev.com/unity-findmissingscripts/
 * Originally from (small mod to fix for Unity 5): http://wiki.unity3d.com/index.php?title=FindMissingScripts
 */

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class FindMissing : EditorWindow
{
    static int go_count = 0, components_count = 0, missing_count = 0;

    [MenuItem("Window/Find Missing Scripts (All)")]
    static void FindInAll()
    {
        go_count = 0;
        components_count = 0;
        missing_count = 0;

        var controllerScene = SceneManager.GetActiveScene();

        GameObject[] sceneObjects = controllerScene.GetRootGameObjects();
        for (int rootCount = 0; rootCount < controllerScene.rootCount; rootCount++)
            FindInGO(sceneObjects[rootCount].gameObject);

        Debug.Log(string.Format("Searched {0} GameObjects, {1} components, found {2} missing", go_count, components_count, missing_count));
    }

    static void FindInGO(GameObject g)
    {
        go_count++;
        Component[] components = g.GetComponents<Component>();
        for (int i = 0; i < components.Length; i++)
        {
            components_count++;
            if (components[i] == null)
            {
                missing_count++;
                string s = g.name;
                Transform t = g.transform;
                while (t.parent != null)
                {
                    s = t.parent.name + "/" + s;
                    t = t.parent;
                }
                Debug.Log(s + " has an empty script attached in position: " + i, g);
            }
        }
        // Now recurse through each child GO (if there are any):
        foreach (Transform childT in g.transform)
        {
            //Debug.Log("Searching " + childT.name  + " " );
            FindInGO(childT.gameObject);
        }
    }

    static IEnumerable<GameObject> SceneRoots()
    {
        var prop = new HierarchyProperty(HierarchyType.GameObjects);
        var expanded = new int[0];
        while (prop.Next(expanded))
        {
            yield return prop.pptrValue as GameObject;
        }
    }
}