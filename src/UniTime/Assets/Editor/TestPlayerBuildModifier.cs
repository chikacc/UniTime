using System.IO;
using UnityEditor;
using UnityEditor.TestTools;

[assembly: TestPlayerBuildModifier(typeof(TestPlayerBuildModifier))]

public class TestPlayerBuildModifier : ITestPlayerBuildModifier {
    public static bool DidBuild { get; private set; }

    public BuildPlayerOptions ModifyOptions(BuildPlayerOptions playerOptions) {
        playerOptions.locationPathName = Path.Combine(Path.GetFullPath("Builds/PlayerWithTests"),
            Path.GetFileName(playerOptions.locationPathName));
        DidBuild = true;
        return playerOptions;
    }
}
