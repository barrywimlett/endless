using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Endless.Windows.Converters
{
    public class IsBusyCursorConverter
    {
        private bool IsExecuting;
            private Cursor Cursor
            {
                get
                {
                    if (this.IsExecuting)
                    {
                        return Cursors.Wait;
                    }
                    else
                    {
                        return Cursors.Hand;
                    }
                }
            }
       
    }
}
