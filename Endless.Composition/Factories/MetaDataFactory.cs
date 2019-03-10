using System.Collections.Generic;

using System.Composition;
using System.Composition.Hosting;
using System.Linq;

namespace Endless.Composition.Factories
{
    abstract public class MetaDataFactory<TExport,TMetaData,TValue> : BaseFactory<TExport>
    {
        [ImportMany]
        protected IEnumerable<ExportFactory<TExport, TMetaData>> Factories { get; set; }

        public IEnumerable<TExport> GetExports(TValue eventId)
        {
            IList<TExport> loggers = new List<TExport>();
            var suitableFactories = this.Factories.Where(factory => FactoryFilter(factory.Metadata, eventId));
            var bestFactories = suitableFactories;

            return bestFactories.Select(f => f.CreateExport().Value);
        }

        protected IEnumerable<ExportFactory<TExport, TMetaData>> FilteredFactories(TValue value)
        {
            return this.Factories.Where(f => FactoryFilter(f.Metadata, value));
        }

        abstract protected bool FactoryFilter(TMetaData metadata, TValue value);

        [ImportingConstructor]
        protected MetaDataFactory(CompositionHost context) : base(context)
        {
        }
    }
}