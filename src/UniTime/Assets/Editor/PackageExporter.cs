using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using static System.IO.Directory;
using static System.IO.Path;

public static class PackageExporter {
    static readonly string ProjectPath = GetFullPath(".");
    static readonly string PluginsPath = GetFullPath(Combine("Assets", "Plugins"));
    static readonly string PackagePath = GetFullPath(Combine("Assets", "Plugins", "UniTime"));
    static readonly string UnitypackageExtension = "unitypackage";

    [MenuItem("Tools/Export UniTime Package...")]
    public static void Export() {
        var version = GetVersion(PackagePath);
        var fileNameWithoutExtension = PackagePath.Split(DirectorySeparatorChar, AltDirectorySeparatorChar)[^1];
        var fileName = string.IsNullOrEmpty(version) ? $"{fileNameWithoutExtension}.{UnitypackageExtension}"
            : $"{fileNameWithoutExtension}.{version}.{UnitypackageExtension}";
        var exportPath = Application.isBatchMode ? GetFullPath(fileName)
            : EditorUtility.SaveFilePanel("Export package...", string.Empty, fileName, UnitypackageExtension);
        if (string.IsNullOrEmpty(exportPath)) return;
        var plugins = EnumerateFiles(PluginsPath, "*", SearchOption.AllDirectories)
            .Select(GetRelativePath);
        var assets = EnumerateFiles(PackagePath, "*", SearchOption.AllDirectories)
            .Select(GetRelativePath).Union(plugins).ToArray();
        AssetDatabase.ExportPackage(assets, exportPath);
        var builder = new StringBuilder();
        builder.AppendFormat("Export completed. ({0})", GetFullPath(exportPath));
        foreach (var asset in assets) builder.AppendFormat("\n{0}", asset);
        if (Application.isBatchMode) {
            Console.WriteLine(builder.ToString());
            EditorApplication.Exit(0);
            return;
        }

        EditorUtility.RevealInFinder(exportPath);
        Debug.Log(builder.ToString());
    }

    static string GetRelativePath(string path) => Path.GetRelativePath(ProjectPath, path)
        .Replace(DirectorySeparatorChar, AltDirectorySeparatorChar);

    static string GetVersion(string packagePath) {
        var version = Environment.GetEnvironmentVariable("UNITY_PACKAGE_VERSION");
        var versionJson = Combine(packagePath, "package.json");
        if (File.Exists(versionJson)) {
            var v = JsonUtility.FromJson<Version>(File.ReadAllText(versionJson));
            if (!string.IsNullOrEmpty(version)) {
                var message =
                    $"package.json and env version are mismatched. UNITY_PACKAGE_VERSION: {version}, package.json: {v.version}";
                if (v.version != version) {
                    if (Application.isBatchMode) {
                        Console.WriteLine(message);
                        EditorApplication.Exit(1);
                    }

                    throw new ArgumentException(message);
                }
            }

            return v.version;
        }

        return version;
    }

    struct Version {
        // ReSharper disable once InconsistentNaming
        public string version;
    }
}
