using System.Collections.Generic;
using System.Linq;

namespace Endless.ComponentModel.Validation
{
    public class RuleAll<T> : Rule<T>
    {
        private readonly IList<Rule<T>> rules = new List<Rule<T>>();

        public RuleAll(Rule<T>[] rules,object error) : base(error) {

            foreach(var rule in rules)
            {
                this.rules.Add(rule);
            }
        }

        public override bool IsValid(T obj)
        {
            return rules.All(r => r.IsValid(obj));
        }

    }
}