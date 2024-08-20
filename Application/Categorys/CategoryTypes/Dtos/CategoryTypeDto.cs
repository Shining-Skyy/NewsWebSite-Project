using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Categorys.CategoryTypes.Dtos
{
    public class CategoryTypeDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int? ParentTypeId { get; set; }
    }
}
