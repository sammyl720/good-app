using System.Collections.Generic;

namespace GoodApp.Data.Models
{
   public class CommentComparer:IEqualityComparer<Deed>
   {
       public bool Equals(Deed x, Deed y)
       {
           return x.CreatorId == y.CreatorId
                  && x.ChallengeId == y.ChallengeId;
       }

       public int GetHashCode(Deed obj)
       {
           return obj.DeedId.GetHashCode();
       }
   }
}
