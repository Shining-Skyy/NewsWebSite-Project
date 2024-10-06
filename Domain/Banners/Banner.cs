using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Banners
{
    public class Banner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public bool IsActive { get; set; }
        public int Priority { get; set; }
        public Position Position { get; set; }
    }

    public enum Position
    {
        /// <summary>
        /// Slider
        /// </summary>
        [Display(Name = "Slider")]
        Slider = 0,
        /// <summary>
        /// first line
        /// </summary>
        [Display(Name = "first line")]
        Line_1 = 1,
        /// <summary>
        /// second line
        /// </summary>
        [Display(Name = "second line")]
        Line_2 = 2,
        /// <summary>
        /// third line
        /// </summary>
        [Display(Name = "third line")]
        Line_3 = 3,
        /// <summary>
        /// fourth line
        /// </summary>
        [Display(Name = "fourth line")]
        Line_4 = 4,
        /// <summary>
        /// fifth line
        /// </summary>
        [Display(Name = "fifth line")]
        Line_5 = 5,
    }
}
