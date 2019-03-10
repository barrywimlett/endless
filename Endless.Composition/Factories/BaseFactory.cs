using System.Composition;
using System.Composition.Hosting;

namespace Endless.Composition.Factories
{
    abstract public class BaseFactory<TExport>
    {
        private readonly CompositionHost _context;

        [ImportingConstructor]
        protected BaseFactory(CompositionHost context)
        {
            this._context = context;
            _context.SatisfyImports(this);
        }

    }
}