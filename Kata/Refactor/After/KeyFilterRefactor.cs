using System.Collections.Generic;
using System.Linq;

namespace Kata.Refactor.After
{
    public class KeyFilterRefactor
    {
        private ISessionService SessionService { get; set; }
        
        public List<string> Filter(IList<string> marks, bool isGoldenKey)
        {
            var keys = new List<string>();

            if (marks == null || marks.Count == 0)
            {
                return keys;
            }

            if (isGoldenKey)
            {
                keys = AddKeysByType(KeyType.GoldenKey.ToString(), keys);
                marks = ValidateGoldenKeys(marks);
            }
            else
            {
                keys = AddKeysByType(KeyType.SilverKey.ToString(), keys);
                keys = AddKeysByType(KeyType.CopperKey.ToString(), keys);
            }
            
            return marks.Where(mark => keys.Contains(mark) || IsFakeKey(mark)).ToList();
        }

        private IList<string> ValidateGoldenKeys(IList<string> marks)
        {
            var golden02Mark = marks.Where(x => x.StartsWith(CodeType.GD02.ToString()));
            
            foreach (var mark in golden02Mark)
            {
                if (!marks.Any(x => x.StartsWith(CodeType.GD01.ToString()) && mark.Substring(4, 6).Equals(x.Substring(4, 6))))
                {
                    marks.Remove(mark);
                }
            }

            return marks;
        }

        private bool IsFakeKey(string mark)
        {
            return mark.EndsWith(CodeType.FAKE.ToString());
        }
        
        private  List<string> AddKeysByType(string keyType, List<string> keys )
        {
            keys.AddRange(SessionService.Get<List<string>>(keyType));

            return keys;
        }
    }
}