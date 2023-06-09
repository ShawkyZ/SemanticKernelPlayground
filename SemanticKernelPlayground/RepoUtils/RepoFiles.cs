﻿// Copyright (c) Microsoft. All rights reserved.

using System.Reflection;

namespace SemanticKernelPlayground.RepoUtils;

internal static class RepoFiles
{
    /// <summary>
    /// Scan the local folders from the repo, looking for "samples/skills" folder.
    /// </summary>
    /// <returns>The full path to samples/skills</returns>
    public static string SkillsPath()
    {
        const string Folder = "Skills";

        bool SearchPath(string pathToFind, out string result, int maxAttempts = 10)
        {
            var currDir = Path.GetFullPath(Assembly.GetExecutingAssembly().Location);
            bool found;
            do
            {
                result = Path.Join(currDir, pathToFind);
                found = Directory.Exists(result);
                currDir = Path.GetFullPath(Path.Combine(currDir, ".."));
            } while (maxAttempts-- > 0 && !found);

            return found;
        }

        if (!SearchPath(Path.DirectorySeparatorChar + Folder, out string path)
            && !SearchPath(Folder, out path))
        {
            throw new Exception("Skills directory not found. The app needs the skills from the repo to work.");
        }

        return path;
    }
}
