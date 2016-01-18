using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyboardManiac.Sdk
{
    public class CommandRequest
    {
        /// <summary>
        /// Gets or sets the command text after the matching alias has been removed.
        /// </summary>
        public string AliasCleansedCommandText { get; set; }
        public bool CanHandleCommand { get; set; }
        public string CommandText { get; set; }
        public string MatchingAlias { get; set; }
    }
}
