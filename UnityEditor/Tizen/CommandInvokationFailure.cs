﻿namespace UnityEditor.Tizen
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEditor.Utils;

    internal class CommandInvokationFailure : Exception
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string[] <Errors>k__BackingField;
        public readonly string Args;
        public readonly string Command;
        public readonly int ExitCode;
        public readonly string HighLevelMessage;
        public readonly string[] StdErr;
        public readonly string[] StdOut;

        public CommandInvokationFailure(string message, Program p)
        {
            this.HighLevelMessage = message;
            this.Command = p.GetProcessStartInfo().FileName;
            this.Args = p.GetProcessStartInfo().Arguments;
            this.ExitCode = p.ExitCode;
            this.StdOut = p.GetStandardOutput();
            this.StdErr = p.GetErrorOutput();
            this.Errors = new string[0];
        }

        public string CommandString() => 
            (this.Command + " " + this.Args);

        public string StdErrString() => 
            string.Join("\n", this.StdErr);

        public string StdOutString() => 
            string.Join("\n", this.StdOut);

        public override string ToString()
        {
            string highLevelMessage = this.HighLevelMessage;
            if (this.Errors != null)
            {
                foreach (string str2 in this.Errors)
                {
                    highLevelMessage = highLevelMessage + "\n" + str2;
                }
            }
            return (highLevelMessage + "\n\n" + base.ToString());
        }

        public string[] Errors { get; set; }

        public override string Message =>
            $"{this.HighLevelMessage}
{this.CommandString()}

stderr[
{this.StdErrString()}
]
stdout[
{this.StdOutString()}
]";
    }
}

