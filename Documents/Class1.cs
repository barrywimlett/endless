using System;
using System.Collections.Generic;
using System.Linq;

namespace Documents
{
    public class BaseDocument
    {
        public Guid DocumentTypeID { get; protected set; }

        public DateTime DocumentDate { get; set; }

        
    }

    public class BaseDocumentRow
    {
        public BaseDocumentRow Parent { get; set; }
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? UntilDate { get; set; }

        public string Description { get; set; }
    }

    public class DocumentValueRow : BaseDocumentRow {
        public Decimal Value { get; set; }

    }

    public abstract class DocumentCalculatedValueRow : BaseDocumentRow
    {
        public DocumentCalculatedValueRow SourceRow { get; set; }
        public DocumentCalculatedValueRow SourceRowID { get; set; }

        public abstract Decimal Value { get; }

    }

    public class DocumentQuantityRow : DocumentCalculatedValueRow
    {
        public Decimal Quantity { get; set; }
        public Decimal UnitCost { get; set; }

        public override decimal Value
        {
            get { return Quantity * UnitCost; }
        }
        
    }

    public class DocumentProductQuantityRow : DocumentQuantityRow
    {
        private Guid ProductID { get; set; } 
    }

    public class SubTotalRow : DocumentCalculatedValueRow
    {
        
        public List<DocumentValueRow> SubRows { get; set; }

        public override decimal Value
        {
            get { return SubRows.Sum(row=>row.Value); }
        }

    }


    public class NetTotal : SubTotalRow
    {

    }

    public class TaxRow : DocumentCalculatedValueRow
    {
        public Decimal Rate { get; set; }

        public override decimal Value
        {
            get { return SourceRow.Value*Rate; }
        }

    }
}
