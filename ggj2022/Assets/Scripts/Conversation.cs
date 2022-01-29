using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Conversation : ScriptableObject {
    [Serializable]
    public class Path {
        public string Name;
        public List<Entry> Entries = new List<Entry>();
        public List<Option> Options = new List<Option>();
    }

    [Serializable]
    public class Option {
        public string Text;
        public string Path;
        public List<string> AddStates = new List<string>();
        public List<string> RemoveStates = new List<string>();
    }

    [Serializable]
    public class Entry {
        public string Text;
    }

    public List<Path> Paths = new List<Path>();

    [TextArea(minLines: 30, maxLines: 0)]
    public string Config;

    protected void OnEnable() {
        if (Config == null) {
            return;
        }
        Paths = new List<Path>();
        string[] parts = Config.Split('\n');

        Path currentPath = new Path();
        Paths.Add(currentPath);
        Option currentOption = null;

        foreach (string rawPart in parts) {
            string part = rawPart.Trim();
            if (part.Length == 0) {
                continue;
            }
            if (part.StartsWith("<")) {
                int end = part.IndexOf(">");
                if (end < 0) {
                    continue;
                }
                string newPathName = part.Substring(1, end - 1);
                currentPath = new Path();
                Paths.Add(currentPath);
                currentPath.Name = newPathName;
                currentOption = null;
            } else if (part.StartsWith("[")) {
                int end = part.IndexOf("]");
                if (end < 0) {
                    continue;
                }
                string optionName = part.Substring(1, end - 1);
                string text = part.Substring(end + 1, part.Length - end - 1).Trim();
                currentOption = new Option { Text = text, Path = optionName };
                currentPath.Options.Add(currentOption);
                text.Trim();
            } else if (part.StartsWith("{")) {
                int end = part.IndexOf("}");
                if (end < 0) {
                    continue;
                }
                string optionName = part.Substring(1, end - 1);
                bool isRemoval = false;
                if (optionName.StartsWith("-")) {
                    isRemoval = true;
                    optionName = optionName.Substring(1, optionName.Length - 1).Trim();
                }
                if (currentOption == null) {
                    continue;
                }
                if (isRemoval) {
                    currentOption.RemoveStates.Add(optionName);
                } else {
                    currentOption.AddStates.Add(optionName);
                }
            } else {
                currentPath.Entries.Add(new Entry { Text = part.Trim() });
            }
        }
    }
}
