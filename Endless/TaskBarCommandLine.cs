namespace Endless
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    /// <summary>
    /// SolarvistataskBarCommandLine
    /// </summary>
    public class TaskBarCommandLine
    {
        /// <summary>
        /// The pattern
        /// </summary>
        private const string CommandLinePattern = @"(?<arg>[a-zA-Z]+)=(?<value>[^;]+);?";

        /// <summary>
        /// The lazy pattern
        /// </summary>
        private static readonly Lazy<Regex> LazyPattern = new Lazy<Regex>(() => new Regex(CommandLinePattern, RegexOptions.Compiled | RegexOptions.IgnoreCase));

        /// <summary>
        /// Gets or sets the data DSN.
        /// </summary>
        /// <value>
        /// The data DSN.
        /// </value>
        public string DataDsn { get; set; }

        /// <summary>
        /// Gets or sets the system DSN.
        /// </summary>
        /// <value>
        /// The system DSN.
        /// </value>
        public string SystemDsn { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the access code.
        /// </summary>
        /// <value>
        /// The access code.
        /// </value>
        public string AccessCode { get; set; }

        /// <summary>
        /// Parses the specified arguments.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">
        /// argument or value groups missing in  + CommandLinePattern
        /// or
        /// Unexpected key argument
        /// </exception>
        public static TaskBarCommandLine Parse(string[] arguments)
        {
            Contract.Requires(arguments != null);

            Contract.Assert(!arguments.Any(a => string.IsNullOrWhiteSpace(a)));

            var res = new TaskBarCommandLine();

            foreach (string argument in arguments)
            {
                MatchCollection mc = LazyPattern.Value.Matches(argument);

                foreach (Match m in mc)
                {
                    Group ng = m.Groups["arg"];
                    Group vg = m.Groups["value"];

                    if (ng == null || vg == null)
                    {
                        throw new InvalidOperationException("argument or value groups missing in " + CommandLinePattern);
                    }
                    else
                    {
                        var key = ng.Value;

                        var value = vg.Value;

                        switch (key.ToUpperInvariant())
                        {
                            case "DATADSN":
                                res.DataDsn = value;
                                break;

                            case "SYSTEMDSN":
                                res.SystemDsn = value;
                                break;

                            case "USERID":
                                res.UserId = Convert.ToInt32(value);
                                break;

                            case "ACCESSCODE":
                                res.AccessCode = value;
                                break;

                            default:
                                throw new InvalidOperationException("Unexpected key " + key + " in arg " + argument);

                        }
                    }
                }
            }

            return res;
        }
    }
}
