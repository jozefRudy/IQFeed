using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine.Text;
using CommandLine;


namespace FuturesDownloader
{
    class ParseOptions
    {
        [ParserState]
        public IParserState LastParserState { get; set; }

        [Option("symbol", Required = true, HelpText = "symbol to download")]
        public string symbol { get; set; }

        [Option("start_date", Required = true, HelpText = "starting date, e.g. 2010-01-01")]
        public string start_date { get; set; }

        [HelpOption("?", HelpText = "Display this help")]
        public string GetUsage()
        {
            var help = new HelpText();

            if (this.LastParserState?.Errors.Any() == true)
            {
                var errors = help.RenderParsingErrorsText(this, 2);

                if (!string.IsNullOrEmpty(errors))
                {
                    help.AddPostOptionsLine(string.Concat(Environment.NewLine, "ERROR(S):"));
                    help.AddPostOptionsLine(errors);
                }
            }

            help.AddDashesToOption = true;
            help.Copyright = new CopyrightInfo("Rudy Consulting", 2017);
            help.AddOptions(this);
            return help;
        }      
    }
}
