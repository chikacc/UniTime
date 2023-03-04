using System;
using System.Linq;
using UnityEditor;
using UnityEngine.TestTools;

[assembly: PostBuildCleanup(typeof(BuildTestPlayerTearDown))]

public class BuildTestPlayerTearDown : IPostBuildCleanup {
    public void Cleanup() {
        if (BuildTestPlayerSetUp.DidBuild && Environment.GetCommandLineArgs().Contains("-runTests"))
            EditorApplication.update += () => EditorApplication.Exit(0);
    }
}
