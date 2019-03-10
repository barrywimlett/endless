using System.Collections.Generic;
using System.Linq;

namespace Endless.ComponentModel.Validation
{
    public class RuleAny<T> : Rule<T>
    {
        private readonly IList<Rule<T>> rules=new List<Rule<T>>();

        public RuleAny(Rule<T>[] rules, object error)
            : base(error)
        {

            foreach (var rule in rules)
            {
                AddRule(rule);
            }
        }

        public void AddRule(Rule<T> rule)
        {
            this.rules.Add(rule);
        }
        public override bool IsValid(T obj)
        {
            return rules.Any(r => r.IsValid(obj));
        }

    }
}