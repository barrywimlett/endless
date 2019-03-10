namespace Endless
{
    using System;
    using System.Collections.Generic;
    using System.Composition;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Solarvista (Planner and Business Manager) command line parser for add-ins.
    /// </summary>
    [Shared]
    [Export(typeof(IArgumentProvider))]
    public sealed class ArgumentProvider : IArgumentProvider
    {
        private const string BusinessManagerCommandLine = "Expecting 'FormID=', 'OwnerID=' and 'UserID=' arguments.";
        private const string PlannerCommandLine         = "Expecting '/sysJobNum:#', '/UID:#' and '/sysUserId:#' arguments.";

        private const string ColaborationCommandLine = "Expecting '/ServiceOrdersJobs.UID:##' ..";


        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public ArgumentContext Context
        {
            get;
            private set;
        }


        public void Parse(string[] arguments)
        {
            Contract.Requires(arguments != null, "Arguments must be provided.");

            this.Context = new ArgumentContext();

            // Detect which application called us. This is easier than looking at the parent process.
            if (arguments.Any(arg => arg.Contains("sysJobNum")))
            {
                this.ParsePlannerArguments(arguments);
            }
            else if (arguments.Any(arg => arg.Contains("ServiceOrdersJobs.UID")))
            {
                this.ParseCollaborationArguments(arguments);
            }
            else
            {
                this.ParseBusinessManagerArguments(arguments);
            }
        }

        /// <summary>
        /// Parses the collaboration arguments.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        private void ParseCollaborationArguments(string[] arguments)
        {
            Contract.Assert(arguments.Length > 1, ColaborationCommandLine);

            foreach (string argument in arguments)
            {
                if (argument.ToUpper().StartsWith("/SERVICEORDERSJOBS.UID"))
                {
                    this.Context.FormId = 31;
                    this.Context.OwnerId = int.Parse(argument.Split(':')[1]);
                    break;
                }                
            }
        }

        /// <summary>
        /// Parses the specified arguments.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        private void ParseBusinessManagerArguments(string[] arguments)
        {
            Contract.Assert(arguments.Length == 3, BusinessManagerCommandLine);

            foreach (string argument in arguments)
            {
                if (argument.ToUpper().StartsWith("USERID="))
                {
                    this.Context.UserId = int.Parse(argument.Split('=')[1]);
                }
                else if (argument.ToUpper().StartsWith("OWNERID="))
                {
                    this.Context.OwnerId = int.Parse(argument.Split('=')[1]);
                }
                else if (argument.ToUpper().StartsWith("FORMID="))
                {
                    this.Context.FormId = int.Parse(argument.Split('=')[1]);
                }
            }
        }


        private void ParsePlannerArguments(string[] arguments)
        {
            Contract.Assert(arguments.Length >= 3, PlannerCommandLine);
/* planner
arguments = new String[] { 
    "/SYSUSERID:2",
    "/UID:692",
    "/SysJobNum:905",
};
*/
            foreach (string argument in arguments)
            {
                if (argument.ToUpper().StartsWith("/SYSUSERID:"))
                {
                    this.Context.UserId = int.Parse(argument.Split(':')[1]);
                }
                else if (argument.ToUpper().StartsWith("/SYSJOBNUM:"))
                {
                    this.Context.ServiceOrderJobId = int.Parse(argument.Split(':')[1]);
                }
                else if (argument.ToUpper().StartsWith("/UID:"))
                {
                    this.Context.ServiceOrderJobPersonnelId = int.Parse(argument.Split(':')[1]);
                }
            }
        }

    }
}
