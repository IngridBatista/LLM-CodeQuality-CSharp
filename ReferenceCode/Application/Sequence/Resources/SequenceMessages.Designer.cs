namespace Application.Sequence.Resources
{
    using System;

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class SequenceMessages
    {

        private static global::System.Resources.ResourceManager resourceMan;

        private static global::System.Globalization.CultureInfo resourceCulture;

        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SequenceMessages()
        {
        }

        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("code_metrics.Application.Sequence.Resources.SequenceMessages", typeof(SequenceMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        internal static string Double_SequenceTitle
        {
            get
            {
                return ResourceManager.GetString("Double_SequenceTitle", resourceCulture);
            }
        }

        internal static string Double_InputHint
        {
            get
            {
                return ResourceManager.GetString("Double_InputHint", resourceCulture);
            }
        }

        internal static string Fraction_SequenceTitle
        {
            get
            {
                return ResourceManager.GetString("Fraction_SequenceTitle", resourceCulture);
            }
        }

        internal static string Fraction_InputHint
        {
            get
            {
                return ResourceManager.GetString("Fraction_InputHint", resourceCulture);
            }
        }

        internal static string InputPrompt
        {
            get
            {
                return ResourceManager.GetString("InputPrompt", resourceCulture);
            }
        }

        internal static string InvalidInput
        {
            get
            {
                return ResourceManager.GetString("InvalidInput", resourceCulture);
            }
        }

        internal static string Double_InvalidInput
        {
            get
            {
                return ResourceManager.GetString("Double_InvalidInput", resourceCulture);
            }
        }

        internal static string Fraction_InvalidInput
        {
            get
            {
                return ResourceManager.GetString("Fraction_InvalidInput", resourceCulture);
            }
        }

        internal static string ReadingFinished
        {
            get
            {
                return ResourceManager.GetString("ReadingFinished", resourceCulture);
            }
        }

        internal static string EmptySequencesError
        {
            get
            {
                return ResourceManager.GetString("EmptySequencesError", resourceCulture);
            }
        }

        internal static string ResultTitle
        {
            get
            {
                return ResourceManager.GetString("ResultTitle", resourceCulture);
            }
        }

        internal static string NoMatchingFractions
        {
            get
            {
                return ResourceManager.GetString("NoMatchingFractions", resourceCulture);
            }
        }

        internal static string Separator
        {
            get
            {
                return ResourceManager.GetString("Separator", resourceCulture);
            }
        }

        internal static string GetSequenceTitle(string typeName)
        {
            return ResourceManager.GetString($"{typeName}_SequenceTitle", resourceCulture)
                ?? $"\n=== Leitura da Sequência do tipo ({typeName}) ===";
        }

        internal static string GetInputHint(string typeName)
        {
            return ResourceManager.GetString($"{typeName}_InputHint", resourceCulture)
                ?? $"Digite valores do tipo {typeName}:";
        }

        internal static string GetInvalidInputMessage(string typeName)
        {
            return ResourceManager.GetString($"{typeName}_InvalidInput", resourceCulture)
                ?? InvalidInput;
        }
    }
}
