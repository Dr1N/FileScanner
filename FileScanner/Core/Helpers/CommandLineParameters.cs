using CommandLine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileScanner.Core
{
    public enum Action
    {
        None,
        All,
        Cs,
        ReversedOne,
        ReversedTwo,
    }

    class CommandLineParameters
    {
        #region Constants

        private readonly IReadOnlyList<string> AvailableActions = new List<string>() { "all", "cs", "reversed1", "reversed2" };

        private readonly IReadOnlyDictionary<string, Action> Actions = new Dictionary<string, Action>()
        {
            { "ALL", Action.All },
            { "CS", Action.Cs },
            { "REVERSED1", Action.ReversedOne },
            { "REVERSED2", Action.ReversedTwo },
        };

        #endregion

        #region Properties

        [Option('s', "start", Required = true, HelpText = "Start directory")]
        public string StartDirectory { get; set; }

        [Option('a', "action", Required = false, DefaultValue = "all", HelpText = "Action (all, cs, reversed1, reversed2)")]
        public string ActionStr { get; set; }

        [Option('r', "result", Required = false, DefaultValue = "results.txt", HelpText = "Results file ('console' - for console output)")]
        public string ResultFile { get; set; }

        #endregion

        public static CommandLineParameters MakeParameters(string[] args)
        {
            CommandLineParameters options = null;
            try
            {
                var parseResult = Parser.Default.ParseArguments<CommandLineParameters>(args);
                if (!parseResult.Errors.Any())
                {
                    options = parseResult.Value;
                }
            }
            catch 
            {
                // ignored
            }

            return options;
        }

        public bool IsValid()
        {
            bool isValidDirectory = !string.IsNullOrWhiteSpace(StartDirectory) && Directory.Exists(StartDirectory);
            bool isValidResultFile = !string.IsNullOrWhiteSpace(ResultFile);
            bool isValidAction = AvailableActions.Any(a => a.ToUpper() == ActionStr.Trim().ToUpper());

            return isValidDirectory && isValidResultFile && isValidAction;
        }

        public Action GetAction()
        {
            var result = Action.None;
            if (Actions.TryGetValue(ActionStr.Trim().ToUpper(), out Action action))
            {
                result = action;
            }

            return result;
        }

        public bool IsConsole()
        {
            var resultFile = ResultFile.Trim().ToUpper();
            var console = "console".ToUpper();
            var result = resultFile == console;

            return result;
        }
    }
}