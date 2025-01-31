using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class FindMissingReferences : EditorWindow
{
    [MenuItem("Tools/Find Missing References in Scene")]
    public static void FindMissingReferencesInScene()
    {
        // Find all GameObjects in the scene
        GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> objectsWithMissingRefs = new List<GameObject>();

        // Loop through all GameObjects in the scene
        foreach (var go in allGameObjects)
        {
            // Get all components attached to the current GameObject
            var components = go.GetComponents<Component>();

            // Loop through each component
            foreach (var c in components)
            {
                // Check if the component is missing (null)
                if (!c)
                {
                    // Log an error indicating a missing component and the GameObject it belongs to
                    Debug.LogError("Missing Component in GO: " + FullPath(go), go);
                    objectsWithMissingRefs.Add(go);
                    continue;
                }

                // Create a SerializedObject to inspect the component's properties
                SerializedObject so = new SerializedObject(c);
                var sp = so.GetIterator();

                // Loop through all serialized properties of the component
                while (sp.NextVisible(true))
                {
                    // Check if the property is of type ObjectReference
                    if (sp.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        // Check if the property has a null object reference, but it's not a zero instance ID
                        if (sp.objectReferenceValue == null
                            && sp.objectReferenceInstanceIDValue != 0)
                        {
                            // Log an error indicating a missing reference in the GameObject's component
                            ShowError(go, c.GetType().Name, ObjectNames.NicifyVariableName(sp.name));
                            objectsWithMissingRefs.Add(go);
                        }
                    }
                }
            }
        }

        // Check if any missing references were found in the scene
        if (objectsWithMissingRefs.Count == 0)
        {
            Debug.Log("No missing references found in scene.");
        }
    }

    // Log an error message for a missing reference
    private static void ShowError(GameObject go, string componentName, string propertyName)
    {
        Debug.LogError("Missing Ref in: GO=" + FullPath(go) + ", Component=" + componentName + ", Property=" + propertyName, go);
    }

    // Get the full path of a GameObject in the scene hierarchy
    private static string FullPath(GameObject go)
    {
        return go.transform.parent == null ? go.name : FullPath(go.transform.parent.gameObject) + "/" + go.name;
    }
}
