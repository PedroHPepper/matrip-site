using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Matrip.Domain.Models.Entities
{
    public class ma15subtripphoto
    {
        [Key]
        public int ma15idsubtripphoto { get; set; }




        [ForeignKey("ma14subtrip")]
        public int FK1514idsubtrip { get; set; }
        public virtual ma14subtrip ma14subtrip { get; set; }
    }
}
