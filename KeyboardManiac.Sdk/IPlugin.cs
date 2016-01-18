using System;
using System.Xml;

namespace KeyboardManiac.Sdk
{
    /// <summary>
    /// The interface that must be implemented to provide plugins to the keyboard maniac engine.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Gets or sets the name of this plugin.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gives the plugin an oportunity to handle the command text.
        /// </summary>
        /// <param name="commandText">The command text to assess.</param>
        /// <returns>
        /// A result object with details on whether the plugin can handle the command.
        /// </returns>
        CommandRequest CanHandleCommand(string commandText);
        /// <summary>
        /// Allows this plugin to initialise itself.
        /// </summary>
        /// <param name="node">The plugin specific setting node.</param>
        void Initialise(XmlNode node);
    }
}
