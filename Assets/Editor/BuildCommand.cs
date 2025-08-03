
using UnityEditor;
using System.Linq;

public class BuildCommand
{
    public static void Build()
    {
        var scenes = EditorBuildSettings.scenes.Select(s => s.path).ToArray();
        var buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = "Builds/macOS/MyUnity2D.app",
            target = BuildTarget.StandaloneOSX,
            options = BuildOptions.None,
        };

        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}
