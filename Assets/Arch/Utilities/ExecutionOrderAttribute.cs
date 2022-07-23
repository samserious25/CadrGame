using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Assets.Arch
{
    [AttributeUsage( AttributeTargets.Class, Inherited = false, AllowMultiple = false )]
    internal sealed class ExecutionOrderAttribute : Attribute {
        private readonly int _executionOrder = 0;

        public ExecutionOrderAttribute( int executionOrder ) {
            _executionOrder = executionOrder;
        }

#if UNITY_EDITOR
        private const string PbTitle = "Updating Execution Order";
        private const string PbMessage = "Hold on to your butt, Cap'n!";
        private const string ErrMessage = "Unable to locate and set execution order for {0}";

        [InitializeOnLoadMethod]
        private static void Execute() {
            var type = typeof( ExecutionOrderAttribute );
            var assembly = type.Assembly;
            var types = assembly.GetTypes();
            var scripts = new Dictionary<MonoScript, ExecutionOrderAttribute>();

            var progress = 0f;
            var step = 1f / types.Length;

            foreach ( var item in types ) {
                var attributes = item.GetCustomAttributes( type, false );
                if ( attributes.Length != 1 ) continue;
                var attribute = attributes[0] as ExecutionOrderAttribute;

                var asset = "";
                var guids = AssetDatabase.FindAssets($"{item.Name} t:script");

                if ( guids.Length > 1 ) {
                    foreach ( var guid in guids ) {
                        var assetPath = AssetDatabase.GUIDToAssetPath( guid );
                        var filename = Path.GetFileNameWithoutExtension( assetPath );
                        if (filename != item.Name) continue;
                        asset = guid;
                        break;
                    }
                } else if ( guids.Length == 1 ) {
                    asset = guids[0];
                } else {
                    Debug.LogErrorFormat( ErrMessage, item.Name );
                    return;
                }

                var script = AssetDatabase.LoadAssetAtPath<MonoScript>( AssetDatabase.GUIDToAssetPath( asset ) );
                scripts.Add( script, attribute );
            }

            var changed = false;
            foreach ( var item in scripts )
            {
                if (MonoImporter.GetExecutionOrder(item.Key) == item.Value._executionOrder) continue;
                changed = true;
                break;
            }

            if ( changed ) {
                foreach ( var item in scripts ) {
                    var cancelled = EditorUtility.DisplayCancelableProgressBar( PbTitle, PbMessage, progress );
                    progress += step;

                    if ( MonoImporter.GetExecutionOrder( item.Key ) != item.Value._executionOrder ) {
                        MonoImporter.SetExecutionOrder( item.Key, item.Value._executionOrder );
                    }

                    if ( cancelled ) break;
                }
            }

            EditorUtility.ClearProgressBar();
        }
#endif
    }
}
