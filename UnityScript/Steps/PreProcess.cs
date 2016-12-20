﻿namespace UnityScript.Steps
{
    using Boo.Lang;
    using Boo.Lang.Compiler;
    using Boo.Lang.Compiler.IO;
    using Boo.Lang.Compiler.Steps;
    using Boo.Lang.Useful.IO;
    using System;
    using System.IO;

    [Serializable]
    public class PreProcess : AbstractCompilerStep
    {
        public override void Run()
        {
            List<ICompilerInput> enumerable = new List<ICompilerInput>();
            foreach (ICompilerInput input in this.get_Parameters().get_Input())
            {
                enumerable.Add(this.RunPreProcessorOn(input));
            }
            this.get_Parameters().get_Input().Clear();
            this.get_Parameters().get_Input().Extend(enumerable);
        }

        private StringInput RunPreProcessorOn(ICompilerInput input)
        {
            PreProcessor processor;
            TextReader reader;
            StringInput input2;
            PreProcessor processor1 = processor = new PreProcessor(this.get_Parameters().get_Defines().Keys);
            int num1 = (int) (processor.PreserveLines = true);
            PreProcessor processor2 = processor;
            IDisposable disposable = (reader = input.Open()) as IDisposable;
            try
            {
                StringWriter writer = new StringWriter();
                processor2.Process(reader, writer);
                input2 = new StringInput(input.get_Name(), writer.ToString());
            }
            finally
            {
                if (disposable != null)
                {
                    disposable.Dispose();
                    disposable = null;
                }
            }
            return input2;
        }
    }
}

