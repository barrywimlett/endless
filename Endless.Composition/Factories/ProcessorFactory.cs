using System.Composition;
using System.Composition.Hosting;
using IProcessorMetaData = Endless.Composition.Interfaces.IProcessorMetaData;

// THESE FILE REQUIRE MEF aka System.Composition - and have a Build Action of None

namespace Endless.Composition.Factories
{
    public class ProcessorMetaData : IProcessorMetaData
    {
        public int EventId { get; set; }
    }

    /*
    /// <summary>
    /// An MEF ExportFactory 
    /// </summary>
    [Shared]
    public class ProcessorFactory : MetaDataFactory<ClassFactory.iProcessor,ProcessorMetaData,int>
    {
        override protected bool FactoryFilter(ProcessorMetaData metadata, int value)
        {
            return metadata.EventId == value;
        }

        [ImportingConstructor]
        public ProcessorFactory(CompositionHost context) : base(context)
        {
        }
    }
    */
    public class ClassFactory
    {
    }
}



