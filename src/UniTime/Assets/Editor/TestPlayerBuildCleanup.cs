using System;
using System.Linq;
using UnityEditor;
using UnityEngine.TestTools;

[assembly: PostBuildCleanup(typeof(TestPlayerBuildCleanup))]

public class TestPlayerBuildCleanup : IPostBuildCleanup {
    public void Cleanup() {
        if (TestPlayerBuildModifier.DidBuild && Environment.GetCommandLineArgs().Contains("-runTests"))
            EditorApplication.update += () => EditorApplication.Exit(0);
    }
}
