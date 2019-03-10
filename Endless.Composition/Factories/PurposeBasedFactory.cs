using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Linq;
using Endless.Composition.Attributes;

namespace Endless.Composition.Factories
{

    [Shared]
    public class PurposeBasedFactory<TExport> : BaseFactory<TExport> where TExport:class
    {
        [ImportingConstructor]
        public PurposeBasedFactory(CompositionHost context) : base(context)
        {
        }

        [ImportMany]
        private IEnumerable<ExportFactory<TExport, ExportPurposeMetaData>> factories { get; set; }

        public TExport GetExport()
        {
            TExport export = null;
            
            var purposes = new ExportPurposeEnum[]
            {
                ExportPurposeEnum.UnitTesting, ExportPurposeEnum.Experimental, ExportPurposeEnum.Bespoke,
                ExportPurposeEnum.Product
            };

            foreach (var purpose in purposes)
            {
                var suitableFactories = this.factories.Where(f => f.Metadata.Purpose == purpose).ToList();
                if (!suitableFactories.Any())
                {
                
                }
                else if (suitableFactories.Count() == 1)
                {
                    export = suitableFactories.First().CreateExport().Value;
                    break;
                }
                else
                {
                    throw new ApplicationException("got found too many exports");
                }
            }
            
            return export;

        }

    }
}