namespace Endless
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// IDependencyResolver interface
    /// </summary>
    public interface IDependencyResolver
    {
        /// <summary>
        /// Exports the specified type.
        /// </summary>
        /// <param name="exportType">Type of the export.</param>
        /// <returns>The exported instance.</returns>
        object Export(Type exportType);

        /// <summary>
        /// Exports the specified type.
        /// </summary>
        /// <typeparam name="T">The type to export</typeparam>
        /// <returns>The exported instance.</returns>
        T Export<T>();

        /// <summary>
        /// Gets a set of exports.
        /// </summary>
        /// <param name="exportType">Type of the export.</param>
        /// <returns>An enumeration of the exported type</returns>
        IEnumerable<object> GetExports(Type exportType);

        /// <summary>
        /// Gets a set of exports.
        /// </summary>
        /// <typeparam name="T">The esport type</typeparam>
        /// <returns>An enumeration of the exported type</returns>
        IEnumerable<T> GetExports<T>();
    }
}
